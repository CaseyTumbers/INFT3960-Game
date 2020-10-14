using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeDetectPlayer : MonoBehaviour
{
    public GameObject attackArea;
    public GameObject siblingDetectArea;
    private Animator animator;
    private bool coolDown = false;
    

    // Start is called before the first frame update
    void Awake()
    {
        attackArea.GetComponent<BoxCollider2D>().enabled = false;
        animator = GetComponentInParent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //print("OW");
        if (collision.gameObject.name.Equals("Main Character") && !coolDown)
        {
            GetComponentInParent<SlimeMovement>().setIsAttacking(true);
            if (this.gameObject.name.Contains("Right"))
            {
                faceRight();
                print("Right side");  
            }
            siblingDetectArea.GetComponent<BoxCollider2D>().enabled = false;
            this.GetComponent<BoxCollider2D>().enabled = false;
            animator.SetTrigger("charge");
            Invoke("attackAnim", 1);
            Invoke("stopAttack", 2);
        }
    }

    void attackAnim()
    {
        animator.SetBool("attack", true);
        Invoke("attack", 0.15f);
    }

    void attack()
    {
        attackArea.GetComponent<BoxCollider2D>().enabled = true;
        coolDown = true;
    }

    void stopAttack()
    {
        animator.SetBool("attack", false);
        attackArea.GetComponent<BoxCollider2D>().enabled = false;
        coolDown = false;
        siblingDetectArea.GetComponent<BoxCollider2D>().enabled = true;
        this.GetComponent<BoxCollider2D>().enabled = true;
        GetComponentInParent<SlimeMovement>().setIsAttacking(false);
        if (this.gameObject.name.Contains("Right"))
        {
            Invoke("faceLeft", 0.1f);
        }
    }

    void faceRight()
    {
        transform.parent.eulerAngles = new Vector3(0, 180, 0);
    }

    void faceLeft()
    {
        transform.parent.eulerAngles = new Vector3(0, 0, 0);
    }
}
