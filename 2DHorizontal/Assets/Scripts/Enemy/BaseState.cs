// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

public abstract class BaseState  //abstract是抽象类，也是关键字
{ 
 
   
    protected Enemy currentEnemy;
    public abstract void OnEnter(Enemy enemy);//进入状态

    public abstract void LogicUpdate();//状态逻辑更新

    public abstract void physicsUpdate();//状态物理更新

    public abstract void OnExit();////退出状态
}
