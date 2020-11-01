using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controllableEnemy : MonoBehaviour
{
    protected bool isControlled = false;
    protected GameObject player; 

    void Awake()
    {
        
    }

    public void setIsControlled(bool value)
    {
        isControlled = value;
    }

    public bool getIsControlled()
    {
        return isControlled;
    }

    public void setPlayer(GameObject p)
    {
        player = p;
    }

    public GameObject getPlayer()
    {
        return player;
    }
}
