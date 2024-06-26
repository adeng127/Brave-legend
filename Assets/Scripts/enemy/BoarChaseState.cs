using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarChaseState : BaseState
{
    

    public override void OnEnter(enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed;
        currentEnemy.anim.SetBool("Run", true) ;
    }

   public override void LogicUpdate()
    {
        if (currentEnemy.LostTimeCounter <= 0)
        {
            currentEnemy.SwitchState(NPCState.Patrol);
        }

        if (currentEnemy.Wait = false || !currentEnemy.physicsCheck.isGround || (currentEnemy.physicsCheck.TouchLeftWall && currentEnemy.faceDir.x < 0 || currentEnemy.physicsCheck.TouchRightWall && currentEnemy.faceDir.x > 0))
        {
            currentEnemy.transform.localScale=new Vector3(currentEnemy.faceDir.x,1,1);
        }


    }

    public override void PhysicsUpdate()
    {
    }
    public override void OnExit()
    {
        currentEnemy.anim.SetBool("Run", false);

        currentEnemy.LostTimeCounter = currentEnemy.LostTime;
    }
}
