using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boar : Enemy
{ 
      protected override void Awake()
      {
        base.Awake();//执行父类的方法
 
        patrolState = new BoarPatrolState();
        chaseState = new BoarChaseState();
      }

}
