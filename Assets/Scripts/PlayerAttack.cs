using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int damage;
    public float startTime;
    public float time;
    private Animator anim;
    private PolygonCollider2D coll2d;
    // Start is called before the first frame update
    void Start()
    {
                
        anim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        coll2d = GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Attack();
    }

    void Attack()
    {
        if(Input.GetButtonDown("Fire1"))
        {        
            anim.SetTrigger("Attack");
            StartCoroutine(startAttack());
            if(  anim.GetBool("DoubleJump"))
            {
            anim.SetBool("DoubleJump",false);
            anim.SetBool("JumpDown",true);
            }

        }
    }

    IEnumerator startAttack()
    {
        yield return new WaitForSeconds(startTime);
        coll2d.enabled=true;
        StartCoroutine(disableHitBox());
    }
    IEnumerator disableHitBox()
    {
        yield return new WaitForSeconds(time);
         coll2d.enabled=false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().TakeDamege(damage);
        }

    }


}
