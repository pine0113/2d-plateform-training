using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float runSpeed;
    public float jumpSpeed;
    public float doubleJumpSpeed;
    public float climbLadderSpeed;
    private Rigidbody2D myRigidbody;
    private Animator myAnim;

    private PolygonCollider2D myBody;
    private BoxCollider2D myFeet;

    public float restoreTime;
    private bool isOnOneWayPlatform;
    private bool isGround;
    private bool canDoubleJump;

    private bool isOnLadder;
    private bool isClimpingLadder;
    private bool isJumpingUp;
    private bool isJumpingDown;    
    private bool isDoubleJumping;    
    private float playerGravity;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
        myFeet = GetComponent<BoxCollider2D>();
        myBody = GetComponent<PolygonCollider2D>();
        playerGravity = myRigidbody.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameController.isGameAlive){
            CheckAirStatus();
            flip();
            Run();
            Jump();
            ClimbLadder();
            ClimbingLadder();
            CheckGrounded();
            CheckLadder();
            SwitchAnimation();
            OneWayPlatformCheck();
        }
        // Attack();
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
        }else
        {
            myAnim.SetBool("JumpDown",playerHasXSpeed);
        }
        
    }

    void CheckGrounded(){

        isGround = myFeet.IsTouchingLayers(LayerMask.GetMask("Ground")) || 
        myFeet.IsTouchingLayers(LayerMask.GetMask("MovingPlatform")) ||
        myFeet.IsTouchingLayers(LayerMask.GetMask("OneWayPlatform"));

        isOnOneWayPlatform = myFeet.IsTouchingLayers(LayerMask.GetMask("OneWayPlatform"));
        Debug.Log(isGround);
    }

    void CheckLadder()
    {
        isOnLadder =  myBody.IsTouchingLayers(LayerMask.GetMask("Ladder"));
    }
    void SwitchAnimation()
    {
         myAnim.SetBool("Idle",false);
         

        if(myAnim.GetBool("JumpUp"))
        {
            if( myRigidbody.velocity.y < 0.0f )
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

        if( myRigidbody.velocity.y < 0.0f && !isOnLadder)
        {
            myAnim.SetBool("JumpDown",true);            
        }
    }

    void ClimbLadder()
    {
        if(isOnLadder)
        {
            float moveY = Input.GetAxis("Vertical");
            if(moveY > 0.5f || moveY < -0.5)
            {
                myAnim.SetBool("ClimbLadder",true);
                myRigidbody.gravityScale = 0.0f;               
            }
            else
            {
                myAnim.SetBool("ClimbLadder",false);
                if(isJumpingDown||isJumpingUp||isDoubleJumping)
                {
                   
                }  
                else
                {
                    myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, 0.0f);
                }
                
            }

        }else
        {
            myAnim.SetBool("ClimbLadder",false);
            myRigidbody.gravityScale = playerGravity;
        }
         
    }

    void ClimbingLadder()
    {
        if(isOnLadder)
        {
            if(isClimpingLadder)
            {
                float moveY = Input.GetAxis("Vertical");
                if(moveY > 0.5f || moveY < -0.5)
                {
                    myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, moveY * climbLadderSpeed);
                    myAnim.SetBool("ClimbingLadder",true);
                    myRigidbody.gravityScale = 0.0f;               
                }
                else
                {
                    myAnim.SetBool("ClimbingLadder",false);
                    if(isJumpingDown||isJumpingUp||isDoubleJumping)
                    {
                        myAnim.SetBool("ClimbLadder",true);
                    
                    }  
                    else
                    {
                        myAnim.SetBool("ClimbLadder",true);
                        
                    }
                    myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, 0.0f);
                }

            }
        }else
        {
            myAnim.SetBool("ClimbingLadder",false);
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.GetType().ToString()=="UnityEngine.BoxCollider2D")
        {
           myRigidbody.velocity *= new Vector2(0.0f, 1f) ;
        }
        
    }

    void OneWayPlatformCheck()
    {
        // if(isGround && gameObject.layer)
        // {

        // }

        float moveY = Input.GetAxis("Vertical");
        if( isOnOneWayPlatform && moveY < -0.1f)
        {
            gameObject.layer = LayerMask.NameToLayer("OneWayPlatform");
            canDoubleJump = true;
            Invoke("RestorePlayerLayer", restoreTime);
        }


    }

    void RestorePlayerLayer()
    {
        if(!isGround && gameObject.layer != LayerMask.NameToLayer("Player")){
            gameObject.layer = LayerMask.NameToLayer("Player");
            
        }
    }


    void CheckAirStatus()
    {
        isJumpingUp = myAnim.GetBool("JumpUp");
        isJumpingDown = myAnim.GetBool("JumpDown");
        isDoubleJumping = myAnim.GetBool("DoubleJump");
        isClimpingLadder =  myAnim.GetBool("ClimbLadder");
        
    }
}


