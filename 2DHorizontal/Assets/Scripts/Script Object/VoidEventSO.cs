using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(menuName = "Event/VoidEventSO")]
public class VoidEventSO : ScriptableObject
{
    public UnityAction OnEventRaised;//This is the event

    public void RaiseEvent()//这是事件的触发函数
    {
        OnEventRaised?.Invoke();//意思是：如果OnEventRaised不为空，则调用OnEventRaised.Invoke()
    }

}
