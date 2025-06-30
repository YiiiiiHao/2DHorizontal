using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class DataManager : MonoBehaviour
{
    [Header("监听数据")]
    public VoidEventSO saveDataEvent;
    public VoidEventSO loadDataEvent;
   public static DataManager instance;  //单例
   private List<ISaveable> saveableList = new List<ISaveable>();//保存所有需要保存数据的对象 PS：列表语法
   private Data saveData;
   private void Awake()
   {
       //这是单例通用的代码 确保场景中有 且 只有一个DataManager实例
      if (instance == null)//如果没有实例
      {
        instance = this;//设置唯一的DataManager实例
      }
      else//如果有实例
      {
        Destroy(this.gameObject);//销毁这个实例
      }
     
      saveData = new Data();
   }
   private void Update()
   {
        if(Keyboard.current.lKey.wasPressedThisFrame)//按下L键保存数据,括号内的 LKey的L，可以按需求换成其他按键，比如：qkey，zkey等
        {
            Load();
        }
   }
   public void OnEnable()
   {
        saveDataEvent.OnEventRaised += Save;//注册事件监听
        loadDataEvent.OnEventRaised += Load;//注册事件监听
   }
    public void OnDisable()
    {
        saveDataEvent.OnEventRaised -= Save;//注销事件监听
        loadDataEvent.OnEventRaised -= Load;//注销事件监听
    }
    public void RegisterSaveaDate(ISaveable saveable)//注册保存数据接口,统一通知所有对象保存数据 或 加载数据
    {
        if(!saveableList.Contains(saveable))//去到每个场景会注册一次，但防止重复注册，这里做个判断
        {
            saveableList.Add(saveable);//如果没有，就添加进去
        }
    
    }

    public void UnRegisterSaveDate(ISaveable saveable)//注销保存数据接口,统一通知所有对象注销保存数据 或 加载数据
    {
        saveableList.Remove(saveable);//从保存列表中移除
    }

    public void Save()//保存数据
    { 
        foreach(var saveable in saveableList)
        {
            saveable.GetSaveDate(saveData);
        }
        foreach(var item in saveData.characterPosDrict)
        {
            Debug.Log(item.Key+"   "+item.Value);
        }
    }

    public void  Load()//加载数据
    {
        foreach(var saveable in saveableList)
        {
            saveable.loadData(saveData);
        }

    }
}
