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
            if (collision.gameObject.name.Contains("rock"))
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
        else if (detectionInt == 1)
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
                //print("Back on track");
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

    void faceRight()
    {
        transform.parent.eulerAngles = new Vector3(0, 180, 0);
    }

    void faceLeft()
    {
        transform.parent.eulerAngles = new Vector3(0, 0, 0);
    }
}
