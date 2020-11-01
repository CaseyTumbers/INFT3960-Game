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
    private GameObject temp;
    AudioSource audio;

    // Start is called before the first frame update
    void Awake()
    {
        attackArea.GetComponent<BoxCollider2D>().enabled = false;
        animator = GetComponentInParent<Animator>();
        audio = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //print("OW");
        if (collision.gameObject.name.Equals("Main Character"))
        {
            Attack();
        }
    }

    public void Attack()
    {
        if (!coolDown)
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
            else if (this.gameObject.name.Contains("Right") && turnAroundScript.GetComponent<SlimeTurnAround>().getTurned())
            {
                faceLeft();
            }

            animator.SetTrigger("charge");
            if (GetComponentInParent<controllableEnemy>().getIsControlled())
            {
                temp = GetComponentInParent<controllableEnemy>().getPlayer();
                temp.GetComponent<Player>().chargeAnim();
            }
            Invoke("attackAnim", 1);
            Invoke("stopAttack", 2);
        }
    }
    void attackAnim()
    {
        animator.SetBool("attack", true);
        if (GetComponentInParent<controllableEnemy>().getIsControlled())
        {
            temp.GetComponent<Player>().attackAnim(true);
        }
        Invoke("attack", 0.15f);
    }

    void attack()
    {
        audio.Play(0);
        attackArea.GetComponent<BoxCollider2D>().enabled = true;
    }

    void stopAttack()
    {
        animator.SetBool("attack", false);
        if (GetComponentInParent<controllableEnemy>().getIsControlled())
        {
            temp.GetComponent<Player>().attackAnim(false);
        }
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
