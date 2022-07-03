using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float runSpeed;
    public float jumpSpeed;
    public float wallJumpPower=6f;
    public float doubleJumpSpeed;
    public float climbLadderSpeed;

    private Animator myAnim;
    private Rigidbody2D myPhysicalBody;
    private PolygonCollider2D myCoreBody;
    private BoxCollider2D myFeet;
    private Transform myTransform;

    public float restoreTime;
    private bool isOnOneWayPlatform;
    private bool isGround;
    private bool canDoubleJump;

    private bool isOnLadder;
    private bool isClimpingLadder;
    private bool isJumpingUp;
    private bool isJumpingDown;    
    private bool isDoubleJumping;
    private bool isClimb;    
    private float playerGravity;
    private int direction;
    private float ClimbWallRadius=0.3f;
    private float slideSpeed = 0.7f;

    public bool isEquipSword=false;

    private bool isClimbWallCheck;
    public Transform ClimbWallCheckPoint;
    public LayerMask ClimbWallLayerMask;
    public bool disableXmove=false;

    // Start is called before the first frame update
    void Start()
    {
        myPhysicalBody = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
        myFeet = GetComponent<BoxCollider2D>();
        myCoreBody = GetComponent<PolygonCollider2D>();
        myTransform = GetComponent<Transform>();
        playerGravity = myPhysicalBody.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameController.isGameAlive){
            CheckAirStatus();
            flip();
            Run();
            Climb();
            ClimbWallCheck();
            Jump();
            ClimbLadder();
            ClimbingLadder();
            CheckGrounded();
            CheckLadder();
            SwitchAnimation();
            OneWayPlatformCheck();
            Attack();
        }
        
    }
    private void LateUpdate() {

        //WallJumpFix();
        
    }

    
    void ClimbWallCheck()
    {
         if(!isGround)
         {
            ClimbWallRadius = 0.3f;
            isClimbWallCheck = Physics2D.OverlapCircle(ClimbWallCheckPoint.position,ClimbWallRadius,ClimbWallLayerMask);
            if(!isClimbWallCheck)
            {
                isClimb = false;
            }

            if(isClimbWallCheck )
            {
                if((direction==1 &&  Input.GetAxis("Horizontal") > 0.1f ) || (direction==-1 &&  Input.GetAxis("Horizontal") < -0.1f ))
                {               
                
                    isClimb = true;
                    HandleWallSliding();                    
        
                }else
                {
                    isClimb = false;
                }
            }
            
        }
    }

    void HandleWallSliding()
    {
        myPhysicalBody.velocity = new Vector2(myPhysicalBody.velocity.x,-slideSpeed);
        isClimb = true;
      
    }

    void flip()
    {
        //direction=1;
        bool playerHasXSpeed = Mathf.Abs(myPhysicalBody.velocity.x) > Mathf.Epsilon;
        if(playerHasXSpeed)
        {  
            if(myPhysicalBody.velocity.x > 0.1f)
            {
                direction=1;
                transform.localRotation = Quaternion.Euler(0,0,0);
            }
            else if(myPhysicalBody.velocity.x < -0.1f)
            {
                direction=-1;
                transform.localRotation = Quaternion.Euler(0,180,0);
            }
        }
    }

    void Run()
    {
        if(!disableXmove){
            float moveDir = Input.GetAxis("Horizontal");

            Vector2 PlayerVel = new Vector2(moveDir * runSpeed,myPhysicalBody.velocity.y);
            myPhysicalBody.velocity = PlayerVel;
            bool playerHasXSpeed = Mathf.Abs(myPhysicalBody.velocity.x) > Mathf.Epsilon;

            if(isGround){
                myAnim.SetBool("Run",playerHasXSpeed);
            }else
            {
                myAnim.SetBool("JumpDown",playerHasXSpeed);
            }
        }
    }

    IEnumerator delayEnableXmove()
    {
        yield return new WaitForSeconds(0.05f);
        disableXmove = false;
    }
    void CheckGrounded(){

        isGround = myFeet.IsTouchingLayers(LayerMask.GetMask("Ground")) || 
        myFeet.IsTouchingLayers(LayerMask.GetMask("MovingPlatform")) ||
        myFeet.IsTouchingLayers(LayerMask.GetMask("OneWayPlatform"));

        isOnOneWayPlatform = myFeet.IsTouchingLayers(LayerMask.GetMask("OneWayPlatform"));
        // Debug.Log(isGround);
    }

    void CheckLadder()
    {
        isOnLadder =  myCoreBody.IsTouchingLayers(LayerMask.GetMask("Ladder"));
    }
    void SwitchAnimation()
    {
         myAnim.SetBool("Idle",false);
         

        if(myAnim.GetBool("JumpUp"))
        {
            if( myPhysicalBody.velocity.y < 0.0f )
            {
                myAnim.SetBool("JumpUp",false);
                myAnim.SetBool("JumpDown",true);
            }
    
        }else if (isGround)
        {
            myAnim.SetBool("DoubleJump",false);
            myAnim.SetBool("JumpUp",false);
            myAnim.SetBool("JumpDown",false);
            if( Mathf.Abs(myPhysicalBody.velocity.x) > Mathf.Epsilon)
            {
                myAnim.SetBool("Run",true);
            }else
            {
                myAnim.SetBool("Idle",true);
            }
            
        }
        
        if(isClimb)
        {
            myAnim.SetBool("JumpUp",false); 
            myAnim.SetBool("JumpDown",false);      
            myAnim.SetBool("DoubleJump",false);   
            myAnim.SetBool("Run",false);      
            myAnim.SetBool("Climb",true);
        }else
        {
            myAnim.SetBool("Climb",false);      
        }
        
    }



    void Jump()
    {
        if(Input.GetButtonDown("Jump"))
        {
            myAnim.SetBool("Run",false);
            myAnim.SetBool("Climb",false);
            if(isGround){
                float moveY = Input.GetAxis("Vertical");
                if( isOnOneWayPlatform && moveY < -0.1f)
                {
                    gameObject.layer = LayerMask.NameToLayer("OneWayPlatform");
                    canDoubleJump = true;
                    Invoke("RestorePlayerLayer", restoreTime);
                }else{
                    myAnim.SetBool("JumpUp",true);
                
                    Vector2 jumpVel = new Vector2(0.0f, jumpSpeed);
                    myPhysicalBody.velocity = Vector2.up * jumpVel;
                    canDoubleJump = true;
                }
            }
            else
            {
                
                if(canDoubleJump)
                {
                    myAnim.SetBool("JumpUp",false);
                    myAnim.SetBool("DoubleJump",true);
                   
                    Vector2 doubleJumpVel = new Vector2(0.0f, doubleJumpSpeed);
                    myPhysicalBody.velocity = Vector2.up * doubleJumpVel;
                    canDoubleJump = false;
                }
            }

          

            if(isClimpingLadder)
            {
                myAnim.SetBool("ClimbLadder",false);
                myAnim.SetBool("JumpUp",true);
                isClimpingLadder = false;
                Vector2 jumpVel = new Vector2( - direction *  2f, jumpSpeed);
                myPhysicalBody.velocity = Vector2.up * jumpVel;
                canDoubleJump = true;
            }


            if(isClimbWallCheck )
            {
                myAnim.SetBool("JumpUp",true);
                myAnim.SetBool("JumpDown",false);
                myAnim.SetBool("DoubleJump",false);
                isClimb =false;
                if((direction==1 &&  Input.GetAxis("Horizontal") > 0.1f ) || (direction==-1 &&  Input.GetAxis("Horizontal") < -0.1f ))
                {                         
                    direction = -direction;
                    //myTransform.position = new Vector2( myTransform.position.x + direction*0.3f, myTransform.position.y+0.3f);
                    myPhysicalBody.velocity= (new Vector2(direction * runSpeed , wallJumpPower));
                    Debug.Log(myPhysicalBody.velocity.x);
                    canDoubleJump = true;
                    disableXmove=true;
                    StartCoroutine(delayEnableXmove());
                }
                     
                
            }
            

        }

        if( myPhysicalBody.velocity.y < 0.0f && !isOnLadder)
        {
            myAnim.SetBool("JumpDown",true);            
        }
    }

    void Climb()
    {
        if(isClimb)
        {
           myPhysicalBody.gravityScale=0;
           myPhysicalBody.velocity = new Vector2(0f,0f);      
              
        }
           myPhysicalBody.gravityScale=playerGravity;
           
    }

    void ClimbLadder()
    {
        if(isOnLadder)
        {
            float moveY = Input.GetAxis("Vertical");
            if(moveY > 0.5f || moveY < -0.5)
            {
                myAnim.SetBool("ClimbLadder",true);
                myPhysicalBody.gravityScale = 0.0f;               
            }
            else
            {
                myAnim.SetBool("ClimbLadder",false);
                if(isJumpingDown||isJumpingUp||isDoubleJumping)
                {
                   
                }  
                else
                {
                    myPhysicalBody.velocity = new Vector2(myPhysicalBody.velocity.x, 0.0f);
                }
                
            }

        }else
        {
            myAnim.SetBool("ClimbLadder",false);
            myPhysicalBody.gravityScale = playerGravity;
        }
         
    }

    void ClimbingLadder()
    {
        if(isOnLadder)
        {
            if(isClimpingLadder)
            {
                myPhysicalBody.gravityScale = 0.0f;               
                float moveY = Input.GetAxis("Vertical");
                if(moveY > 0.5f || moveY < -0.5)
                {
                    myPhysicalBody.velocity = new Vector2(myPhysicalBody.velocity.x, moveY * climbLadderSpeed);
                    myAnim.SetBool("ClimbingLadder",true);
                    
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
                    myPhysicalBody.velocity = new Vector2(myPhysicalBody.velocity.x, 0.0f);
                }

            }
        }else
        {
            myAnim.SetBool("ClimbingLadder",false);
        }
    }
    void Attack()
    {
        if(isEquipSword){
            myAnim.SetBool("SwordAttack",true);
        }else
        {
            myAnim.SetBool("SwordAttack",false);
        }

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
           //myRigidbody.velocity *= new Vector2(0.0f, 1f) ;
        }

        
       
        
    }

    void OneWayPlatformCheck()
    {
        // if(isGround && gameObject.layer)
        // {

        // }

        float moveY = Input.GetAxis("Vertical");
        if(Input.GetButtonDown("Jump"))
        {
            if( isOnOneWayPlatform && moveY < -0.1f)
            {
                gameObject.layer = LayerMask.NameToLayer("OneWayPlatform");
                canDoubleJump = true;
                Invoke("RestorePlayerLayer", restoreTime);
            }
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


