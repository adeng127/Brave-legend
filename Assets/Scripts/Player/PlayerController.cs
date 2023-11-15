using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayinputControl inputControl;
    public Vector2 inputDirection;

    private void Awake()   //start之前运行
    {
        inputControl = new PlayinputControl();

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
}
