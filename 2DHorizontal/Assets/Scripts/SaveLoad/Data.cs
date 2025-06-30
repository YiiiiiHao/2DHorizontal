using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data 
{ 
    public string sceneToSave; //场景名称
    //字典
    public Dictionary<string,Vector3> characterPosDrict = new Dictionary<string, Vector3>(); //角色位置字典
    public Dictionary<string,float> floatSavedData = new Dictionary<string, float>(); //保存的浮点数数据

    public void SaveGameScene(GameSceneSO savedScene)//保存游戏数据
    {
        sceneToSave = JsonUtility.ToJson(savedScene); //保存场景名称
        Debug.Log(sceneToSave);
    }

    public GameSceneSO GetSavedScene()
    {
        var newScene = ScriptableObject.CreateInstance<GameSceneSO>(); //创建新的场景对象
        JsonUtility.FromJsonOverwrite(sceneToSave, newScene); //从json数据中恢复场景数据
        return newScene; //返回场景对象
    }
}
