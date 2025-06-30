using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rb;
    [HideInInspector]public Animator anim;//protected:子类可以访问
    [HideInInspector]public PhysicsCheck physicsCheck;
     [Header("基本参数")]
    public float nomalSpeed;//正常速度
    public float chaseSpeed;//追逐速度
    [HideInInspector]public float currentSpeed; //当前速度
    public Transform attacker; //攻击者
    public Vector3 faceDri; //面朝方向

    [Header("检测")]
    public Vector2 conterOffset; //反弹偏移
    public Vector2 checkSize;//检测范围
    public float checkDistancs; //检测距离
    public LayerMask attackLayer; //攻击层

    public float hurtForce; //受伤时所受的击退力
    [Header("计时器")]
    public float waitTime; //等待时间
    public float waitTimeCounter; //当前时间
    public bool wait; //等待状态
    [Header("状态")]
    public bool isHurt; //受伤状态
    public bool isDead; //死亡状态
    
    //调用抽象类
    private BaseState currentState;//当前状态
    protected BaseState patrolState;//巡逻状态
    protected BaseState chaseState;//追逐状态 

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim =GetComponent<Animator>();
        physicsCheck = GetComponent<PhysicsCheck>();

        currentSpeed =nomalSpeed;
        waitTimeCounter = waitTime;

  
        
    }
    private void OnEnable()//激活时调用
    {
        currentState = patrolState;//当前状态 进入到 巡逻状态
        currentState.OnEnter(this);
    }
    private void Update()
    {
        faceDri = new Vector3(-transform.localScale.x, 0, 0) ;//朝向有三个参数，所以用Vector3表示，如果只有横轴，竖轴，就可以用Vector2

        currentState.LogicUpdate();
        TimeCounter();
    
        // if(physicsCheck.touchLeftWall || physicsCheck.touchRightWall || faceDri .x >0)
        // {
        //     transform.localScale = new Vector3(-faceDri.x * 1, 1)    ;
        // }
 
    }
    private void FixedUpdate()
    {
        if((!isHurt&&!isDead&& !wait))//如果没有受伤才能移动
        {
        Move(); 
        }
 
        currentState.physicsUpdate();//调用当前状态的物理更新方法
    }

    public void OnDisable()
    {
        currentState.OnExit();
    }

    public virtual void Move()//virtual:子类可以重写该方法
    {
        rb.velocity = new Vector2(currentSpeed * faceDri.x * nomalSpeed * Time.deltaTime, rb.velocity.y);
        anim.SetBool("Walk",true);
    }

    public void TimeCounter()//计时器
    {
        if(wait)
        {
            waitTimeCounter -= Time.deltaTime;
            if(waitTimeCounter<=0)
            {
                transform.localScale = new Vector3(faceDri.x * 1, 1);   
                wait = false;
                waitTimeCounter = waitTime;
            }
        }

    }

    public bool foundPlayer()
    {
        return Physics2D.BoxCast(transform.position+(Vector3)conterOffset,checkSize,0,faceDri,checkDistancs,attackLayer);
    }

    public void SwitchState(NPCState state)
    {
        var newState = state switch
        {
            NPCState.Patrol => patrolState,//这行代码意思是：如果state等于Patrol，则将当前状态设置为patrolState
            NPCState.Chase => chaseState,//如果state等于Chese，则将当前状态设置为chaseState
            _=>null,//如果没有匹配到任何状态，则返回null

        };
        currentState.OnExit();
        currentState = newState;
        currentState.OnEnter(this);
    }


    #region  事件执行方法
    public void OnTakeDamage(Transform attackerTrans)//在受到伤害时调用 attackerTrans:攻击者的Transform
    {
        attacker = attackerTrans;
        //转身
        if(attacker.position.x-transform.position.x>0)
        {
            transform.localScale = new Vector3(-1,1,1);
        }
        if(attacker.position.x-transform.position.x<0)
        {
            transform.localScale = new Vector3(1,1,1);
        }
        //受伤被击退
        isHurt = true;
        anim.SetTrigger("Hurt");//播放受伤动画

        //添加一个反方向的力
        Vector2 Dri = new Vector2(transform.position.x-attacker.position.x,0).normalized;
        OnHurt(Dri);
        StartCoroutine(OnHurt(Dri));
       
    }
    IEnumerator OnHurt(Vector2 Dri)//因为Dri是局域性变量，无法在此被访问，所以作为参数被传递进来
    {
        rb.AddForce(Dri*hurtForce,ForceMode2D.Impulse );
        yield return new WaitForSeconds(0.5f);//受攻击后等待时间
         isHurt = false;
    }

    public void OnDead()//死亡
    {      
            gameObject.layer = 2;
            
            anim.SetBool("Dead",true);
            isDead = true;
    }

    public void DestroyAfterAnimation()//动画播放完后销毁    //在Unity中调用这个方法，既：动画播放完成后调用这个方法，销毁这个对象

    {
        Destroy(this.gameObject);
    }
    #endregion

     void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere( transform.position+(Vector3)conterOffset,0.2f);
    }

}
