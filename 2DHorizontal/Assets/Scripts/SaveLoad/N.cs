using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N : MonoBehaviour,IInteractable
{
    [Header("广播")]
    public VoidEventSO saveDataEvent;//加载游戏事件
 
    [Header("变量")]
    public SpriteRenderer spriteRenderer;
    public Sprite sprite1;//图片1
    public Sprite sprite2; //图片2
    private bool isBlack;//是否黑色
    public GameObject lightObj;
     
    void OnEnable()
    {
        spriteRenderer.sprite = isBlack? sprite2 : sprite1;//根据是否黑色显示不同的图片
        lightObj.SetActive(isBlack);//根据是否黑色显示灯光
 
    }
    public void TriggerAction()
    {
        if(!isBlack)//如果灯光是黑色，则切换为蓝色
        {
             isBlack = true;//切换灯光状态
             spriteRenderer.sprite = sprite2;//显示蓝色图片
             lightObj.SetActive(true);
            this.gameObject.tag = "Untagged";//取消Tag
            // SaveGame();
            //TODO:保存游戏
            saveDataEvent.RaiseEvent();//呼叫事件，加载游戏
        }

    }
    // public void SaveGame()
    // {

    // }

 
}
