using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    private Animator animate;

    private void Awake()
    {
        animate = this.GetComponent<Animator>();
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Equals("AttackArea"))
        {
            animate.SetTrigger("break");
            print("broke it");
        }
    }

    public void Break()
    {
        animate.SetTrigger("break");
        this.GetComponent<BoxCollider2D>().enabled = false;
    }
}
