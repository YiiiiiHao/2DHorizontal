using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarPatrolState : BaseState
{

    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        // throw new System.NotImplementedException();
    }
    public override void LogicUpdate()
    {
        if(currentEnemy.foundPlayer())
        {
            currentEnemy.SwitchState(NPCState.Chase);
        }
       if(!currentEnemy.physicsCheck.isGround || (currentEnemy.physicsCheck.touchLeftWall&&currentEnemy.faceDri .x <0)|| (currentEnemy.physicsCheck.touchRightWall&& currentEnemy.faceDri .x >0))
        {
            if(!currentEnemy.physicsCheck.isGround)
            // {Debug.Log("is not ground");}
            currentEnemy.wait = true;
            currentEnemy.anim.SetBool("Walk", false);
        }
        else
        {
            currentEnemy.anim.SetBool("Walk", true);
        }
    }
    

    public override void physicsUpdate()
    {
         
    }

    public override void OnExit()
    {
        currentEnemy.anim.SetBool("Walk", false);
        // Debug.Log("Exit");

     }
    
 
}
