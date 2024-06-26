using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Sign : MonoBehaviour
{
    private PlayinputControl Playinput;
    public Animator anim;
    public bool CanPress;
    public GameObject SignSprite;
    public Transform PlayTrans;
    public IInteractable targetItem;
    private void Awake()
    {
        anim = SignSprite.GetComponent<Animator>();
        Playinput = new PlayinputControl();
        Playinput.Enable();
    }

    private void OnEnable()
    {
        InputSystem.onActionChange += OnActionChange;
        Playinput.GamePlayer.Confirm.started += OnCofirm;
    }

   

    private void Update()
    {
        SignSprite.GetComponent<SpriteRenderer>().enabled = CanPress;
        SignSprite.transform.localScale = PlayTrans.localScale;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("InterActable"))
        {
            CanPress = true;
            targetItem = other.GetComponent<IInteractable>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        CanPress = false;
    }

    private void OnActionChange(object obj, InputActionChange ActionChange)
    {
        if (ActionChange == InputActionChange.ActionStarted)
        {
            var d = ((InputAction)obj).activeControl.device;
            switch (d.device)
            {
                case Keyboard:
                    anim.Play("KeyBoard");
                    break;
            }
        }
    }

    private void OnCofirm(InputAction.CallbackContext obj)
    {
        if (CanPress)
        {
            targetItem.TriggerAction();
        }
    }
}
