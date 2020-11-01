using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI dialogue;
    private string[] sentences;
    public string[] sentences1;
    public string[] sentences2;
    public string[] sentences3;

    private int index;
    public float speed;

    public GameObject nextText;
    private bool progressText = false;
    private bool finished = false;
    private int num = 0;

    private void Start()
    {
        //StartCoroutine(Type());
        
    }

    private void Update()
    {
        if (sentences != null)
        {
            if (dialogue.text == sentences[index])
            {
                nextText.SetActive(true);
                progressText = true;
            }
            else
            {
                progressText = false;
            }
        }
    }

    public void writeText()
    {
        if (num == 1)
        {
            sentences = sentences1;
            print("number 1");
        }
        else if (num == 2)
        {
            sentences = sentences2;
        }
        else if (num == 3)
        {
            sentences = sentences3;
        }

        index = 0;
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

    public void NextSentence()
    {
        nextText.SetActive(false);

        if (index < sentences.Length - 1)
        {
            index++;
            dialogue.text = "";
            StartCoroutine(Type());
            finished = false;
        }
        else
        {
            dialogue.text = "";
            finished = true;
        }
    }

    public void setNum(int n)
    {
        num = n;
    }

    
    public bool checkProgress()
    {
        return progressText;
    }
    
    public bool checkFinished()
    {
        return finished;
    }
}
