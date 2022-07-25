using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]

public class PlayerJump : MonoBehaviour
{
    [Header("Jump Details")]
    public float jumpForce;
    public float jumpTime;
    private float jumpTimeCounter;
    private bool stoppedJumping;

    [Header("Ground Details")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float radOCircle;
    [SerializeField] private LayerMask whatIsGround;
    public bool grounded;

    [Header("Components")]
    private Rigidbody2D rb;
    private Animator myAnimator;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        jumpTimeCounter = jumpTime;
    }

    //myAnimator.SetBool("Falling", true);
    //myAnimator.SetBool("Falling", false);
    //myAnimator.SetTrigger("Jump");
    //myAnimator.ResetTrigger("Jump");

    private void Update()
    {
        //what it means to be grounded
        grounded = Physics2D.OverlapCircle(groundCheck.position,radOCircle,whatIsGround);

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
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            stoppedJumping = false;
            //tell the animator to play jump animation
            myAnimator.SetTrigger("Jump");
        }

        //to keep jumping while button is held
        if (Input.GetButton("Jump") && !stoppedJumping && (jumpTimeCounter > 0))
        {
            //Jump!
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
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

        if(rb.velocity.y < 0)
        {
            myAnimator.SetBool("Falling", true);
        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(groundCheck.position, radOCircle);
    }

    private void FixedUpdate()
    {
        HandleLayers();
    }

    private void HandleLayers()
    {
        if(!grounded)
        {
            myAnimator.SetLayerWeight(1, 1);
        }
        else
        {
            myAnimator.SetLayerWeight(1, 0);
        }
    }
}

