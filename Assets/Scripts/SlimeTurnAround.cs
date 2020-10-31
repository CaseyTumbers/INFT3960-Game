using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeTurnAround : MonoBehaviour
{
    public GameObject detectAreaLeft;
    public int detectionInt = 0;
    private bool turned = false;
    private bool wait = false;
    void OnTriggerStay2D(Collider2D collision) {
        if (detectionInt == 0)
        {
            if (collision.gameObject.tag.Contains("rock"))
            {
                if (!GetComponentInParent<SlimeMovement>().getIsAttacking())
                {
                    detectAreaLeft.GetComponent<BoxCollider2D>().enabled = false;
                    //print("HIT ROCK");
                    if (turned)
                    {
                        faceLeft();
                        turned = false;
                        GetComponentInParent<SlimeMovement>().direction = -1f;
                    }
                    else if (!turned)
                    {
                        //print("TURNED AROUND");
                        faceRight();
                        turned = true;
                        GetComponentInParent<SlimeMovement>().direction = 1f;
                    }
                    detectAreaLeft.GetComponent<BoxCollider2D>().enabled = true;
                }
            }
        }
        /*else if (detectionInt == 1)
        {
            //print(collision.gameObject.name);
            if (!collision.IsTouchingLayers(9) && !wait)
            {
                if (!GetComponentInParent<SlimeMovement>().getIsAttacking())
                {
                    wait = true;
                    print("AHHH NO EDGE");
                    detectAreaLeft.GetComponent<BoxCollider2D>().enabled = false;
                    this.GetComponent<BoxCollider2D>().enabled = false;
                    //print("HIT ROCK");
                    if (turned)
                    {
                        print("Turning Left");
                        faceLeft();
                        turned = false;
                        GetComponentInParent<SlimeMovement>().direction = -1f;
                    }
                    else if (!turned)
                    {
                        print("Turning Right");
                        faceRight();
                        turned = true;
                        GetComponentInParent<SlimeMovement>().direction = 1f;
                    }
                    detectAreaLeft.GetComponent<BoxCollider2D>().enabled = true;
                    this.GetComponent<BoxCollider2D>().enabled = true;
                    wait = false;
                }
            }
            else
            {
                //print("Back on track");
            }
        }*/

    }

    private void FixedUpdate()
    {
        if (detectionInt == 1 && !GetComponentInParent<SlimeMovement>().getIsControlled())
        {
            // Bit shift the index of the layer (8) to get a bit mask
            int layerMask = 1 << 8;
       
            // Does the ray intersect any objects excluding the player layer
            if (Physics2D.Raycast(transform.position, transform.TransformDirection(Vector3.down), 1, layerMask))
            {
                //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * 1, Color.yellow);
                //Debug.Log("Did Hit");
            }
            else
            {
                //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * 10, Color.white);
                //Debug.Log("Did not Hit");
                if (!GetComponentInParent<SlimeMovement>().getIsAttacking())
                {
                    wait = true;
                    //print("AHHH NO EDGE");
                    detectAreaLeft.GetComponent<BoxCollider2D>().enabled = false;
                    this.GetComponent<BoxCollider2D>().enabled = false;
                    //print("HIT ROCK");
                    if (turned)
                    {
                        //print("Turning Left");
                        faceLeft();
                        //turned = false;
                        
                    }
                    else if (!turned)
                    {
                        //print("Turning Right");
                        faceRight();
                        //turned = true;
                  
                    }
                    detectAreaLeft.GetComponent<BoxCollider2D>().enabled = true;
                    this.GetComponent<BoxCollider2D>().enabled = true;
                    wait = false;
                }
            }
        }
    }

    /*void OnCollisionExit(Collision collision)
    {
        if (detectionInt == 1)
        {
            //print(collision.gameObject.name);
            if (collision.gameObject.layer != 8 && !wait)
            {
                if (!GetComponentInParent<SlimeMovement>().getIsAttacking())
                {
                    wait = true;
                    print("AHHH NO EDGE");
                    detectAreaLeft.GetComponent<BoxCollider2D>().enabled = false;
                    this.GetComponent<BoxCollider2D>().enabled = false;
                    //print("HIT ROCK");
                    if (turned)
                    {
                        faceLeft();
                        turned = false;
                        GetComponentInParent<SlimeMovement>().direction = -1f;
                    }
                    else if (!turned)
                    {
                        //print("TURNED AROUND");
                        faceRight();
                        turned = true;
                        GetComponentInParent<SlimeMovement>().direction = 1f;
                    }
                    detectAreaLeft.GetComponent<BoxCollider2D>().enabled = true;
                    this.GetComponent<BoxCollider2D>().enabled = true;
                    wait = false;
                }
            }
            else
            {
                print("Back on track");
            }
        }

    }*/

    public void faceRight()
    {
        transform.parent.eulerAngles = new Vector3(0, 180, 0);
        turned = true;
        GetComponentInParent<SlimeMovement>().direction = 1f;
    }

    public void faceLeft()
    {
        transform.parent.eulerAngles = new Vector3(0, 0, 0);
        turned = false;
        GetComponentInParent<SlimeMovement>().direction = -1f;
    }

    public bool getTurned()
    {
        return turned;
    }
}
