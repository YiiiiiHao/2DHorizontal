using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SceneLoader : MonoBehaviour,ISaveable
{ 
    public Transform playerTrans;
    public Vector3 firstPosition;
    public Vector3 menuPosition;
    [Header("事件监听")]
    public SceneLoadEventSO loadEventSO;
    public VoidEventSO newGameEvent;
    public VoidEventSO backToMenuEvent;

    [Header("广播")]
    public VoidEventSO afterSceneLoadEvent;//场景加载完成事件
    public FadeEventSO fadeEvent;//淡入淡出事件
    public SceneLoadEventSO unloadedSceneEvent;//场景卸载完成事件
    [Header("场景")]
    public GameSceneSO firstLoadScene;
    public GameSceneSO menuScene;
    private GameSceneSO currentLoadedScene;//当前加载的场景
    public Vector3 positionToGo;
    private GameSceneSO sceneToLoad;
    private bool fadeScreen;
    private bool isLoading;
    public float fadeDuration;
    private void Awake()
    {
        // Addressables.LoadSceneAsync(Scene_1.sceneRefence/*这是你要加载的场景名字*/,LoadSceneMode.Additive/*这是加载模式*/);

        // currentLoadedScene = Scene_1;
        // currentLoadedScene.sceneRefence.LoadSceneAsync(LoadSceneMode.Additive);
 

    }
    //TODO 做完main menu后，这里可以改成从main menu加载
    private void Start()
    {
        loadEventSO.RaiseLoadRequestEvent(menuScene, menuPosition, true);
        // NewGame();
        
    }


    private void OnbackToMenuEvent()
    {
         sceneToLoad = menuScene;//加载菜单场景
         loadEventSO.RaiseLoadRequestEvent(sceneToLoad, menuPosition, true);

    }


    private void NewGame()
    {
        sceneToLoad = firstLoadScene;
        // OnLoadRequestEvent(sceneToLoad,firstPosition,true);
        loadEventSO.RaiseLoadRequestEvent(sceneToLoad, firstPosition, true);
         newGameEvent.OnEventRaised -= NewGame;

    }
    /// <summary>
    /// 场景加载事件请求
    /// </summary>
    /// <param name="locationToLoad"></param>
    /// <param name="posToGo"></param>
    /// <param name="fadeScreen"></param>
    private void OnLoadRequestEvent(GameSceneSO locationToLoad, Vector3 posToGo, bool fadeScreen)
    {
        if(isLoading)//如果正在加载中
        {
            return;//返回,不再执行下面代码,防止玩家一直按E，导致重复加载
        }
        isLoading = true;
        sceneToLoad = locationToLoad;
        positionToGo = posToGo;
        this.fadeScreen = fadeScreen;
         if(currentLoadedScene!= null)//如果上一个场景不为空
        {
            StartCoroutine(UnLoadPreviousScene());//开始卸载上一个场景
        }
        else//如果上一个场景为空
        {
            LoadNewScene();//加载新场景
        }
        // Debug.Log(sceneToLoad.sceneRefence.SubObjectName);
        // StartCoroutine(UnLoadPreviousScene());
     }

     private IEnumerator UnLoadPreviousScene()
     {
        if(fadeScreen)
        {
            //TODO   变黑
            fadeEvent.FadeIn(fadeDuration);
        }
        yield return new WaitForSeconds(fadeDuration);//等待淡出效果结束
        //广播 血条显示 事件
        unloadedSceneEvent.RaiseLoadRequestEvent(sceneToLoad, positionToGo, true);//广播场景卸载完成事件
       
        yield return currentLoadedScene.sceneRefence.UnLoadScene();//卸载上一个场景
    
        playerTrans.gameObject.SetActive(false);//隐藏玩家，关闭人物
        //加载新场景
        LoadNewScene();
     }

        private void OnEnable()
    {
        ISaveable saveable =this;        
        loadEventSO.LoadRequesetEvent += OnLoadRequestEvent; 
        newGameEvent.OnEventRaised += NewGame;
        backToMenuEvent.OnEventRaised += OnbackToMenuEvent;
        // saveable.RegisterSaveaDate();
        

    }


    private void OnDisable()
    {
         ISaveable saveable =this;
         loadEventSO.LoadRequesetEvent -= OnLoadRequestEvent; 
         newGameEvent.OnEventRaised -= NewGame;
         backToMenuEvent.OnEventRaised -= OnbackToMenuEvent;
         saveable.UnRegisterSaveDate();



    }

    private void LoadNewScene()//加载新场景
    {
        // sceneToLoad.sceneRefence.LoadSceneAsync(LoadSceneMode.Additive,true);
    //  | |
    //  | |              loadingOption是一个临时变量，用来查看返回值是什么
    //   V
        var loadingOption = sceneToLoad.sceneRefence.LoadSceneAsync(LoadSceneMode.Additive,true);
        loadingOption.Completed+=OnLoadCompleted;
    }
    /// <summary>
    /// 场景加载结束后
    /// </summary>
    /// <param name="handle"></param>
    private void OnLoadCompleted(AsyncOperationHandle<SceneInstance> handle)
    {
        currentLoadedScene = sceneToLoad;//更新当前加载的场景

        playerTrans.position = positionToGo;//将玩家位置设置为目标位置
        playerTrans.gameObject.SetActive(true);//显示玩家
        if(fadeScreen)
        {
            //TODO  变透明
            fadeEvent.FadeOut(fadeDuration);
        }
        isLoading = false;
 
        if(currentLoadedScene.sceneType!=SceneType.Menu)//如果是游戏场景，则执行下面的代码
        {
        afterSceneLoadEvent.RaiseEvent();   //广播场景加载完成事件
        }
    }

    public DataDefinition GetDataID()
    {
        return GetComponent<DataDefinition>();//返回数据ID
     }

    public void GetSaveDate(Data data)
    {
         data.SaveGameScene(currentLoadedScene);
         
     }
    
    public void loadData(Data data)
    {
        var playerID = playerTrans.GetComponent<DataDefinition>().ID;
       if(data.characterPosDrict.ContainsKey(playerID))
       {
        positionToGo = data.characterPosDrict[playerID];
        sceneToLoad = data.GetSavedScene();
        OnLoadRequestEvent(sceneToLoad,positionToGo,true);
       }
    }
}

