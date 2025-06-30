using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attack : MonoBehaviour
{
    public int damage;//伤害值
    public float attackRange;//攻击范围
    public float attackRate;//攻击频率 
    private PlayerController playerController;
    public SpriteRenderer SR; 
    private void OnTriggerStay2D(Collider2D other)
    {
      other.GetComponent<Character>()?.TakeDamage(this);//“？”表示如果other.GetComponent<Character>()为空，则什么也不做，否则调用TakeDamage方法
    

    } 
    
}
