using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int hp;
    public int blinks;
    public float blinkTime;
    public float dieTime;
    private Renderer myRenderer;
    private Animator myAnim;
    // Start is called before the first frame update
    void Start()
    {
        myRenderer = GetComponent<Renderer>();
        myAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DamagePlayer(int damage)
    {
        hp-=damage;
        if(hp<=0)
        {
               myAnim.SetTrigger("Death");
               Invoke("KillPlayer",dieTime);
        }
        BlinkPlayer(blinks,blinkTime);
    }

    void KillPlayer()
    {
        Destroy(gameObject);
    }
    void BlinkPlayer(int numBlinks, float seconds)
    {
        StartCoroutine(DoBlinks(numBlinks,seconds));
    }

    IEnumerator DoBlinks(int numBlinks, float seconds)
    {
        for (int i=0; i < numBlinks*2; i++)
        {
            myRenderer.enabled = !myRenderer.enabled;
            yield return new WaitForSeconds(seconds);
        }
        myRenderer.enabled = true;
    }
}
