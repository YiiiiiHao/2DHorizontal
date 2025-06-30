using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Cinemachine;
using Unity.VisualScripting;

public class PlayerController : MonoBehaviour
{ 
 [Header("监听事件")]
 public SceneLoadEventSO sceneLoadEvent;
 public VoidEventSO afterSceneLoadEvent;
 public VoidEventSO loadDataEvent;
 public VoidEventSO backToMenuEvent;
  public PlayerInputControl inputControl;//调用PlayerInputControl脚本，是个操作系统脚本。
  public Vector2 InputDirection ;//创建这个变量来存储玩家的输入方向，UNity中会有输入框
  private Rigidbody2D rb;//创建这个变量来存储玩家的刚体组件
  public PhysicsCheck physicsCheck;//创建这个变量来存储玩家的物理检测组件
  private PlayerAnimation playerAnimation;//创建这个变量来存储玩家的动画组件
//   private CapsuleCollider2D coll;//创建这个变量来存储玩家的碰撞体组件
 
  private CapsuleCollider2D coll;
  [Header("基本参数")]
  public float Speed ;//创建这个变量来存储玩家的速度，可以根据需要调整
  private float runSpeed ;
  private float walkSpeed => Speed/2.5f;
  public float JumpForce ;//创建这个变量来存储玩家的跳跃力量，可以根据需要调整
  public float hurtForce;
  public bool isCrouch ;
  public float sprintSpeed;
    [Header("物理材质参数")]
  public PhysicsMaterial2D normal;
  public PhysicsMaterial2D wall;
  public Vector2 originaSize;
  public Vector2 originaoFfset;
  public float TempSpeed => Speed;//记录原始速度

//  private bool Jumping = true;//创建这个变量来存储玩家是否在跳跃
   [Header("状态参数")]
 public bool isHurt = false;
 public bool isDead = false;
 public bool isAttack ;//创建这个变量来存储玩家是否在攻击
 public SpriteRenderer spriteRenderer;

    private void Awake()
    {
        physicsCheck = GetComponent<PhysicsCheck>();//获取玩家的物理检测组件
        inputControl = new PlayerInputControl();//create a new instance of PlayerInputControl class
        rb = GetComponent<Rigidbody2D>();//get the Rigidbody2D component of the player
        spriteRenderer = GetComponent<SpriteRenderer>();//获取玩家的SpriteRenderer组件
        coll = GetComponent<CapsuleCollider2D>();//获取玩家的CupsuleCollider2D组件,为了蹲下时能改变碰撞体尺寸
        originaSize = coll.size;//记录玩家碰撞体原始大小
        originaoFfset = coll.offset;//记录玩家碰撞体原始位置
               
        playerAnimation = GetComponent<PlayerAnimation>();//获取玩家的动画组件
 

        //攻击
        inputControl.GamePlay.Attack.started += PlayeAttack;

        #region 强制走路
        //走路
        runSpeed = Speed;
        inputControl.GamePlay.Walk.performed+=ctx =>//performed是按下按键时触发
        {
            if(physicsCheck.isGround)//保证在地面上
                  Speed = walkSpeed;
 
        };
        inputControl.GamePlay.Walk.canceled += ctx =>//canceled是抬起按键
        {
             if(physicsCheck.isGround)//保证在地面上
                 Speed = runSpeed;
        };
        #endregion
        inputControl.Enable();

    }


    //imputContril to Enable and Disable
    private void OnEnable()
    {
        sceneLoadEvent.LoadRequesetEvent+=OnLoadEvent;
        afterSceneLoadEvent.OnEventRaised += OnAfterSceneLoadEvent;
        loadDataEvent.OnEventRaised += OnloadDataEvent; 
        backToMenuEvent.OnEventRaised += OnloadDataEvent;
    }

    private void OnAfterSceneLoadEvent()
    {
         inputControl.GamePlay.Enable();
    }

    private void OnLoadEvent(GameSceneSO arg0, Vector3 arg1, bool arg2)
    {
        inputControl.GamePlay.Disable(); 
    }

    private void OnDisable()
    {
        inputControl.Disable();
        sceneLoadEvent.LoadRequesetEvent-=OnLoadEvent;
        afterSceneLoadEvent.OnEventRaised -= OnAfterSceneLoadEvent;
        loadDataEvent.OnEventRaised -= OnloadDataEvent; 
        backToMenuEvent.OnEventRaised -= OnloadDataEvent;

    }
    private void OnloadDataEvent() 
    {
      isDead = false;
    }



    private void Update()
    {
         InputDirection = inputControl.GamePlay.Move.ReadValue<Vector2>();//获取玩家的输入方向
 
            //+=Jump：注册函数，当玩家按下跳跃键时，调用Jump方法            
         inputControl.GamePlay.Jump.started +=Jump;//当玩家按下跳跃键时，调用Jump方法
        
        CheckState();


    }


    private void FixedUpdate()//每帧调用一次
    {
        if(!isHurt&&!isAttack)//如果玩家没有受伤，则可以移动
        {
        Move();
        }
    }
    
    // private void OnTriggerStay2D(Collider2D other)
    // {

    // }
    #region 场景加载
    //场景切换时，禁用玩家的输入
    private void OnSceneLoad(GameSceneSO arg0, Vector3 arg1, bool arg2)
    {
        inputControl.GamePlay.Disable();
    }
    //场景切换后，启用玩家的输入
     private void OnSceneLoad()
    {
        inputControl.GamePlay.Enable();
    }
    #endregion
    public void Move()
    {
        
        if(!isCrouch)//如果玩家没有蹲下，则可以移动
        {
        rb.velocity = new Vector2(InputDirection.x *Speed * Time.deltaTime,rb.velocity.y );//根据玩家的输入方向和速度移动玩家的刚体组件
        }

        // transform.Translate(InputDirection * Speed * Time.deltaTime);//根据玩家的输入方向和速度移动玩家

        //人物翻转
         int faceDir = (int)transform.localScale.x;
 
        //transform.localScale = new Vector3();    
        //根据玩家的输入方向来设置玩家的SpriteRenderer组件的Flip 的 X 属性，控制玩家朝向。
            if(InputDirection.x>0)
           {
             faceDir=1;
            // spriteRenderer.flipX = true;
     
           } if(InputDirection.x<0)

           {
             faceDir=-1;
            // spriteRenderer.flipX = false;   

           }
           
           transform.localScale = new Vector3(faceDir,1,1);

           //下蹲   下蹲的本质是获取输入按键 和 播放动画，然后修改碰撞体的高度和位置，让玩家蹲下。
            isCrouch = InputDirection.y< 0 && physicsCheck.isGround;
            if(isCrouch)
            {
                //修改碰撞体高度和位置，碰撞体高度只有原来的三分之一，底部位置不变。
                coll.size = new Vector2(coll.size.x,1.75f) ;
                coll.offset = new Vector2(originaoFfset.x,0.75f);
                 
            }
            else
            {
                //还愿碰撞体高度和位置
                coll.size = originaSize;
                coll.offset = originaoFfset;
  
            }
           

    }

    
    private void Jump(InputAction.CallbackContext context)
    {
        // Debug.Log("Jump");
        if(physicsCheck.isGround)
        {
            rb.AddForce(transform.up*JumpForce,ForceMode2D.Impulse);//添加一个向上方向的力量，模拟跳跃效果
            GetComponent<AudioDefination>().PlayerAudioClip();//播放音效
        }
    }
    private void Sprint()
    {
        //添加一个向前冲刺的力量，模拟冲刺效果
         if(InputDirection.x>0)
            {
                      rb.velocity = new Vector2(transform.right.x *sprintSpeed,0);


            }
            if(InputDirection.x<0)
            {
                        rb.velocity = new Vector2(-transform.right.x *sprintSpeed,0);

            }
         //冲刺
            // if(InputDirection.x>0)
            // {
            //   rb.AddForce(transform.right*sprintSpeed*Time.deltaTime,ForceMode2D.Impulse);

            // }
            // if(InputDirection.x<0)
            // {
            //     rb.AddForce(-transform.right*sprintSpeed*Time.deltaTime,ForceMode2D.Impulse);
            // }
    }

    
    private void PlayeAttack(InputAction.CallbackContext context)
    {
        playerAnimation.PlayAttack();
        isAttack = true;
     
 
    }

    public void GetHurd(Transform attacker)
    {
        isHurt = true;
        rb.velocity = Vector2.zero;
        Vector2 dir = new Vector2((transform.position.x - attacker.position.x),0).normalized;//nomalized是返回一个大小为1的向量
        rb.AddForce(dir * hurtForce,ForceMode2D.Impulse);
    }

    public void PlayerDead()
    {
        isDead = true;
        inputControl.GamePlay.Disable();//禁用玩家的输入
    }

    private void CheckState()
    {   //根据玩家的物理检测组件的isGround属性来设置玩家的碰撞体的物理材质，来改变玩家的角色的行为。
        //当玩家在地面上时，碰撞体的物理材质为normal，当玩家在墙壁上时，碰撞体的物理材质为wall。
        coll.sharedMaterial = physicsCheck.isGround ?normal : wall;
    }
    
}
