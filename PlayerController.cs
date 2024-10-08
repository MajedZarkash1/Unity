using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
//=================================================================================
[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]
public class PlayerController : MonoBehaviour
//==================================================================================
{
    public float walkSpeed = 5f;
    public float runspeed = 8f;
    public float jumpImpulse = 1f;
    public float airWalkSpeed = 4f;
    Vector2 moveInput;
    TouchingDirections touchingDirections;

    public float CurrentMoveSpeed
    {
        get
        {
            if (IsMoving && !touchingDirections.IsOnWall)
            {
                if (touchingDirections.IsGrounded)
                {
                    if (IsRunning)
                    {
                        return runspeed;
                    }
                    else
                    {
                        return walkSpeed;
                    }
                }
                else
                {
                    return airWalkSpeed;
                }
            }
            else
            {
                return 0;
            }
        }
    }
    

    [SerializeField]
    private bool _isMoving = false;

    public bool IsMoving { get 
    {
        return _isMoving;
    }
    private set
    {
        _isMoving = value;
        animator.SetBool(AnimationStrings.isMoving,value);
    }
    }

    [SerializeField]
    private bool _isRunning = false;

    public bool IsRunning
    {
        get
        {
            return _isRunning;
        }
        set
        {
            _isRunning = value;
            animator.SetBool(AnimationStrings.isRunning, value);

        }
    }

    public bool _isFacingRight = true;

    public bool IsFacingRight { get { return _isFacingRight; } private set {
        if(_isFacingRight != value)
        {
            //flip the local scale to make the player face the oppiste direction
            transform.localScale *= new Vector2(-1, 1);

        }
        _isFacingRight = value;
    } }

    Rigidbody2D rb;
    Animator animator;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
    }

// ==================================================================================

//===================================================================================
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);
        animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);
        

    }
//====================================================================================
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        IsMoving = moveInput != Vector2.zero;

        SetFacingDirection(moveInput);
    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if(moveInput.x > 0 && !IsFacingRight)
        {
            //face the right
            IsFacingRight = true;
        }
        else if (moveInput.x < 0 && IsFacingRight)
        {
            //face the left
            IsFacingRight = false;
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsRunning = true;
        } else if(context.canceled)
        {
            IsRunning = false;
        }
    }
//=====================================================================================
//Jumping
    public void OnJump(InputAction.CallbackContext context)
    {
        // TODO check if alive too
        if(context.started && touchingDirections.IsGrounded)
        {
            animator.SetTrigger(AnimationStrings.jump);
            rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
        }
    }
}
