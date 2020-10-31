using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockDoor : MonoBehaviour
{
    public static int numKeys = 2;

    public void Update()
    {
        if(numKeys == 0)
        {
            Destroy(this.gameObject);
        }
    }
}
