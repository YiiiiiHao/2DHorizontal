using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;


public class FadeCanvas : MonoBehaviour
{ 
    [Header("监听事件")]
    public FadeEventSO fadeEvent;
    public Image fadeImage;

    private void OnEnable()
    {
        fadeEvent.OnEventRaised += OnFadeEvent;
    }


    private void OnDisable()
    {
        fadeEvent.OnEventRaised -= OnFadeEvent;

    }
    private void OnFadeEvent(Color targer,float duration,bool fadeIn)//目标颜色 持续时间
    {
        fadeImage.DOBlendableColor(targer, duration);
        
    }
}
