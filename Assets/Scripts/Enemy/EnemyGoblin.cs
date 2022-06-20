using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGoblin : Enemy
{
    public float speed;
    public float startWaitTime;

    public Transform movePos;
    public Transform LeftPos;
    public Transform RightPos;



    private float waitTime;
    private Animator myAnim;

    // Start is called before the first frame update
    public void Start()
    {

        base.Start();
        waitTime = startWaitTime;
        movePos.position = GetRandomPos();
        myAnim = GetComponent<Animator>();
    }


    // Update is called once per frame
    public void Update()
    {
        base.Update();
        flip();
        move();
        

    }

    public Vector2 GetRandomPos()
    {
        Vector2 rndPos = new Vector2(Random.Range(LeftPos.position.x,RightPos.position.x),transform.position.y);
        return rndPos;
    }
    
    void move()
    {
        transform.position = Vector2.MoveTowards(transform.position,movePos.position,speed*Time.deltaTime);
        if(Vector2.Distance(transform.position,movePos.position)<0.1f)
        {
            if(waitTime <=0)
            {
                waitTime = startWaitTime;
                movePos.position = GetRandomPos();
                myAnim.SetBool("Idle",false);
                myAnim.SetBool("Run",true);
            }
            else
            {
                waitTime -= Time.deltaTime;
                myAnim.SetBool("Idle",true);
                myAnim.SetBool("Run",false);
            }
        }
    }
    void flip()
    {
        if(movePos.position.x > transform.position.x)
        {
             transform.localRotation = Quaternion.Euler(0,180,0);
        }else if (movePos.position.x < transform.position.x)
        {
             transform.localRotation = Quaternion.Euler(0,0,0);
        }

    }
    void SwitchAnimation()
    {
     
    }

}
