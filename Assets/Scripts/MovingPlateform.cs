using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlateform : MonoBehaviour
{
    public float speed;
    public float waitTime;
    public Transform[] movePos;  
    private int i;
    private Transform playerDefTransform;
    // Start is called before the first frame update
    
    void Start()
    {
        i=1;
        playerDefTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().parent;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, movePos[i].position , speed * Time.deltaTime);
        if( Vector2.Distance(transform.position,movePos[i].position) < 0.1f){

            if(waitTime < 0.0f){
                if(i == movePos.Length -1 ){
                    i=0;
                }else{
                    i++;
                }
                waitTime = 0.5f;
            }else
            {
                waitTime -= Time.deltaTime;
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && other.GetType().ToString()=="UnityEngine.BoxCollider2D")
        {
            other.gameObject.transform.parent = gameObject.transform;
        }
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player") && other.GetType().ToString()=="UnityEngine.BoxCollider2D")
        {
            other.gameObject.transform.parent =  playerDefTransform;
        }
        
    }
}
