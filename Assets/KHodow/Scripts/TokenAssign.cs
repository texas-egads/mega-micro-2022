using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KHodow
{
    public class TokenAssign : MonoBehaviour
    {
        [SerializeField] private List<GameObject> tokens;
        [SerializeField] private GameObject tokenBacks;
        private void Start()
        {
            foreach(TokenResponse tokenBack in tokenBacks.GetComponentsInChildren<TokenResponse>())
            {
                int index = Random.Range(0, tokens.Count);
                tokenBack.tokenVariant = tokens[index];
                tokens.Remove(tokens[index]);
            }
        }
    }
}

