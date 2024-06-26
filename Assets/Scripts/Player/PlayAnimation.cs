using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimation : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;
    private PlayerController playerController;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        playerController = GetComponent<PlayerController>();
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
        anim.SetBool("isDeath", playerController.isDead);
        anim.SetBool("isAttack", playerController.isAttack);
        anim.SetBool("isWall", physicsCheck.isWall);
        anim.SetBool("isSlide", playerController.isSlide);
    }

    public void PlayHurt()
    {
        anim.SetTrigger("hurt");
    }

    public void PlayAttack()
    {
        anim.SetTrigger("attack");
    }
}