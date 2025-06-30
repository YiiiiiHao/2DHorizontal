using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;

public class Sign : MonoBehaviour
{ 
    private PlayerInputControl playerInput;//获取PlayerInputControl脚本
    public Transform playerTransform;
    private Animator anim;
    public GameObject SignSprite;
    private IInteractable targetItem;//目标物体接口
    public bool canPress;

    private void Awake()
    {
        anim = SignSprite.GetComponent<Animator>();//获取Animator组件
        playerInput = new PlayerInputControl();//创建PlayerInputControl脚本实例
        playerInput.Enable();//启动PlayerInputControl脚本


    } 
    private void OnEnable()//当脚本激活时调用
    {
        InputSystem.onActionChange += OnActionChange;//注册事件监听, 监听PlayerInputControl脚本的OnActionChange事件
        playerInput.GamePlay.Confirm.started += OnConfirm;//注册事件监听, 监听PlayerInputControl脚本的Confirm.started事件

    }

    private void OnDisable()
    {
        canPress = false;
    }
    private void OnConfirm(InputAction.CallbackContext context)
    {
        if(canPress)//canPress为true时，才可以按下确认键
        {
            targetItem.TriggerAction();//调用Interactable物体的TriggerAction方法
            GetComponent<AudioDefination>().PlayerAudioClip();//播放音效
        }
    }

    /// <summary>
    /// 设备输入的切换//在这里没写，但在教程里教了
    /// </summary>s
    /// <param name="obj"></param>
    /// <param name="actionchange"></param>
    private void OnActionChange(object obj, InputActionChange actionchange)
    {
        if(actionchange == InputActionChange.ActionStarted)
        {
            // Debug.Log(((InputAction)obj).activeControl.device);
            var d = ((InputAction)obj).activeControl.device;//获取当前设备
            switch(d.device)//根据设备类型，播放动画
            {
                case Keyboard://键盘设备
                anim.Play("Button_F");//播放Button_F动画
                break;//结束switch语句

            }
        }
    }

    private void Update()
    {
        SignSprite.GetComponent<SpriteRenderer>().enabled = canPress;//获取SpriteRenderer脚本并设置是否激活
        SignSprite.transform.localScale = playerTransform.localScale;//保持跟随玩家的大小
    }
    //生命周期：OnTriggerStay2D() -> OnTriggerEnter2D() -> OnTriggerExit2D()
    private void OnTriggerStay2D(Collider2D other)//生命周期：进入触发器范围内时触发
    {
        if(other.CompareTag("Interactable"))//碰撞到Interactable物体时，canPress为true
        {
            canPress = true;
            targetItem = other.GetComponent<IInteractable>();//获取碰撞到的Interactable物体的接口
        }
        if(!other.CompareTag("Interactable"))
        {
            canPress = false;
        }
    }
    private void OnTriggerExit2D(Collider2D other)// 生命周期：离开触发器范围内时触发
    {                               
        if (other.CompareTag("Interactable"))//离开Interactable物体时，canPress为false
        {
            canPress = false;
        }
    }
   

}
