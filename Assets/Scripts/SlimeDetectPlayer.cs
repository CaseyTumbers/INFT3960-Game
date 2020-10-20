using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeDetectPlayer : MonoBehaviour
{
    public GameObject attackArea;
    public GameObject siblingDetectArea;
    public GameObject turnAroundScript;
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
            startCooldown();
            GetComponentInParent<SlimeMovement>().setIsAttacking(true);
            siblingDetectArea.GetComponent<BoxCollider2D>().enabled = false;
            this.GetComponent<BoxCollider2D>().enabled = false;
            if (this.gameObject.name.Contains("Right") && !turnAroundScript.GetComponent<SlimeTurnAround>().getTurned())
            {
                faceRight();
                print("Right side");  
            }
            else if(this.gameObject.name.Contains("Right") && turnAroundScript.GetComponent<SlimeTurnAround>().getTurned())
            {
                faceLeft();
            }
            
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
    }

    void stopAttack()
    {
        animator.SetBool("attack", false);
        attackArea.GetComponent<BoxCollider2D>().enabled = false;
        Invoke("resetCooldown", 2f);
        siblingDetectArea.GetComponent<BoxCollider2D>().enabled = true;

        if (this.gameObject.name.Contains("Right") && !turnAroundScript.GetComponent<SlimeTurnAround>().getTurned())
        {
            Invoke("faceLeft", 0.2f);
        }
        else if (this.gameObject.name.Contains("Right") && turnAroundScript.GetComponent<SlimeTurnAround>().getTurned())
        {
            Invoke("faceRight", 0.2f);
        }
        this.GetComponent<BoxCollider2D>().enabled = true;
        GetComponentInParent<SlimeMovement>().setIsAttacking(false);
    }

    void startCooldown()
    {
        coolDown = true;
        siblingDetectArea.gameObject.GetComponent<SlimeDetectPlayer>().setCooldown(true);
    }
    void resetCooldown()
    {
        print("Reset Cooldown");
        coolDown = false;
        siblingDetectArea.gameObject.GetComponent<SlimeDetectPlayer>().setCooldown(false);
    }

    public void setCooldown(bool b)
    {
        coolDown = b;
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
