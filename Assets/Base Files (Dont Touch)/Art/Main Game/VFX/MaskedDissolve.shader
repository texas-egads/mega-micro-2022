Shader "Particles/MaskedDissolve"
{
    Properties
    {
        _Mask("Texture Mask", 2D) = "white" {}
        _NoiseFrag("Dissolve Noise Texture", 2D) = "white" {}
        _Scale("Dissolve Noise Scale", Range(0,2)) = 0.5
        _ExtraNoise("Overlay Noise Texture", 2D) = "white" {}
        _Color("Noise Color", Color) = (1,0.5,0,0)
        _ExtraScale("Overlay Noise Scale", Range(0,2)) = 0.5
        _Tint("Tint", Color) = (1,1,0,0)
        _EdgeColor("Edge", Color) = (1,0.5,0,0)
        _Fuzziness("Fuzziness", Range(0,2)) = 0.6
        _Stretch("Dissolve Stretch", Range(0,4)) = 0.4
        _Delay("Dissolve Delay", Range(0,2)) = 0
        [Enum(UnityEngine.Rendering.BlendOp)] _BlendOp("Blend Op", Int) = 0// 0 = add, 4 = max, other ones probably won't look good
    }
        SubShader
        {
            Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" "PreviewType" = "Plane" }
            Blend One OneMinusSrcAlpha
            ColorMask RGB
            Cull Off Lighting Off ZWrite Off
            ZTest Always
            BlendOp[_BlendOp]

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                // make fog work
                #pragma multi_compile_fog

                #include "UnityCG.cginc"

                struct appdata
                {
                    float4 vertex : POSITION;
                    float3 uv : TEXCOORD0;// .z has particle age
                    float4 color : COLOR;
                    float4 normal :NORMAL;
                };

                struct v2f
                {
                    float3 uv : TEXCOORD0; // .z has particle age
                    UNITY_FOG_COORDS(1)
                    float4 vertex : SV_POSITION;
                    float4 color: COLOR;
                };

                sampler2D _Mask, _NoiseFrag, _ExtraNoise;
                float4 _Mask_ST, _Tint, _EdgeColor, _Color;
                float  _Scale, _Fuzziness, _Stretch;
                float _Delay, _ExtraScale;

                v2f vert(appdata v)
                {
                    v2f o;
                    o.uv.xy = TRANSFORM_TEX(v.uv.xy, _Mask);
                    UNITY_TRANSFER_FOG(o,o.vertex);
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv.z = v.uv.z - _Delay;// subtract a number to delay the dissolve
                    o.color = v.color;
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {

                    half4 mask = tex2D(_Mask, i.uv.xy);// sprite mask
                    half4 noise = tex2D(_NoiseFrag, i.uv.xy  * _Scale);// dissolve texture
                    half4 extraTexture = tex2D(_ExtraNoise, i.uv.xy * _ExtraScale);// extra overlay texture
                    float combinedNoise = (noise.r + extraTexture.r) / 2; // combining noise for a more interesting result
                    float dissolve = smoothstep(_Stretch * i.uv.z, _Stretch * i.uv.z + _Fuzziness, combinedNoise);// smooth dissolve
                    float4 color = lerp(_EdgeColor,_Tint, dissolve) ;// lerp the color over the dissolve
                    color += (extraTexture * _Color); // add extra texture (colored)
                    color *= dissolve; // multiply with dissolve so theres no bleeding
                    color *= i.color;// multiply with the color over time
                    color *= mask.a; // multiply with the sprite alpha
                    // apply fog
                    UNITY_APPLY_FOG(i.fogCoord, color);
                    return color;

                }
                ENDCG
            }
        }
}
