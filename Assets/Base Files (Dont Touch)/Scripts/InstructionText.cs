using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class InstructionText : MonoBehaviour
{
    private TextMeshProUGUI text;
    private RectTransform textRect;

    private void Awake() {
        textRect = transform as RectTransform;
        text = GetComponent<TextMeshProUGUI>();
        text.enabled = false;
    }

    public void ShowImpactText(string instruction)
    {
        text.text = instruction + "!";
        text.enabled = true;
        textRect.localScale = Vector3.one * 2;

        Sequence tween = DOTween.Sequence();
        tween.Append(textRect.DOScale(Vector3.one, 0.2f));
        tween.AppendInterval(1f);
        tween.AppendCallback(() => text.enabled = false);
    }
}
