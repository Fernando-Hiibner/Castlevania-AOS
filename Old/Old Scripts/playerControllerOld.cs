using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerControllerOld : MonoBehaviour
{
    [HideInInspector]
    public static playerControllerOld instance;
    //Movement variables
    [HideInInspector]
    public bool isWalking; //Caso eu precise checar se ele esta andando
    public float moveSpeed;

    //Jump variables
    public float jumpHeight;
    [HideInInspector]
    public bool isGrounded = true;
    public LayerMask GroundLayer;
    public Transform groundCheckObject;
    public float checkRadius = 0.2f;
    public bool canDoubleJump;
    bool DoubleJump;


    //Miscelaneous
    [HideInInspector]
    public Rigidbody2D myRB;
    Animator myAnim;
    [HideInInspector]
    public bool attacking;
    [HideInInspector]
    public bool isCrouched;

    bool facingRight;
    [HideInInspector]
    public float move;
    [HideInInspector]
    public float crouch;
    [HideInInspector]
    public bool canMove = true;
    [HideInInspector]
    public float animCrouch;
    [HideInInspector]
    public float moveAxis;
    [HideInInspector]
    public bool jump;



    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
        facingRight = true;
        isWalking = false;
        instance = this;
        DoubleJump = canDoubleJump;
    }

    void Update()
    {
        animCrouch = Input.GetAxis("Vertical");
        if(animCrouch < 0 && isGrounded && !attacking)
        {
            myAnim.SetFloat("crouch", Mathf.Abs(animCrouch));
        }
        if(animCrouch == 0 || animCrouch > 0)
        {
            myAnim.SetFloat("crouch", Mathf.Abs(animCrouch));
        }
        myAnim.SetBool("isGrounded", isGrounded);
        myAnim.SetFloat("vspeed", myRB.velocity.y);
        if (canMove)
        {
            move = Input.GetAxis("Horizontal");
            moveAxis = 0f;
        }
        else if (!canMove)
        {
            moveAxis = Input.GetAxis("Horizontal");
            move = 0f;
            if (moveAxis > 0 && !facingRight && !attacking)
            {
                Flip();
            }
            else if (moveAxis < 0 && facingRight && !attacking)
            {
                Flip();
            }
        }
        jump = Input.GetButtonDown("Jump"); //Util pra quando é algo que não deve acontecer mais de uma vez pq demorou pra atualizar input
        if (jump)
        {
            if (isGrounded && !attacking)
            {
                myAnim.SetBool("isGrounded", isGrounded);
                myRB.velocity = new Vector2(myRB.velocity.x, 0);
                myRB.AddForce(new Vector2(0, jumpHeight));
            }
            else
            {
                if (DoubleJump && !attacking)
                {
                    myRB.velocity = new Vector2(myRB.velocity.x, 0);
                    myRB.AddForce(new Vector2(0, jumpHeight));
                    DoubleJump = false;
                }
            }
        }
        if (isGrounded)
        {
            DoubleJump = canDoubleJump;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheckObject.position, checkRadius, GroundLayer);
        if (move != 0){
            myRB.velocity = new Vector2(moveSpeed*move, myRB.velocity.y);
            isWalking = true; 
            myAnim.SetFloat("speed", Mathf.Abs(move));
            myAnim.SetBool("isWalking", isWalking);
        }else{
            isWalking = false;
            myAnim.SetBool("isWalking", isWalking);
        }
        if(move > 0 && !facingRight){
            Flip();
        }else if(move < 0 && facingRight){
            Flip();
        }
    }
    void Flip(){
        facingRight = !facingRight;
        Vector3 flipScale = transform.localScale;
        flipScale.x *= -1;
        transform.localScale = flipScale;
    }
    /*
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(255, 0, 0);
        Gizmos.DrawWireSphere(groundCheckObject.position, checkRadius);
    }
    */
}
