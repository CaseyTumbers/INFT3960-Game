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
    float coolDownTime;
    GameObject control;
    //private controllableEnemy control;

    // Update is called once per frame
    void Update()
    {
        Collider2D collider = Physics2D.OverlapCircle(isEnemyChecker.position, checkEnemyRadius, enemyLayer);


        if (Input.GetKey(KeyCode.Z) && collider != null && !coolDown)
        {
            //print("MOUNTING");
            playerMovement.setControllingCreature(true);
            animator.SetBool("isMounted", true);
        }

        
        //print(playerMovement.getControllingCreature());
        if (playerMovement.getControllingCreature())
        {
            control = collider.gameObject;
            //print(control);
            transform.position = new Vector3(control.transform.position.x, control.transform.position.y+3, control.transform.position.x);
            control.GetComponent<controllableEnemy>().setIsControlled(true);
            control.GetComponent<controllableEnemy>().setPlayer(gameObject);
            //playerMovement.setControllingCreature(true);
            //transform.position = control.transform.position;
        }
        else if (coolDown)
        {
            coolDownTime -= Time.deltaTime;
        }

        if(coolDownTime < 0)
        {
            coolDown = false;
        }
    }

    public void setCoolDown(bool c)
    {
        coolDown = c;
        coolDownTime = 1f;
    }
}
