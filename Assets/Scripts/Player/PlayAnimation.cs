using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimation : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
    }

    private void Update()
    {
        SetAnimation();
    }
    public void SetAnimation()
    {
        anim.SetFloat("VelociteX",Mathf.Abs(rb.velocity.x));
        anim.SetFloat("VelociteY", rb.velocity.y);
        anim.SetBool("isGround", physicsCheck.isGround);
    }
}