using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    public PlayinputControl inputControl;
    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;
    public Vector2 inputDirection;
    [Header("基本参数")]
    public float speed;
    public float jumpForce;
    public int jumpCount = 0;  // 跟踪跳跃次数

    public float hurtForce;  //受伤反弹的力
    public bool isHurt;
    public bool isDead;
    
    private void Awake()   //start之前运行
    {
        inputControl = new PlayinputControl();
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        inputControl.GamePlayer.Jump.started += Jump;
    }

    
    private void OnEnable()
    {
        inputControl.Enable();

    }
    private void OnDisable()
    {
        inputControl.Disable();
    }
    private void Update()
    {
        inputDirection = inputControl.GamePlayer.Move.ReadValue<Vector2>();

    }
    private void FixedUpdate() //固定更新，固定时钟频率执行，物理有关基本是这个
    {
        if(!isHurt)
        Move();
    }

    //测试
    /*private void OnTriggerStay2D(Collider2D collider)
    {
        Debug.Log(collider.name);
    }*/

    public void Move()
    {
        rb.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime,rb.velocity.y);

        //手柄翻转定制
        int faceDir = (int)transform.localScale.x;
        if (inputDirection.x > 0)
        {
            faceDir = 1;
        }
        else if(inputDirection.x<0)
        {
            faceDir = -1;
        }
        //人物翻转
        transform.localScale = new Vector3(faceDir,1,1);
    }
    //跳跃
    private void Jump(InputAction.CallbackContext obj)
    {
        /* Debug.Log("跳跃");*/
        if (physicsCheck.isGround)
        {
            jumpCount = 0;
            rb.AddForce(transform.up*jumpForce,ForceMode2D.Impulse);
        }
        else
        {
            if (jumpCount < 1)  
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);  // 将角色的垂直速度设置为0
                rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
                jumpCount++;  // 每次跳跃时，增加跳跃次数
            }
            
        }
        
    }
    public void GetHurt(Transform attacker)
    {
        isHurt = true;
        rb.velocity = Vector2.zero;
        Vector2 dir = new Vector2((transform.position.x - attacker.position.x), 0).normalized;
        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
    }

    public void PlayerDead()
    {
        isDead = true;
        inputControl.GamePlayer.Disable(); //禁止继续控制
    }
}

