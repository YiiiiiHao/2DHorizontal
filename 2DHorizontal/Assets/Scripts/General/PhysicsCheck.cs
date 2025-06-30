using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{ 
    private CapsuleCollider2D coll ;
    // private BoxCollider2D boxColl;

    [Header("参数")]
    public bool manual; //是否手动设置检测范围
    public Vector2 bottemOffset; //角色中心点位移偏量，
    public Vector2 leftOffset; //角色左侧偏移量
    public Vector2 rightOffset; //角色右侧偏移量
    public float checkRaduis ;//检测半径
    public LayerMask groundLayer;//地面层，层遮罩
    [Header("状态")]
    public bool isGround;//检测是否与地面碰撞
    internal bool isDead;//是否死亡

    public bool touchLeftWall; //是否碰撞左墙
    public bool touchRightWall; //是否碰撞右墙


    void Awake()
    {
        coll = GetComponent<CapsuleCollider2D>();
 
        if(!manual)
        {
            rightOffset = new Vector2((coll.bounds.size.x+coll.offset.x)/2,coll.bounds.size.y/2);
            leftOffset = new Vector2(-rightOffset.x,rightOffset.y);
        
        }

    }

    void Update() 
    {
        check();
    }
    public void check()
    {  
        if(transform.localScale.x>0)
        {
         //检测是否与地面碰撞                               中心点        +偏移量           半径      层遮罩
        isGround = Physics2D.OverlapCircle((Vector2)transform.position+bottemOffset, checkRaduis, groundLayer);//检测是否与地面碰撞
            
        }
        if(transform.localScale.x<0)
        {
            isGround = Physics2D.OverlapCircle((Vector2)transform.position+-bottemOffset, checkRaduis, groundLayer);
        }

        //墙体检查
        touchLeftWall = Physics2D.OverlapCircle((Vector2)transform.position+leftOffset, checkRaduis, groundLayer); //检测是否与左墙碰撞
        touchRightWall = Physics2D.OverlapCircle((Vector2)transform.position+rightOffset, checkRaduis, groundLayer); //检测是否与右墙碰撞
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere((Vector2)transform.position+bottemOffset, checkRaduis); //绘制检测范围  
        Gizmos.DrawWireSphere((Vector2)transform.position+leftOffset, checkRaduis); //绘制检测范围
        Gizmos.DrawWireSphere((Vector2)transform.position+rightOffset, checkRaduis); //绘制检测范围
    }
}
