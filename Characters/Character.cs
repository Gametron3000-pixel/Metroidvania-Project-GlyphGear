using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]

public abstract class Character : MonoBehaviour
{
    [Header("Movement Variables")]
    [SerializeField] protected float speed = 1.0f;
    [SerializeField] protected float direction;

    protected bool facingRight = true;

    [Header("Jump Details")]
    [SerializeField] protected float jumpForce;
    [SerializeField] protected float jumpTime;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float radOCircle;
    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] protected bool grounded;
    protected float jumpTimeCounter;
    protected bool stoppedJumping;
    
    //[Header("Attack Variables")]
    
    //[Header("Character Stats")]

    protected Rigidbody2D rb;
    protected Animator myAnimator;

    #region monos
    public virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        jumpTimeCounter = jumpTime;
    }
    public virtual void Update()
    {
        //what it means to be grounded
        grounded = Physics2D.OverlapCircle(groundCheck.position,radOCircle,whatIsGround);
        
        //check vertical velocity
        if(rb.velocity.y < 0)
        {
            myAnimator.SetBool("Falling", true);
        }
    }
    public virtual void FixedUpdate()
    {
        //handle mechanics/physics
        HandleMovement();
        HandleLayers();
    }
    #endregion

    #region mechanics
    protected void Move()
    {
        rb.velocity = new Vector2(direction * speed, rb.velocity.y);
    }

    protected void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }
    #endregion

    #region subMechanics
    protected abstract void HandleJumping();
    protected virtual void HandleMovement()
    {
        Move();
    }
    protected void TurnAround(float horizontal)
    {
        if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight)
        {
            facingRight = !facingRight;
            transform.localScale = new Vector2(transform.localScale.x *-1, transform.localScale.y);
        }
    }
    protected void HandleLayers()
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
    #endregion

    #region visdebugs
private void OnDrawGizmos()
{
    Gizmos.DrawSphere(groundCheck.position, radOCircle);
}
    #endregion
}
