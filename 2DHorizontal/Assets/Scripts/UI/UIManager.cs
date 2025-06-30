using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    public PlayerStartBar PlayerStartBar;
    [Header("事件监听")]

    public CharacterEventSO healthEvent;
    public SceneLoadEventSO unloadedSceneEvent; 
    public VoidEventSO loadDataEvent;
    public VoidEventSO gameOverEvent;
    public VoidEventSO backToMenuEvent;

    [Header("组件")]
    public GameObject gameOverPanel;//游戏结束面板
    public GameObject restartBtn;//重新开始按钮


    private void OnEnable()//注册事件
    {
        healthEvent.OnEventRaised += OnHealthEvent;
        unloadedSceneEvent.LoadRequesetEvent += OnUnLoadedSceneEvent;
        loadDataEvent.OnEventRaised += OnLoadDataEvent;
        gameOverEvent.OnEventRaised += OnGameOverEvent;
        backToMenuEvent.OnEventRaised += OnLoadDataEvent;
        // backToMenuEvent.OnEventRaised = backToMenuEvent.OnEventRaised + OnLoadDataEvent;
    }


    private void OnDisable()//注销事件
    {
        healthEvent.OnEventRaised -= OnHealthEvent;
        unloadedSceneEvent.LoadRequesetEvent -= OnUnLoadedSceneEvent;
        loadDataEvent.OnEventRaised -= OnLoadDataEvent;
        gameOverEvent.OnEventRaised -= OnGameOverEvent;
        backToMenuEvent.OnEventRaised -= OnLoadDataEvent;

    }
    private void OnGameOverEvent()
    {
        gameOverPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(restartBtn);//设置焦点到重新开始按钮
     }
    private void OnLoadDataEvent()
    {
       gameOverPanel.SetActive(false);
    }
    private void OnUnLoadedSceneEvent(GameSceneSO sceneToLoad, Vector3 arg1, bool arg2)
    {
        var isMenu = sceneToLoad.sceneType == SceneType.Menu;
        PlayerStartBar.gameObject.SetActive(!isMenu);//隐藏血条
         
    }

    private void OnHealthEvent(Character character)
    {
        //血条百分比 = 当前血量 / 最大血量
       var percentage = character.currentHealth / character.maxHealth; 
 
        PlayerStartBar.OnHealthChange(percentage);
    }
}
