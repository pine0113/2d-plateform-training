using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class  Enemy : MonoBehaviour
{

    public int hp;
    public int damage;
    public float flashTime;
    public GameObject bloodEffect;

    private SpriteRenderer sr;
    private Color originColor;

    // Start is called before the first frame update
    public void Start()
    {
        sr  = GetComponent<SpriteRenderer>();
        originColor = sr.color;
    }

    // Update is called once per frame
    public void Update()
    {
        if(hp<=0)
        {
            Death();
        }

    }

    void Death(){
        Destroy(gameObject);
    }

    public  void TakeDamege(int damage)
    {
        Instantiate(bloodEffect,transform.position,Quaternion.identity);
        hp -= damage;
        FlashColor(flashTime);
        GameController.camShake.Shake();

    }
    void FlashColor(float time)
    {
        sr.color=Color.red;
        Invoke("ResetColor",time);
    }

    void ResetColor()
    {
        sr.color = originColor;
    }


}
