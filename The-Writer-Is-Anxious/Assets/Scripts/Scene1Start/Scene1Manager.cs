﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;


public class Scene1Manager : MonoBehaviour
{
    public Text dialogueText;
    public GameObject dialogueObj;
    bool trigger = true;
    /// <summary>
    /// When trigger is false, use resetTrigger to make trigger to true (by using GetMouseButtonUp)
    /// </summary>
    public bool resetTrigger = false;
    int dialogueType = 0; //Default is 0, at dialogueText. 1 is at Idialogue. 2 is a t editorDialogue

    List<string> dialogueList;
    public GameObject objTable;
    public GameObject objTableDialogue;
    public GameObject objTableRing;
    //Scene of memory
    public GameObject mainCamera;
    public GameObject objMemory;
    public GameObject objIDialogue;
    public GameObject objEditorDialogue;
    public Text IDialogue;
    public Text editorDialogue;
    public int step = 0;
    public Vector3 startCameraPosition;
    public Vector3 memoryCameraPosition;
    private float fraction;
    public float speed;
    private bool cameraMoveToMemory = false;
    private TextAsset textAsset;
    //Sad
    public GameObject triggerObj;

    public Text textComp;
    
    //public Canvas canvas;


    // Start is called before the first frame update
    void Start()
    {
        dialogueList = new List<string>();
        textAsset = Resources.Load("Scene1Start") as TextAsset;
        ReadTextFile();
        
     }   

    // Update is called once per frame
    void Update()
    {
        if (trigger == true && Input.GetMouseButtonDown(0) )
        {
            print("1111");
            NextStep();
        }
        if (resetTrigger == true && Input.GetMouseButtonUp(0))
        {
            trigger = true;
            resetTrigger = false;
        }
        //Begin memory
        if (cameraMoveToMemory == true)
        {
            if (fraction < 1)
            {
                fraction += Time.deltaTime * speed;
                mainCamera.transform.position = Vector3.Lerp(startCameraPosition, memoryCameraPosition, fraction);
            }
            else
            {
                cameraMoveToMemory = false;
                resetTrigger = true;
                trigger = true;
                NextStep();
            }
        }
       
    }
    void ReadTextFile()
    {
        dialogueList = textAsset.text.Split('\n').ToList();
       
    }
    public void NextStep()
    {
        print(dialogueList[step]);
        if (dialogueList[step][0] == '@')
        {
            ClickAction();
        }
        else if (dialogueList[step][0] == '(')
        {
            StepAction();

        }
        else
        {
            if (dialogueType == 0)
            {
                dialogueText.text = dialogueList[step];
                step++;
               
            }
            else if (dialogueType == 1)
            {
                IDialogue.text = dialogueList[step];
                step++;
            }
            else if (dialogueType == 2)
            {
                editorDialogue.text = dialogueList[step];
                step++;
            }

        }
    }
   
    void StepAction()
    {
        
        if (dialogueList[step].Contains("(I_table.png)"))
        {
            objTable.SetActive(true);
            step++;
            NextStep();
        }
        else if (dialogueList[step].Contains("(I_table_dialogue.png)"))
        {
            objTableDialogue.SetActive(true);
            step++;
            NextStep();
        }
        else if (dialogueList[step].Contains("(dialogue fades out)"))
        {
            objTableDialogue.SetActive(false);
            objTableRing.SetActive(true);
            step++;
            trigger = false;
        }
        else if (dialogueList[step].Contains("(division_line)"))
        {
            objMemory.SetActive(true);
            objTableRing.SetActive(false);
            dialogueObj.SetActive(false);
            cameraMoveToMemory = true;
            step++;

        }
        else if (dialogueList[step].Contains("(IDialogueStart)"))
        {
            objIDialogue.SetActive(true);
            dialogueType = 1;
            step++;
            NextStep();

        }
        else if (dialogueList[step].Contains("(IDialogueEnd)"))
        {
            objIDialogue.SetActive(false);
            dialogueType = 0;
            step++;
            NextStep();

        }
        else if (dialogueList[step].Contains("(EditorDialogueStart)"))
        {
            objEditorDialogue.SetActive(true);
            dialogueType = 2;
            step++;
            NextStep();

        }
        else if (dialogueList[step].Contains("(EditorDialogueEnd)"))
        {
            objEditorDialogue.SetActive(false);
            dialogueType = 0;
            step++;
            NextStep();

        }



    }

    void ClickAction()
    {
        string word = dialogueList[step];
        word = word.Substring(1, word.Length - 2);
        print(word);
        print(word.Length);
        step++;
        NextStep();
        trigger = false;
        DrawTrigger(word);
        print("hhhhh");
    }
    public Text GetSuitableText()
    {
        if (dialogueType == 0)
        {
            return dialogueText;
            

        }
        else if (dialogueType == 1)
        {
            return IDialogue;
        }
        else if (dialogueType == 2)
        {
            return editorDialogue;
            
        }
        else
        {
            return null;
        }
    }
    void DrawTrigger(string word)
    {
        int startIndex = dialogueList[step-1].IndexOf(word);
        int endIndex = startIndex+ word.Length;
        print(startIndex);
        
        Vector3 startPosition = PrintPos(startIndex);
        Vector3 endPosition = PrintPos(endIndex);
        new GameObject("point").transform.position = startPosition;
        new GameObject("poin2").transform.position = endPosition;
        float distance = endPosition.x - startPosition.x;
        triggerObj.SetActive(true);
        triggerObj.transform.position = new Vector3(startPosition.x, startPosition.y + distance, 0);
        triggerObj.transform.localScale = new Vector3(distance, distance, 0);
        
    }

    Vector3 PrintPos(int charIndex)
    {
        string text = GetSuitableText().text;

        TextGenerator textGen = new TextGenerator(text.Length);
        Vector2 extents = GetSuitableText().gameObject.GetComponent<RectTransform>().rect.size;
        textGen.Populate(text, GetSuitableText().GetGenerationSettings(extents));

        int newLine = text.Substring(0, charIndex).Split('\n').Length - 1;
        int whiteSpace = text.Substring(0, charIndex).Split(' ').Length - 1;
        int indexOfTextQuad = (charIndex * 4) + (newLine * 4) - 4;
        if (indexOfTextQuad < textGen.vertexCount)
        {
            Vector3 avgPos = (textGen.verts[indexOfTextQuad].position +
                textGen.verts[indexOfTextQuad + 1].position +
                textGen.verts[indexOfTextQuad + 2].position +
                textGen.verts[indexOfTextQuad + 3].position) / 4f;

            //print(avgPos);
            //PrintWorldPos(avgPos);
            return GetSuitableText().transform.TransformPoint(avgPos);
        }
        else
        {
            Debug.LogError("Out of text bound");
            return new Vector3(0,0,0);
        }
    }

    void PrintWorldPos(Vector3 testPoint)
    {
        Vector3 worldPos = textComp.transform.TransformPoint(testPoint);
        print(worldPos);
        new GameObject("point").transform.position = worldPos;
        Debug.DrawRay(worldPos, Vector3.up, Color.red, 50f);
    }

   
}
