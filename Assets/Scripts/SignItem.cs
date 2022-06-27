using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SignItem : MonoBehaviour
{

    public string signText = "please go right";
    public GameObject textFrame;
    public TextMeshProUGUI signTMPText;

    private bool isPlayerInSign;
    
    // Start is called before the first frame update
    void Start()
    {  
        
    }

    // Update is called once per frame
    void Update()
    {
          float moveY = Input.GetAxis("Vertical");
            if(moveY > 0.5f  && isPlayerInSign)
            {
                signTMPText.text = signText;
                textFrame.SetActive(true);
            }
    }
    
   private void OnTriggerStay2D(Collider2D other)
    {       
        if(other.gameObject.CompareTag("Player") && other.GetType().ToString()=="UnityEngine.CapsuleCollider2D")
        {
             isPlayerInSign = true;
 
          
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
         if(other.gameObject.CompareTag("Player"))
        {           
            isPlayerInSign = false;
            textFrame.SetActive(false);
        }  
    }


}
