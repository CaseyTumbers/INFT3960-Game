using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideWithEnemy : Player
{
    //TODO - Make player transform with slime.
    public Transform isEnemyChecker;
    public float checkEnemyRadius;
    public LayerMask enemyLayer;
    public Player playerMovement;
    public bool coolDown = false;
    bool eagleControlled = false;
    float coolDownTime;
    GameObject controlledCreature;
    //private controllableEnemy control;

    // Update is called once per frame
    void Update()
    {
        Collider2D collider = Physics2D.OverlapCircle(isEnemyChecker.position, checkEnemyRadius, enemyLayer);


        if (Input.GetKey(KeyCode.Z) && collider != null && !coolDown && !eagleControlled)
        {
            //print("MOUNTING");
            playerMovement.setControllingCreature(true);
        }

        if (!eagleControlled)
        {
            //print(playerMovement.getControllingCreature());
            if (playerMovement.getControllingCreature())
            {
                controlledCreature = collider.gameObject;
                //print(control);
                transform.position = new Vector3(controlledCreature.transform.position.x, controlledCreature.transform.position.y + 3, controlledCreature.transform.position.x);
                controlledCreature.GetComponent<controllableEnemy>().setIsControlled(true);
                controlledCreature.GetComponent<controllableEnemy>().setPlayer(gameObject);
                //playerMovement.setControllingCreature(true);
                //transform.position = control.transform.position;
            }
            else if (coolDown)
            {
                coolDownTime -= Time.deltaTime;
            }
        }
        else
        {
            controlledCreature.GetComponent<controllableEnemy>().setIsControlled(true);
            controlledCreature.GetComponent<controllableEnemy>().setPlayer(gameObject);
            eagleControlled = true;
        }

        if(coolDownTime < 0)
        {
            coolDown = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collider)
    {

        if (collider.gameObject.layer.Equals(10) && Input.GetKey(KeyCode.Z) && !coolDown)
        {
            controlledCreature = collider.gameObject;
            //print(control);
            transform.position = new Vector3(controlledCreature.transform.position.x, controlledCreature.transform.position.y - 1, controlledCreature.transform.position.x);
            controlledCreature.GetComponent<controllableEnemy>().setIsControlled(true);
            controlledCreature.GetComponent<controllableEnemy>().setPlayer(gameObject);
            eagleControlled = true;
            //playerMovement.setControllingCreature(true);
        }
    }

    public void setCoolDown(bool c)
    {
        coolDown = c;
        coolDownTime = 1f;
    }
}
