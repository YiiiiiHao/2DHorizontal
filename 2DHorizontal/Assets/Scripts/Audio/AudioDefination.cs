using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioDefination : MonoBehaviour
{
    public UnityEvent<AudioClip> OnHurt;//意思是：攻击时播放的音效

    public PlayerAudioEventSO playerAudioEvent;//引用PlayerAudioEventSO脚本
    public AudioClip audioClip;//音效文件
    public  bool playerOnEnable;
    
    private void OnEnable()
    {

        if (playerOnEnable)//如果playerOnEnable为true，就播放音效
        {
            PlayerAudioClip();
        }
    }

    public void PlayerAudioClip()
    {
        playerAudioEvent.RaiseEvent(audioClip);//调用PlayerAudioEventSO脚本的OnEventRaisedd方法，传入音效文件
    }   

}
