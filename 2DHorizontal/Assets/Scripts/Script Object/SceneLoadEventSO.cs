using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(menuName ="Event/SceneLoadEventOS")]
public class SceneLoadEventSO : ScriptableObject 
{
  public UnityAction<GameSceneSO,Vector3,bool> LoadRequesetEvent;

/// <summary>
/// 场景加载请求
/// </summary>
/// <param name="需要加载的场景名字"></param>
/// <param name="场景坐标"></param>
/// <param name="是否渐入渐出"></param>
  public void RaiseLoadRequestEvent(GameSceneSO locationToLoad,Vector3 posToGo,bool fadeScreen)
  {
    LoadRequesetEvent?.Invoke(locationToLoad,posToGo,fadeScreen) ;
  }
}