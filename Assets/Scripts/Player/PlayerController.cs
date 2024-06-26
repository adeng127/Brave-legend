using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    [Header("事件监听")]
    public SceneLoadEventSO SceneLoadEvent;
    public VoidEventSO afterSceneLoadEvent;
    public VoidEventSO LoadDataEvent;
    public VoidEventSO backToMenuEvent;
    [Header("组件")]
    public PlayinputControl inputControl;
    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;
    public PlayAnimation playAnimation;
    public CapsuleCollider2D PlayerColl;
    public AudioDefination PlayAudioClip;
    public Vector2 inputDirection;//移动用坐标值
    [Header("基本参数")]
    public float speed;
    public float walljumpForce;
    public float jumpForce;
    public int jumpCount = 0;  // 跟踪跳跃次数
    public float hurtForce;  //受伤反弹的力
    public float SlideDistance;
    public float SlideSpeed;

    public int combo;

    public bool isHurt;
    public bool isDead;
    public bool isAttack;
    public bool wallJump;
    public bool isSlide;
    [Header("物理材质")]
    public PhysicsMaterial2D Wall;
    public PhysicsMaterial2D Normal;
    [Header("音效")]
    public AudioClip jumpAudio;
    public AudioClip slideAudio;


    private void Awake()   //start之前运行
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
    private void FixedUpdate() //固定更新，固定时钟频率执行，物理有关基本是这个
    {
        if(!isHurt&&!isAttack && !isSlide)
        Move();
    }

    //测试
    /*private void OnTriggerStay2D(Collider2D collider)
    {
        Debug.Log(collider.name);
    }*/
    //场景加载前禁用控制
    private void OnLoadEvent(GameSceneSO arg0, Vector3 arg1, bool arg2)
    {
        inputControl.GamePlayer.Disable();
    }

    private void OnLoadDataEvent()
    {
        isDead = false;
    }

    //场景加载后启用控制
    private void OnafterSceneLoadEvent()
    {
        inputControl.GamePlayer.Enable();
    }
    public void Move()
    {
        if(!wallJump)
        rb.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime,rb.velocity.y);

        //localscale翻转
        int faceDir = (int)transform.localScale.x;
        if (inputDirection.x > 0)
        {
            faceDir = 1;
        }
        else if (inputDirection.x < 0)
        {
            faceDir = -1;
        }
        //人物翻转
        transform.localScale = new Vector3(faceDir,1,1);





    }
    //跳跃
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
                rb.velocity = new Vector2(rb.velocity.x, 0);  // 将角色的垂直速度设置为0
                rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
                PlayAudioClip.PlayAudioClip();
                jumpCount++;  // 每次跳跃时，增加跳跃次数
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

    //攻击
    private void PlayAttack(InputAction.CallbackContext obj)
    {
        playAnimation.PlayAttack();
        isAttack = true;
        
    }

    //滑铲
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
    #region 受伤与死亡
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
    #endregion
}

