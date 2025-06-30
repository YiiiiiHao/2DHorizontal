using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
public class AudioManager : MonoBehaviour
{   
    [Header("监听事件")]

    public PlayerAudioEventSO FXEvent;
    public PlayerAudioEventSO BGMEvent;
    [Header("音效组件")]
    public AudioSource BGMSource;
    public AudioSource FXSource;  

    private void OnEnable()
    {
       FXEvent.OnEventRaisedd += OnFXEvent;
      BGMEvent.OnEventRaisedd += OnBGMEvent;
    }


    private void OnDisable()
    {
        FXEvent.OnEventRaisedd -= OnFXEvent;
         BGMEvent.OnEventRaisedd -= OnBGMEvent;
        
    }
    private void OnFXEvent(AudioClip clip)
    {
       FXSource.clip = clip;
       FXSource.Play();//播放音效

    }
    private void OnBGMEvent(AudioClip arg0)
    {
        //游戏开始就循环播放背景音乐
        BGMSource.clip = arg0;
        BGMSource.Play();
    }

    
}
