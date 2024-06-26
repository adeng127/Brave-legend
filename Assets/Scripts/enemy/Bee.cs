using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee : enemy
{
    [Header("ÒÆ¶¯·¶Î§")]
    public float protalRadius;

    protected override void Awake()
    {
        base.Awake();
        prtalState = new BeeprtalState();
        chaseState = new BeeChaseState();
    }
    public override bool FoundPlayer()
    {
       var obj= Physics2D.OverlapCircle(transform.position, checkDistance, attackLayer);
        if (obj)
        {
            Attacker = obj.transform;
        }
        return obj;
    }

    public override void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, checkDistance);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, protalRadius);
    }

    public override Vector3 GetNewPoint()
    {
        var targetX = Random.Range(-protalRadius, protalRadius);
        var targetY = Random.Range(-protalRadius, protalRadius);
        return spwanPoint + new Vector3(targetX, targetY);
    }
    public override void Move()
    {
        
    }
}
