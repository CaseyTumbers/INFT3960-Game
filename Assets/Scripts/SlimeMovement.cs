using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[System.Serializable]
public class SlimeMovement : controllableEnemy
{
    public float speed = 10f;
    public float direction = -1f;
    private Rigidbody2D rbody;
    private Animator animate;
    private bool isAttacking = false;
    private bool mountAnim = false;
    //private bool isControlled = false;

    void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        animate = GetComponent<Animator>();
    }

    void Update()
    {
        if (isControlled)
        {
            if (!mountAnim)
            { 
                animate.SetTrigger("slimeMounted");
                mountAnim = true;
            }
            if (Input.GetKey("left"))
            {
                player.GetComponent<Player>().faceLeft();
            }
            else if (Input.GetKey("right"))
            {
                player.GetComponent<Player>().faceRight();
            }
            Move();
            DisableAggro();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                player.GetComponent<Player>().setControllingCreature(false);
                player.GetComponent<CollideWithEnemy>().setCoolDown(true);
                setIsControlled(false);
                launchPlayer();
                setPlayer(null);
                mountAnim = false;
            }
        }
        else
        {
            EnableAggro();
            if (!isAttacking)
            {
                AIMove();
            }
            else
            {
                rbody.velocity = new Vector2(0,0);
            }
        }
    }

    void AIMove()
    {
        rbody.velocity = new Vector2(speed * direction, rbody.velocity.y);
    }
    void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float moveBy = x * speed;
        rbody.velocity = new Vector2(moveBy, rbody.velocity.y);
    }

    void EnableAggro()
    {
        if (!GetComponentInChildren<SlimeDetectPlayer>().enabled)
        {
            GetComponentInChildren<SlimeDetectPlayer>().enabled = true;
        }
    }

    void DisableAggro()
    {
        if (GetComponentInChildren<SlimeDetectPlayer>().enabled)
        {
            GetComponentInChildren<SlimeDetectPlayer>().enabled = false;
        }
    }

    void launchPlayer() 
    {
        animate.SetTrigger("launch");
        player.GetComponent<Player>().getAnimator().SetBool("isMounted", false);
        Rigidbody2D playerBody = player.GetComponent<Rigidbody2D>();
        playerBody.velocity = new Vector2(playerBody.velocity.x, 50f);
        //playerBody.velocity += Vector2.up * Physics2D.gravity * (250 - 1) * Time.deltaTime;
    }

    public void setIsAttacking(bool a)
    {
        isAttacking = a;
    }

    public bool getIsAttacking()
    {
        return isAttacking;
    }

    public bool getIsControlled()
    {
        return isControlled;
    }
}
