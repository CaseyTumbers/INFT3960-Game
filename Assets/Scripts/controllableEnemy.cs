using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controllableEnemy : MonoBehaviour
{
    protected bool isControlled = false;
    protected GameObject player; 
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
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
