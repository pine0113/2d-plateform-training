using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinItem : MonoBehaviour
{
    private Animator myanim;
    // Start is called before the first frame update
    void Start()
    {
        myanim = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player")&&other.GetType().ToString()=="UnityEngine.CapsuleCollider2D")
        {
            CoinUI.current_coin_quantity += 1;
            myanim.SetTrigger("IsPickUP");
           
        }   
    }

    private void bePickUPed()
    {
        Destroy(gameObject);
    }
}
