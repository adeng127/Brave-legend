using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector]public Animator anim;
   public PhysicsCheck physicsCheck;
    [Header("基本参数")]

    public float NowBootomOffetsX;
    public float normalSpeed;
    public float chaseSpeed;
    public float currentSpeed;
    public float hurtForce;
    public Vector3 faceDir;

    [HideInInspector] public Transform Attacker;
    public Vector3 spwanPoint;
    [Header("计时器")]
    public float WaitTime;
    public float WaitTimeCounter;

    public float LostTime;
    public float LostTimeCounter;

    [Header("状态判断")]
    public bool Wait;
    public bool isWait;
    public bool isHurt;
    public bool isDeath;

    public BaseState prtalState;
    public BaseState currentState;
    public BaseState chaseState;

    [Header("检测")]
    
    public Vector2 centerOffset;
    public Vector2 checkSize;
    public float checkDistance;
    public LayerMask attackLayer;


    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        physicsCheck = GetComponent<PhysicsCheck>();
        currentSpeed = normalSpeed;
       
        spwanPoint = transform.position;
    }
    private void OnEnable()
    {
        NowBootomOffetsX = physicsCheck.bottomOffset.x;
        currentState = prtalState;
        currentState.OnEnter(this);
    }
    private void Update()
    {
       if (transform.localScale.x > 0)
        {
            physicsCheck.bottomOffset.x = NowBootomOffetsX;
        }
        else
        {

            physicsCheck.bottomOffset.x = -NowBootomOffetsX;
        }

        faceDir = new Vector3(-transform.localScale.x, 0, 0);
        currentState.LogicUpdate();
        TimeCounter();
    }

    private void FixedUpdate()
    {
        
        if (!isHurt && !isDeath && !Wait)
        {
            Move();
        }


        currentState.PhysicsUpdate();
    }

    private void OnDisable()
    {
        currentState.OnExit();
    }
    public virtual void Move()
    {
        rb.velocity = new Vector2(faceDir.x * currentSpeed * Time.deltaTime, rb.velocity.y);

    }

    public virtual bool FoundPlayer()
    {
        return Physics2D.BoxCast(transform.position + (Vector3)centerOffset, checkSize, 0, faceDir, checkDistance, attackLayer);
    }

    public void SwitchState(NPCState state)
    {
        var newstate = state switch
        {
            NPCState.Patrol => prtalState,
            NPCState.Chase => chaseState,
            _ => null

        };
        currentState.OnExit();
        currentState = newstate;
        currentState.OnEnter(this);
    }


    public virtual Vector3 GetNewPoint()
    {
        return transform.position;
    }
    #region 事件执行方法
    /// <summary>
    /// 计时器
    /// </summary>
    public void TimeCounter()
    {
        if (Wait)
        {
            if (!isWait)
            {
                rb.velocity = Vector2.zero;
                isWait = true;

            }
            
            WaitTimeCounter -= Time.deltaTime;
            if (WaitTimeCounter <= 0)
            {
                WaitTimeCounter = WaitTime;
                transform.localScale = new Vector3(faceDir.x, 1, 1);
                Wait = false;
                isWait = false ;
            }
        }
        

        if (!FoundPlayer()&&LostTimeCounter>0)
        {
            LostTimeCounter -= Time.deltaTime;
        }
       

    }


    public void OnTakeDamage(Transform attackTrans)
    {
        Attacker = attackTrans;
        //转身
        if (Attacker.position.x - transform.position.x>0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        if (Attacker.position.x - transform.position.x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        isHurt = true;
        anim.SetTrigger("Hurt");
        Vector2 dir = new Vector2(transform.position.x - attackTrans.position.x, 0).normalized;
        rb.velocity = Vector2.zero; // 将速度设置为零
        StartCoroutine(OnHurt(dir));
    }


    private IEnumerator OnHurt(Vector2 dir)
    {
        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.5f);
        isHurt = false;

    }

    public void OnDie()
    {
        gameObject.layer = 2;
        anim.SetBool("Death", true);
        isDeath = true;
    }
    public void DestroyAfterAnimation()
    {
        Destroy(this.gameObject);
    }


    #endregion


    public virtual void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + (Vector3)centerOffset+new Vector3(checkDistance*-transform.localScale.x,0), 0.2f);
    }
}
