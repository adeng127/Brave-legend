using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    [Header("�¼�����")]
    public SceneLoadEventSO SceneLoadEvent;
    public VoidEventSO afterSceneLoadEvent;
    public VoidEventSO LoadDataEvent;
    public VoidEventSO backToMenuEvent;
    [Header("���")]
    public PlayinputControl inputControl;
    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;
    public PlayAnimation playAnimation;
    public CapsuleCollider2D PlayerColl;
    public AudioDefination PlayAudioClip;
    public Vector2 inputDirection;//�ƶ�������ֵ
    [Header("��������")]
    public float speed;
    public float walljumpForce;
    public float jumpForce;
    public int jumpCount = 0;  // ������Ծ����
    public float hurtForce;  //���˷�������
    public float SlideDistance;
    public float SlideSpeed;

    public int combo;

    public bool isHurt;
    public bool isDead;
    public bool isAttack;
    public bool wallJump;
    public bool isSlide;
    [Header("�������")]
    public PhysicsMaterial2D Wall;
    public PhysicsMaterial2D Normal;
    [Header("��Ч")]
    public AudioClip jumpAudio;
    public AudioClip slideAudio;


    private void Awake()   //start֮ǰ����
    {
        inputControl = new PlayinputControl();
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        playAnimation = GetComponent<PlayAnimation>();
        PlayerColl = GetComponent<CapsuleCollider2D>();
        PlayAudioClip = GetComponent<AudioDefination>();
        inputControl.GamePlayer.Jump.started += Jump;
        inputControl.GamePlayer.Attack.started += PlayAttack;
        inputControl.GamePlayer.Slide.started += Slide;

        inputControl.Enable();
    }

   

    private void OnEnable()
    {
        SceneLoadEvent.loadRequestEvent +=OnLoadEvent;
        afterSceneLoadEvent.OnEventRaise += OnafterSceneLoadEvent;
        LoadDataEvent.OnEventRaise += OnLoadDataEvent;
        backToMenuEvent.OnEventRaise += OnLoadDataEvent;
    }

    

    private void OnDisable()
    {
        inputControl.Disable();
        SceneLoadEvent.loadRequestEvent -= OnLoadEvent;
        afterSceneLoadEvent.OnEventRaise -= OnafterSceneLoadEvent;
        LoadDataEvent.OnEventRaise -= OnLoadDataEvent;
        backToMenuEvent.OnEventRaise -= OnLoadDataEvent;
    }

   
    private void Update()
    {
        inputDirection = inputControl.GamePlayer.Move.ReadValue<Vector2>();
        CheckState();
        if (physicsCheck.isGround)
        {
            jumpCount = 0;
        }
    }
    private void FixedUpdate() //�̶����£��̶�ʱ��Ƶ��ִ�У������йػ��������
    {
        if(!isHurt&&!isAttack && !isSlide)
        Move();
    }

    //����
    /*private void OnTriggerStay2D(Collider2D collider)
    {
        Debug.Log(collider.name);
    }*/
    //��������ǰ���ÿ���
    private void OnLoadEvent(GameSceneSO arg0, Vector3 arg1, bool arg2)
    {
        inputControl.GamePlayer.Disable();
    }

    private void OnLoadDataEvent()
    {
        isDead = false;
    }

    //�������غ����ÿ���
    private void OnafterSceneLoadEvent()
    {
        inputControl.GamePlayer.Enable();
    }
    public void Move()
    {
        if(!wallJump)
        rb.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime,rb.velocity.y);

        //localscale��ת
        int faceDir = (int)transform.localScale.x;
        if (inputDirection.x > 0)
        {
            faceDir = 1;
        }
        else if (inputDirection.x < 0)
        {
            faceDir = -1;
        }
        //���﷭ת
        transform.localScale = new Vector3(faceDir,1,1);





    }
    //��Ծ
    private void Jump(InputAction.CallbackContext obj)
    {

        PlayAudioClip.audioClip = jumpAudio;
        if (physicsCheck.isGround)
        {
            rb.AddForce(transform.up*jumpForce,ForceMode2D.Impulse);
            isSlide = false;
            StopAllCoroutines();
            PlayAudioClip.PlayAudioClip();
        }
        else
        {
            if (wallJump)
            {
                rb.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime, rb.velocity.y);
                
            }
            if (jumpCount < 1&&!physicsCheck.isWall)  
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);  // ����ɫ�Ĵ�ֱ�ٶ�����Ϊ0
                rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
                PlayAudioClip.PlayAudioClip();
                jumpCount++;  // ÿ����Ծʱ��������Ծ����
            }
            
        }

        if (physicsCheck.isWall)
        {
            rb.AddForce(new Vector2(-inputDirection.x, 2f) * walljumpForce, ForceMode2D.Impulse);
            Debug.Log(new Vector2(-inputDirection.x, 2f) * walljumpForce);
            wallJump = true;
            jumpCount -= 1;
            PlayAudioClip.PlayAudioClip();
        }
        
    }

    //����
    private void PlayAttack(InputAction.CallbackContext obj)
    {
        playAnimation.PlayAttack();
        isAttack = true;
        
    }

    //����
    private void Slide(InputAction.CallbackContext obj)
    {

        PlayAudioClip.audioClip = slideAudio;
        if (!isSlide)
        {
            PlayAudioClip.PlayAudioClip();
            isSlide = true;
            gameObject.layer = LayerMask.NameToLayer("Enemy");
            var tragerPos = new Vector3(transform.position.x + SlideDistance * transform.localScale.x, transform.position.y);

            StartCoroutine(TriggerSlide(tragerPos));
    }
    }
    private IEnumerator TriggerSlide(Vector3 trage)
    {
        do
        {
            yield return null;
            if (!physicsCheck.isGround)
            {
                break;
            }
            if (physicsCheck.TouchLeftWall&&transform.localScale.x<0f||physicsCheck.TouchRightWall && transform.localScale.x > 0f)
            {
                isSlide = false;
                break;
            }
            rb.MovePosition(new Vector2( transform.position.x + transform.localScale.x * SlideSpeed, transform.position.y));

        } while (MathF.Abs(trage.x - transform.position.x) > 0.2f);
        isSlide = false;
        gameObject.layer = LayerMask.NameToLayer("Player");
    }

    public void CheckState()
    {
        PlayerColl.sharedMaterial = physicsCheck.isGround ? Normal : Wall;

        if (physicsCheck.isWall)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 2f);
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y );
        }
        if (wallJump && rb.velocity.y < 0)
        {
            wallJump = false;
        }
    }
    #region ����������
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
    #endregion
}

