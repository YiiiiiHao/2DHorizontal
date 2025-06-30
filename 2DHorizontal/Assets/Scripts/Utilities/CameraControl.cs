using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;
using System;

public class CameraControl : MonoBehaviour
{
    [Header("监听事件")]
    public VoidEventSO afterSceneLoadEvent;
     private CinemachineConfiner2D confiner2D;//获取方
     public CinemachineImpulseSource impulseSource;//获取振动源
     public VoidEventSO cameraShakeEvent;//获取摇摆事件

     private void Awake()
     {
         confiner2D = GetComponent<CinemachineConfiner2D>();//获取方的组件
     }

    private void  OnEnable()
     {
         cameraShakeEvent.OnEventRaised += OnCameraShakeEvent;//注册摇摆事件
         afterSceneLoadEvent.OnEventRaised += OnAfterSceneLoadEvent;//注册场景切换事件
     }
    private void OnDisable()
     {
         cameraShakeEvent.OnEventRaised -= OnCameraShakeEvent;//注销摇摆事件
         afterSceneLoadEvent.OnEventRaised -= OnAfterSceneLoadEvent;//注销场景切换事件


     }

    private void OnAfterSceneLoadEvent()
    {
       GetNewCameraBounds();
    }

    private void OnCameraShakeEvent()
    {
        impulseSource.GenerateImpulse();
    }
 
 

    //  void Start()
    //  {
    //      GetNewCameraBounds();
    //  }

    private void GetNewCameraBounds()//通过调用此获取新相机边界组件
    {
        var obj = GameObject.FindGameObjectWithTag("Bounds");//获取Bounds标签下的游戏对象，不能拼错
         if (obj == null)
         {
            //  Debug.Log("Bounds not found!");
             return;//如果没找到Bounds，就停止代码
         }
         confiner2D.m_BoundingShape2D = obj.GetComponent<Collider2D>();//获取Bounds组件的Collider2D，并赋值给方的边界组件
         confiner2D.InvalidateCache();//清理缓存
    }

}
