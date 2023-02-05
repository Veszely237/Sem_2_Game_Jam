using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Threading.Tasks;
using System.Xml.Xsl;

public class Dialogue : MonoBehaviour
{
    public SpeechObject[] sequence1;
    public TextMeshProUGUI identityHeader;
    public GameObject button;
    private bool compilied;

    //public TextMeshProUGUI[] sequence; 

    public float textSpeed; 
    private int Line_index;
    private int sequence_index;


    // Start is called before the first frame update
    void Start()
    {
        
        button.SetActive(false);
        sequence1[sequence_index].textComponent.text = string.Empty;
        identityHeader.text = sequence1[sequence_index].speaker;
        startDialogue();

       
    }
    
    // Update is called once per frame
    async void Update()
    {
        allowInput();
       if(compilied) { 

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if(sequence1[sequence_index].textComponent.text == sequence1[sequence_index].lines[Line_index])
            {
                nextLine();
            }
            else 
            {
                // stuttering issue cause likely around here ( start () )

                sequence1[sequence_index].textComponent.text = sequence1[sequence_index].lines[Line_index];
                StopAllCoroutines();
                Start();
                

            }
           await Task.Delay(8);
        }

        // checks if the arrary of SpeechObjects has been cycled through,
        //if that is the case the UI panel is disabled and button to change scene activated

        if (sequenceOver())
        {
            button.SetActive(true);
            gameObject.SetActive(false);
            
        }

       }
    }
    void startDialogue()
    {
        Line_index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach(char c in sequence1[sequence_index].lines[Line_index].ToCharArray())
        {
            sequence1[sequence_index].textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        
    }

   void nextLine()
    {
        if(Line_index < sequence1[sequence_index].lines.Length - 1)
        {
            Line_index++;
            sequence1[sequence_index].textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            sequence1[sequence_index].textComponent.text = string.Empty;
            sequence_index++;
            Line_index = 0;
        }
    }

    bool sequenceOver()
    {
        if(sequence1.Length != sequence_index)
        {
            return false;
        }
        else
        {
            return true;
           
        }
    }

    void allowInput()
    {
        if (sequence1[sequence_index].lines[Line_index] != sequence1[sequence_index].textComponent.text)
        {
            compilied= false;
        }
        else
        {
            compilied= true;
        }
    }
    //bool 

   
}
