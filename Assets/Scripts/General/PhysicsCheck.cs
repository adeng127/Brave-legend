using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    private CapsuleCollider2D Coll;
    private PlayerController playerController;
    private Rigidbody2D rb;
    [Header("¼ì²â²ÎÊý")]
    public Vector2 bottomOffset;
    public Vector2 leftOffset;
    public Vector2 rightOffset;

    public float chaeckRaduis;
    public LayerMask groudLayer;
    [Header("×´Ì¬")]
    public bool manual;
    public bool isGround;
    public bool TouchLeftWall;
    public bool TouchRightWall;
    public bool isWall;
    public bool isPlayer;
    private void Awake()
    {
        Coll = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        if (!manual)
        {
            leftOffset = new Vector2(Coll.offset.x-Coll.bounds.size.x/2-0.1f,Coll.bounds.size.y/2);

            rightOffset = new Vector2(-leftOffset.x-0.1f, leftOffset.y);
        }
        if (isPlayer)
            playerController = GetComponent<PlayerController>();


    }
    private void Update()
    {
        Check();    
    }
    public void Check()
    {

        isGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, chaeckRaduis, groudLayer);


        TouchLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, chaeckRaduis, groudLayer);
        TouchRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, chaeckRaduis, groudLayer);

        if (isPlayer)
        {
            isWall = ((TouchLeftWall && playerController.inputDirection.x < 0 || TouchRightWall && playerController.inputDirection.x > 0) && rb.velocity.y < 0);
        }

    }
    private void OnDrawGizmosSelected() //Ñ¡ÔñÊ±»æÖÆ¼ì²â·¶Î§
    {
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset,chaeckRaduis);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, chaeckRaduis);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, chaeckRaduis);
    }


}
