using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    private float runSpeed = 2.0f;
    private float walkSpeed = 1.0f; 
    public override void Start()
    {
        base.Start();
        speed = runSpeed;
    }
    public override void Update()
    {
        base.Update();
        direction = Input.GetAxisRaw("Horizontal");
        HandleJumping();
    }
    protected override void HandleMovement()
    {
        base.HandleMovement();
        myAnimator.SetFloat("speed", direction);
        TurnAround(direction);
    }

    protected override void HandleJumping()
    {
        if (grounded)
        {
            jumpTimeCounter = jumpTime;
            myAnimator.ResetTrigger("Jump");
            myAnimator.SetBool("Falling", false);
        }

        //use space and w to jump
        if (Input.GetButtonDown("Jump") && grounded)
        {
            //Jump!
            Jump();
            stoppedJumping = false;
            //tell the animator to play jump animation
            myAnimator.SetTrigger("Jump");
        }

        //to keep jumping while button is held
        if (Input.GetButton("Jump") && !stoppedJumping && (jumpTimeCounter > 0))
        {
            //Jump!
            Jump();
            jumpTimeCounter -= Time.deltaTime;
            myAnimator.SetTrigger("Jump");
        }
        
        //if we stop holding button
        if (Input.GetButtonUp("Jump"))
        {
            jumpTimeCounter = 0;
            stoppedJumping = true;
            myAnimator.SetBool("Falling", true);
            myAnimator.ResetTrigger("Jump");
        }
    }
}
