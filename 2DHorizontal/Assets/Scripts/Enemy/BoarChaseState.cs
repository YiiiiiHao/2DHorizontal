using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarChaseState :BaseState
{ 
    public override void OnEnter(Enemy enemy)
    {
         currentEnemy = enemy;
         Debug.Log( "Chase State" );
    }

    public override void LogicUpdate()
    {
         
    }


    public override void physicsUpdate()
    {
       
    }

    public override void OnExit()
    {
      
    }
}
