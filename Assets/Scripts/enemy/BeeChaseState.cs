using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeChaseState : BaseState
{
    private Attack attack;
    private Vector3 targer;
    private Vector3 MoveDir;
    private bool isAttack;
    private float AttackRateCounter = 0;
    public override void OnEnter(enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed;
        attack = enemy.GetComponent<Attack>();
        currentEnemy.LostTimeCounter = currentEnemy.LostTime;
        currentEnemy.anim.SetBool("Chase", true);
    }
    public override void LogicUpdate()
    {
        if (currentEnemy.LostTimeCounter <= 0)
        {
            currentEnemy.SwitchState(NPCState.Patrol);
        }
        targer = new Vector3(currentEnemy.Attacker.position.x, currentEnemy.Attacker.position.y + 1.5f, 0);
        if (Mathf.Abs(targer.x - currentEnemy.transform.position.x)<=attack.attackRange&& Mathf.Abs(targer.y - currentEnemy.transform.position.y) <= attack.attackRange)
        {
            isAttack = true;
            //Í£Ö¹ÒÆ¶¯²¢¹¥»÷
            if (!currentEnemy.isHurt)
            {

            currentEnemy.rb.velocity = Vector2.zero;
            }
            
            AttackRateCounter -= Time.deltaTime;
            if (AttackRateCounter <= 0)
            {
                AttackRateCounter = attack.attackRate;
                currentEnemy.anim.SetTrigger("Attack");
            }
        }
        else
        {
            isAttack = false;
        }

        MoveDir = (targer - currentEnemy.transform.position).normalized;
        if (MoveDir.x > 0)
        {
            currentEnemy.transform.localScale = new Vector3(-1, 1, 1);

        }
        else if (MoveDir.x < 0)
        {
            currentEnemy.transform.localScale = new Vector3(1, 1, 1);
        }
    }


    public override void PhysicsUpdate()
    {
        if (!currentEnemy.isDeath && !currentEnemy.isHurt&&!isAttack)
        {
            currentEnemy.rb.velocity = MoveDir * currentEnemy.currentSpeed * Time.deltaTime;
        }
      
    }

    public override void OnExit()
    {
        currentEnemy.anim.SetBool("Chase", false);
    }



}
