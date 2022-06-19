using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float runSpeed;
    public float jumpSpeed;
    public float doubleJumpSpeed;
    private Rigidbody2D myRigidbody;
    private Animator myAnim;
    private BoxCollider2D myFeet;

    private bool isGround;
    private bool canDoubleJump;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
        myFeet = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        flip();
        Run();
        Jump();
        CheckGrounded();
        SwitchAnimation();
        Attack();
    }




    void flip()
    {
        bool playerHasXSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        if(playerHasXSpeed)
        {  
            if(myRigidbody.velocity.x > 0.1f)
            {
                transform.localRotation = Quaternion.Euler(0,0,0);
            }
            else if(myRigidbody.velocity.x < -0.1f)
            {
                transform.localRotation = Quaternion.Euler(0,180,0);
            }
        }
    }

    void Run()
    {
        float moveDir = Input.GetAxis("Horizontal");
        Vector2 PlayerVel = new Vector2(moveDir * runSpeed,myRigidbody.velocity.y);
        myRigidbody.velocity = PlayerVel;
        bool playerHasXSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        if(isGround){
            myAnim.SetBool("Run",playerHasXSpeed);
        }
    }

    void CheckGrounded(){

        isGround = myFeet.IsTouchingLayers(LayerMask.GetMask("Ground"));
        Debug.Log(isGround);
    }

    void SwitchAnimation()
    {
         myAnim.SetBool("Idle",false);
         
        if(myAnim.GetBool("JumpUp"))
        {
            if( myRigidbody.velocity.y < 0.0f)
            {
                myAnim.SetBool("JumpUp",false);
                myAnim.SetBool("JumpDown",true);
            }
    
        }else if (isGround)
        {
            myAnim.SetBool("DoubleJump",false);
            myAnim.SetBool("JumpUp",false);
            myAnim.SetBool("JumpDown",false);
            if( Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon)
            {
                myAnim.SetBool("Run",true);
            }else
            {
                myAnim.SetBool("Idle",true);
            }
            
        }
        
    }

    void Jump()
    {
        if(Input.GetButtonDown("Jump"))
        {
            myAnim.SetBool("Run",false);
            if(isGround){
                myAnim.SetBool("JumpUp",true);
              
                Vector2 jumpVel = new Vector2(0.0f, jumpSpeed);
                myRigidbody.velocity = Vector2.up * jumpVel;
                canDoubleJump = true;
            }
            else
            {
                
                if(canDoubleJump)
                {
                    myAnim.SetBool("JumpUp",false);
                    myAnim.SetBool("DoubleJump",true);
                   
                    Vector2 doubleJumpVel = new Vector2(0.0f, doubleJumpSpeed);
                    myRigidbody.velocity = Vector2.up * doubleJumpVel;
                    canDoubleJump = false;
                }
            }
        }
    }

    void Attack()
    {
         if(Input.GetButtonDown("Fire1"))
         {
             myAnim.SetTrigger("Attack");
             if(  myAnim.GetBool("DoubleJump"))
             {
                myAnim.SetBool("DoubleJump",false);
                myAnim.SetBool("JumpDown",true);
             }
           
         }

    }
}

