using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarPrtalState : BaseState
{
 public override void OnEnter(enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.normalSpeed;
    }
    public override void LogicUpdate()
    {
        if (currentEnemy.FoundPlayer())
        {
            currentEnemy.SwitchState(NPCState.Chase);
        }

        if (currentEnemy.Wait=false||!currentEnemy.physicsCheck.isGround||(currentEnemy.physicsCheck.TouchLeftWall && currentEnemy.faceDir.x < 0 || currentEnemy.physicsCheck.TouchRightWall && currentEnemy.faceDir.x > 0))
        {
            currentEnemy.Wait = true;
            
            currentEnemy.anim.SetBool("Walk", false);
        }
        else
        {
            currentEnemy.anim.SetBool("Walk", true);
        }
    }

   
    public override void PhysicsUpdate()
    {

    }
    public override void OnExit()
    {
        currentEnemy.anim.SetBool("Walk", false);
        Debug.Log("exit");
    }

}
