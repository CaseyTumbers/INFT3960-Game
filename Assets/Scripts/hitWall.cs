using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitWall : MonoBehaviour
{
    public Player player;
 
    void OnTriggerEnter2D(Collider2D collision)
    {
        //print("OW");

        if (collision.gameObject.layer.Equals(8))
        {
            //print("OW");
            player.stop = true;

        }
        else
        {
            player.stop = false;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        player.stop = false;
    }
}
