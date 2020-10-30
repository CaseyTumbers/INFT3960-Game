using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI dialogue;
    public string[] sentences;
    private int index;
    public float speed;

    private void Start()
    {
        //StartCoroutine(Type());
        
    }

    public void writeText()
    {
        StartCoroutine(Type());
    }

    IEnumerator Type()
    {
        foreach(char letter in sentences[index].ToCharArray())
        {
            dialogue.text += letter;
            yield return new WaitForSeconds(speed);
        }
    }
}
