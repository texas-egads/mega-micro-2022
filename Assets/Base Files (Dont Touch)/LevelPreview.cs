using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

public class LevelPreview : MonoBehaviour
{
    private Animator _animator;
    [SerializeField] private Image image;
    [SerializeField] private Image mask;
    public static LevelPreview instance;

    private void Awake()
    {
        if(instance == null) instance = this;
        else Destroy(gameObject);
        _animator = GetComponent<Animator>();
        mask.enabled = false;
        image.enabled = false;
        _animator.Play("mask-small-idle");
    }

    public void HandleLevelPreview(bool grow)
    {
        mask.enabled = true;
        image.enabled = true;
        _animator.Play(grow ? "mask-grow" : "mask-shrink");
        StartCoroutine(DisableImage());
    }

    private IEnumerator DisableImage()
    {
        yield return new WaitForSeconds(MainGameManager.halfBeat);
        yield return null;
        mask.enabled = false;
        image.enabled = false;
    }
    
}
