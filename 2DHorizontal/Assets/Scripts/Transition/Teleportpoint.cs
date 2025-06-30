using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleportpoint :  MonoBehaviour , IInteractable 
{ 
    public SceneLoadEventSO loadEventSO;
    public GameSceneSO sceneToGo;// 传送的场景
    public Vector3 positionToGo;// 传送的位置

    public void TriggerAction()
    {
        Debug.Log("传送");

        loadEventSO.RaiseLoadRequestEvent(sceneToGo, positionToGo,true);
    }

}
