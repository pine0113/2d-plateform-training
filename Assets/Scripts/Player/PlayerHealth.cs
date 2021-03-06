using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int max_hp;
    public int hp;
    public int blinks;
    public float blinkTime;
    public float dieTime;
    public float hitBoxCdTime;
    private Renderer myRenderer;
    private Animator myAnim;
    private HPHud hphud;
    private ScreenFlash screenFlash;
    private PolygonCollider2D myPoly;
    // Start is called before the first frame update
    void Start()
    {
        myRenderer = GetComponent<Renderer>();
        myAnim = GetComponent<Animator>();
        hphud = GameObject.Find("HPHud").GetComponent<HPHud>();
        screenFlash = GetComponent<ScreenFlash>();
        myPoly = GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {   
        hphud.hp_max = max_hp;
        hphud.hp_current = hp;
    }

    public void DamagePlayer(int damage)
    {
        hp-=damage;
        if(hp<=0)
        {
               GameController.isGameAlive = false;
               myAnim.SetTrigger("Death");
               Invoke("KillPlayer",dieTime);

        }
        BlinkPlayer(blinks,blinkTime);
        screenFlash.FlashScreen();
        myPoly.enabled=false;
        StartCoroutine(ShowPlayerHitBox());
    }


    IEnumerator ShowPlayerHitBox()
    {
        yield return new WaitForSeconds(hitBoxCdTime);
        myPoly.enabled=true;
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
