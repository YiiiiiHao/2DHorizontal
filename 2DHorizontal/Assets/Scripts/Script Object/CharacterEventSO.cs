using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//创建一个资产菜单，命名为CharacterEventSO
[CreateAssetMenu(menuName ="Event/CharacterEventSO")]//Create a new scriptable object
 
public class CharacterEventSO :ScriptableObject
{ 
    public UnityAction <Character> OnEventRaised;//事件订阅,需要把 “Character” 作为参数传出去。

    public void RaiseEvent(Character character)//事件调用
    {
         OnEventRaised?.Invoke(character);//调用事件订阅函数
    }
}
 
 