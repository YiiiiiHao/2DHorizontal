using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Menu : MonoBehaviour
{
    public GameObject newGameButton;

    private void OnEnable()//选中新游戏按钮
    {
        EventSystem.current.SetSelectedGameObject(newGameButton);
    }
    public void ExitGame()//退出游戏
    {
        Debug.Log("Exit game");
        Application.Quit();
    }
}
