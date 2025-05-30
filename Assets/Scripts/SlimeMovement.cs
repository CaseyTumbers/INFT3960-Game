﻿using System.Collections;
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

    private int slimeHealth = 1;
    private Transform initialPos;
    private float initialX, initialY;
    private bool respawnPointFlag = false;
    //private bool isControlled = false;

    void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        animate = GetComponent<Animator>();
        if (!respawnPointFlag)
        {
            respawnPointFlag = true;
            initialPos = this.transform;
            initialY = Mathf.Floor(this.transform.position.y);
            initialX = this.transform.position.x;
        
        }
    }

    void Update()
    {
        if (isControlled)
        {
            if (!mountAnim)
            { 
                animate.SetTrigger("slimeMounted");
                mountAnim = true;

                if(this.GetComponentInChildren<SlimeTurnAround>().getTurned() && !player.GetComponent<Player>().getFacingRight())
                {
                    this.GetComponentInChildren<SlimeTurnAround>().faceLeft();
                }
                else if(!this.GetComponentInChildren<SlimeTurnAround>().getTurned() && player.GetComponent<Player>().getFacingRight())
                {
                    this.GetComponentInChildren<SlimeTurnAround>().faceRight();
                }
            }
            if (Input.GetKey("left"))
            {
                player.GetComponent<Player>().faceLeft();
                this.GetComponentInChildren<SlimeTurnAround>().faceLeft();
            }
            else if (Input.GetKey("right"))
            {
                player.GetComponent<Player>().faceRight();
                this.GetComponentInChildren<SlimeTurnAround>().faceRight();
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
            if (Input.GetKeyDown(KeyCode.X))
            {
                GetComponentInChildren<SlimeDetectPlayer>().Attack();
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

            if (this.transform.position.y < initialY)
            {
                Respawn();
            }
            
            //print(this.gameObject.name + " Respawn Point: (" + initialX + "," + initialY + ")");
            
            
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
        playerBody.velocity = new Vector2(playerBody.velocity.x, 75f);
        player.GetComponent<Player>().setLaunched(true);
        //playerBody.velocity += Vector2.up * Physics2D.gravity * (250 - 1) * Time.deltaTime;
    }

    public void slimeLoseHealth()
    {
        slimeHealth -= 1;
        if (slimeHealth <= 0)
        {
            print(this.gameObject.name + " is hurt");
            slimeHealth = 1;
            Respawn();
        }
    }

    public void Respawn()
    {
        animate.SetTrigger("Despawn");
        Invoke("MoveToInitialPosition", 1f);
    }

    public void MoveToInitialPosition()
    {
        transform.position = new Vector3(initialX, initialY, 0);
    }

    public int getSlimeHealth()
    {
        return slimeHealth;
    }

    public void setIsAttacking(bool a)
    {
        isAttacking = a;
    }

    public bool getIsAttacking()
    {
        return isAttacking;
    }
}
