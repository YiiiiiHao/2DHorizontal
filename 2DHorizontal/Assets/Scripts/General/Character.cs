using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Events;//事件系统

public class Character : MonoBehaviour,ISaveable
{
    [Header("监听事件")]
    public VoidEventSO newGameEvent;
    public float maxHealth;//最大生命值
    public float currentHealth;//当前生命值
    public float currentpower;//当前技能值
   [Header("受伤无敌状态")]
   public float invulnerableTime;//受伤无敌状态持续时间
   private float invulnerableCount;//受伤无敌状态持续时间计时

   public bool invulnerable; //是否处于受伤无敌状态

   public UnityEvent<Character> OnHealthChange; //生命值改变事件
   

   public UnityEvent <Transform> OnTakeDamage; //受到攻击事件
   public UnityEvent OnDie; //死亡事件
    public void NewGame()
    {
        currentHealth = maxHealth;//游戏开始时，初始化生命值为最大生命值
       
        OnHealthChange?.Invoke(this);//触发生命值改变事件
    
    }
    private void OnEnable()
    {
        newGameEvent.OnEventRaised += NewGame;
        ISaveable saveable = this;  
        saveable.RegisterSaveaDate();
    }
    private void OnDisable()
    {
        newGameEvent.OnEventRaised -= NewGame;
        ISaveable saveable = this;
        saveable.UnRegisterSaveDate();
    }
     public void TakeDamage(attack attacker)    
     {

        if(invulnerable)//受伤无敌状态
        {
            return;//如果处于受伤无敌状态，则不受到攻击
        }
        // Debug.Log(attacker.damage);
        if( currentHealth - attacker.damage > 0 )//如果生命值为0，则死亡
        {
        currentHealth -= attacker.damage;//受到攻击，扣除生命值

        TriggerInvulnerable();
        OnTakeDamage?.Invoke(attacker.transform);//触发受到攻击事件
        }
        else
        {
            currentHealth = 0;
            OnDie?.Invoke();//触发死亡事件
        }

        OnHealthChange?.Invoke(this);//触发生命值改变事件
            
     }
     public void Update()
     {
        if(invulnerable)//受伤无敌状态
        {
            invulnerableCount -= Time.deltaTime;//受伤无敌状态持续时间计时
            if(invulnerableCount<=0)//受伤无敌状态持续时间结束
            {
                invulnerable = false;
            }
        }
        
     }

    void OnTriggerStay2D(Collider2D other)
    {
         if(other.CompareTag("Water"))//如果碰到水面，则触发死亡和血量归零
         {
            if(currentHealth>0)//如果生命值大于0
            {
            currentHealth = 0;
            OnHealthChange?.Invoke(this);//触发生命值改变事件
            OnDie?.Invoke();
            }
         }
    }

    private void TriggerInvulnerable()
    {
        if(!invulnerable)
        {
        invulnerable =true;
        invulnerableCount = invulnerableTime;//受伤无敌状态持续时间计时
        }

    }

    public DataDefinition GetDataID()
    {
        return GetComponent<DataDefinition>();//获取数据ID
    }

    public void GetSaveDate(Data data)
    {
        if(data.characterPosDrict.ContainsKey(GetDataID().ID))//如果保存数据中包含该角色的位置信息
        {
            data.characterPosDrict[GetDataID().ID] = transform.position;//保存角色位置信息
            data.floatSavedData[GetDataID().ID + "health"] =this.currentHealth;//保存角色生命值信息
            data.floatSavedData[GetDataID().ID + "power"] =this.currentpower;
        } 
        else
        {
            data.characterPosDrict.Add(GetDataID().ID,transform.position);//如果保存数据中不包含该角色的位置信息，则添加角色位置信息
            data.floatSavedData.Add(GetDataID().ID + "health",this.currentHealth);////如果保存数据中不包含该角色的生命值信息，则添加角色生命值信息
            data.floatSavedData.Add(GetDataID().ID + "power",this.currentpower);
        }
    }

    public void loadData(Data data)
    {
       if(data.characterPosDrict.ContainsKey(GetDataID().ID))
       {
        transform.position = data.characterPosDrict[GetDataID().ID];//加载角色位置信息
        this.currentHealth = data.floatSavedData[GetDataID().ID + "health"];//加载角色生命值信息
        this.currentpower = data.floatSavedData[GetDataID().ID + "power"];//加载角色技能值信息

        //通知更新
        OnHealthChange?.Invoke(this);
       }
    }
}
