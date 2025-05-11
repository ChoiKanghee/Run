using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;

    private Animator anim;

    [Header("Movement Info")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float doubleJumpForce;

    private bool playerUnlocked;
    private bool canDoubleJump;

    [Header("Slide Info")]
    [SerializeField] private float slideSpeed;
    [SerializeField] private float slideTime;
    private float slideTimeCounter;
    private bool isSliding;

    [Header("Collision Info")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Vector2 wallCheckSize;
    private bool isGrounded;
    private bool wallDetected;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorControllers();
        CheckCollision();

        slideTimeCounter -= Time.deltaTime;

        if (playerUnlocked && !wallDetected)
            Movement();


        if (isGrounded)
            canDoubleJump = true;

        CheckForSlide();
        CheckInput();

    }

    private void CheckForSlide()
    {
        if (slideTimeCounter < 0;)
        {
            isSliding = false;
        }

    private void Movement()
    {
        if (isSliding)
            rb.velocity = new Vector2(slideSpeed, rb.velocity.y);

        else
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
    }


    private void AnimatorControllers()
    {
        anim.SetFloat("xVelocity", rb.velocity.x);
        anim.SetFloat("yVelocity", rb.velocity.y);

        anim.SetBool("canDoubleJump", canDoubleJump);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isSliding", isSliding);
    }

    private void CheckCollision()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        Debug.Log("Player Grounded: " + isGrounded);
        wallDetected = Physics2D.BoxCast(wallCheck.position, wallCheckSize, 0, Vector2.right, 0, whatIsGround);
    }

    private void CheckInput()
    {
        if (Input.GetButtonDown("Fire2"))
            playerUnlocked = true;
        Debug.Log("Player Run");

        if (Input.GetButtonDown("Jump"))
            JumpButton();
        //Debug.Log("Player Jump");

        if (Input.GetKeyDown(KeyCode.LeftShift))
            SlideButton();
        //Debug.Log("Player Slide");
    }

    private void SlideButton()
    {
            isSliding = true;
            slideTimeCounter = slideTime;
    }

    private void JumpButton()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            Debug.Log("Player Double Jump");
        }
        else if (canDoubleJump)
        {
            canDoubleJump = false;
            rb.velocity = new Vector2(rb.velocity.x, doubleJumpForce);
            Debug.Log("Player Jump");
        }
    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.red;
        //Gizmos.DrawRay(transform.position, Vector2.down * groundCheckDistance);
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundCheckDistance));
        Gizmos.DrawWireCube(wallCheck.position, wallCheckSize);
    }

}