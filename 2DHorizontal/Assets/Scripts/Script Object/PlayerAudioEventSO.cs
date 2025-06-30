using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

  [CreateAssetMenu(menuName = "Event/PlayerAudioEvent")] 
public class PlayerAudioEventSO :ScriptableObject
{
    public UnityAction<AudioClip> OnEventRaisedd;//一个Action，可以传递一个AudioClip进来
    public void RaiseEvent(AudioClip clip)//放一个音乐片段进来
    {
        OnEventRaisedd?.Invoke(clip);//调用Action
    }

}
