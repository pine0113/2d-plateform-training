using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class  Enemy : MonoBehaviour
{

    public int hp;
    public int damage;
    // Start is called before the first frame update
    public void Start()
    {
        
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
        hp -= damage;

    }

}
