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
    [Header("��������")]
    public float speed;
    public float jumpForce;
    public int jumpCount = 0;  // ������Ծ����

    public float hurtForce;  //���˷�������
    public bool isHurt;
    public bool isDead;
    
    private void Awake()   //start֮ǰ����
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
    private void FixedUpdate() //�̶����£��̶�ʱ��Ƶ��ִ�У������йػ��������
    {
        if(!isHurt)
        Move();
    }

    //����
    /*private void OnTriggerStay2D(Collider2D collider)
    {
        Debug.Log(collider.name);
    }*/

    public void Move()
    {
        rb.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime,rb.velocity.y);

        //�ֱ���ת����
        int faceDir = (int)transform.localScale.x;
        if (inputDirection.x > 0)
        {
            faceDir = 1;
        }
        else if(inputDirection.x<0)
        {
            faceDir = -1;
        }
        //���﷭ת
        transform.localScale = new Vector3(faceDir,1,1);
    }
    //��Ծ
    private void Jump(InputAction.CallbackContext obj)
    {
        /* Debug.Log("��Ծ");*/
        if (physicsCheck.isGround)
        {
            jumpCount = 0;
            rb.AddForce(transform.up*jumpForce,ForceMode2D.Impulse);
        }
        else
        {
            if (jumpCount < 1)  
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);  // ����ɫ�Ĵ�ֱ�ٶ�����Ϊ0
                rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
                jumpCount++;  // ÿ����Ծʱ��������Ծ����
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
        inputControl.GamePlayer.Disable(); //��ֹ��������
    }
}

