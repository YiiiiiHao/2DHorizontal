using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour,IInteractable
{

    private SpriteRenderer spriteRenderer;
    public Sprite openSprite;//图片A
    public Sprite closedSprite;//图片B
    public bool isDome;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
 
    }
    private void OnEnable()//
    {
        spriteRenderer.sprite =isDome?openSprite:closedSprite;//判断是宝箱图片A还是B，并赋值给spriteRenderer
    }
          
    public void TriggerAction()
    {
        if(!isDome)//只有没被打开过的宝箱，可以打开
        {
        // Debug.Log("+++++++++++++++Open Chest++++++++++++++++++++Open Chest+++++++++++++++Open Chest+++++++++++++++Open Chest+++++++++++++++"  );
            OpenChest();
        }
     }

     public void OpenChest()
     {
         spriteRenderer.sprite = openSprite;
         isDome = true; 
         this.gameObject.tag = "Untagged";//宝箱打开后，将其tag设置为Untagged，防止它被其他物体碰撞触发 
                       

     }
 
}
