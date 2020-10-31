using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    GameObject temp;
    void OnTriggerEnter2D(Collider2D collision)
    {
        //print("OW");

        if (collision.gameObject.name.Equals("Main Character"))
        {
            //RaycastHit2D hit1 = Physics2D.Raycast(collision.transform.position, Vector2.right, 1000f);
            //RaycastHit2D hit2 = Physics2D.Raycast(collision.transform.position, -Vector2.right, 1000f);
            if (!GetComponentInParent<SlimeMovement>().getIsControlled() && !Input.GetKey(KeyCode.Z))
            {
               
               
                    collision.GetComponent<Player>().TakeDamage();
                    //collision.GetComponent<Rigidbody2D>().velocity = new Vector2(-100f, 50f);
                    //collision.GetComponent<Player>().setLaunched(true);
                    //print("GO AWAY!!!");
                
                //collision.GetComponent<Player>().loseHealth();
            }
            //print(hit1.collider);
            /*if (hit1.collider == null && hit2.collider == null)
            {
                collision.GetComponent<Rigidbody2D>().AddForce(transform.up * 5000 + transform.right * -15000);
            }
            else
            {
                print("GOING UP");
                collision.GetComponent<Rigidbody2D>().AddForce(transform.up * 5000);
            }*/
        }
        else if (collision.gameObject.name.Contains("Slime") && !collision.gameObject.Equals(this.transform.parent.gameObject))
        {
           
                collision.GetComponent<SlimeMovement>().slimeLoseHealth();
            
        }
        else if (collision.gameObject.name.Contains("Block"))
        {
            temp = collision.gameObject;
            //temp.GetComponent<Breakable>().Break();
            print("DESTROYED");
        }
    }
}
