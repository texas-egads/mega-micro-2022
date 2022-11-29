using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    public TMPro.TextMeshProUGUI text;

    private void Awake() {
        Deactivate();
    }

    public void Activate() {
        // do nothing lmao
    }

    public void Deactivate() {
        text.text = "";
    }

    public void ShowTime(float time) {
        text.text = "" + Mathf.CeilToInt(time);
    }
}
