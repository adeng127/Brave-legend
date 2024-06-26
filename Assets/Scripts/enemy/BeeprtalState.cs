using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeprtalState : BaseState
{
    public Vector3 targer;
    public Vector3 MoveDir;

    
    public override void OnEnter(enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.normalSpeed;
        targer = enemy.GetNewPoint();
    }

    public override void LogicUpdate()
    {
        if (currentEnemy.FoundPlayer())
        {
            currentEnemy.SwitchState(NPCState.Chase);
        }
        if (Mathf.Abs(targer.x - currentEnemy.transform.position.x) < 0.1f &&Mathf.Abs( targer.y - currentEnemy.transform.position.y )< 0.1f)
        {
            currentEnemy.Wait = true;
            targer = currentEnemy.GetNewPoint();
        }

        MoveDir = (targer - currentEnemy.transform.position).normalized;
        if (MoveDir.x > 0)
        {
            currentEnemy.transform.localScale = new Vector3(-1, 1, 1);

        }else if (MoveDir.x < 0)
        {
            currentEnemy.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    
 

    public override void PhysicsUpdate()
    {
        if (!currentEnemy.isDeath && !currentEnemy.isHurt && !currentEnemy.Wait)
        {
            currentEnemy.rb.velocity = MoveDir * currentEnemy.currentSpeed * Time.deltaTime;
        }
        else
        {
            currentEnemy.rb.velocity = Vector2.zero;
        }
    }
    public override void OnExit()
    {
        
    }
}
