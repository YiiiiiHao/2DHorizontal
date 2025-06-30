using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;
    public float speed;
    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;
    private PlayerController playerController;
    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();//获取动画组件
        rb =  GetComponent<Rigidbody2D>();//获取刚体组件
        physicsCheck = GetComponent<PhysicsCheck>();//获取物理检测组件
        playerController = GetComponent<PlayerController>();//获取玩家控制器组件
        //anim = gameObject.GetComponent<Animator>();
 

    }
    void Update()
    {
        SetAnimation();
    }
 

    public void SetAnimation()
    {
        anim.SetFloat("velocityX",Mathf.Abs(rb.velocity.x));//Mathf.Abs:获取绝对值
        anim.SetFloat("velocityY",rb.velocity.y); //获取y轴速度
        anim.SetBool("isGround",physicsCheck.isGround);//判断是否在地面上 
        anim.SetBool("isDead", playerController.isDead); //判断是否死亡
        //将Animator组件中名为"isAttack"的布尔参数的值设置为玩家控制器（playerController）的isAttack属性的值。
        //这样，当玩家开始攻击时（即playerController.isAttack变为true），
        //动画状态机就会根据这个变化来触发相应的攻击动画。
        anim.SetBool("isAttack", playerController.isAttack);  
        anim.SetBool("isCrouch",playerController.isCrouch); //判断是否下蹲
    } 

    public void PlayHurt()
    {
        anim.SetTrigger("hurt");
    }

    public void PlayAttack()
    {

        anim.SetTrigger("attack");
    }
}
