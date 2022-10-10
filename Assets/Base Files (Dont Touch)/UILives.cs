using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILives : MonoBehaviour
{
    private void Start()
    {
        for(int i = 0; i <= 2; i++) //TODO: CHANGE THIS
        {
            if (i >= MainGameManager.Instance.remainingLives) transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
