using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    private Animator animate;
    public int health;

    private void Awake()
    {
        animate = this.GetComponent<Animator>();
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Equals("AttackArea"))
        {
            health--;
            if(health <= 0)
            {
                animate.SetTrigger("break");
                print("broke it");
            }
            else
            {
                print("OUCH THATHURT");
                animate.SetTrigger("hit");
            }
        }
    }

    public void Break()
    {
        health--;
        if (health <= 0)
        {
            animate.SetTrigger("break");
            print("broke it");
            this.GetComponent<BoxCollider2D>().enabled = false;
        }
        else
        {
            print("OUCH THATHURT");
            animate.SetTrigger("hit");
        }
       
    }
}
