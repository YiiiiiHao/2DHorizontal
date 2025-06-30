using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Fade Event")]
public class FadeEventSO : ScriptableObject
{ 
    public UnityAction<Color,float,bool> OnEventRaised;
    /// <summary>
    /// 逐渐变黑
    /// </summary>
    /// <param name="duration"></param>
    public void FadeIn(float duration)//让颜色逐渐变黑
    {
        RaiseEvent(Color.black, duration, true);//调用事件
    }

    /// <summary>
    /// 逐渐变透明
    /// </summary>
    /// <param name="duration"></param>
    public void FadeOut(float duration)
    {
        RaiseEvent(Color.clear, duration, false);//调用事件
    }

    public void RaiseEvent(Color target, float duration, bool fadeIn)
    {
        OnEventRaised?.Invoke(target, duration, fadeIn);
    }

}
