﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// O is catRoom. 1 is Kitchen, 2 is entry way.
/// catroom - kitchen - entry way - catroom
/// </summary>
public class Scene2Arrow : MonoBehaviour
{
    public GameObject catRoom;
    public GameObject kitchen;
    public GameObject entryWay;
    public Scene2Manager scene2Manager;
    public int roomIndex = 0;
    public bool right;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnMouseDown()
    {
        if (scene2Manager.otherObjActive == true)
        {
            if (right == true)
            {
                if (roomIndex == 0)
                {
                    roomIndex = 1;
                    catRoom.SetActive(false);
                    kitchen.SetActive(true);
                }
                else if (roomIndex == 1)
                {
                    roomIndex = 2;
                    kitchen.SetActive(false);
                    entryWay.SetActive(true);
                }
                else
                {
                    roomIndex = 0;
                    entryWay.SetActive(false);
                    catRoom.SetActive(true);

                }
            }
            else
            {
                if (roomIndex == 0)
                {
                    roomIndex = 2;
                    catRoom.SetActive(false);
                    entryWay.SetActive(true);
                }
                else if (roomIndex == 1)
                {
                    roomIndex = 0;
                    kitchen.SetActive(false);
                    catRoom.SetActive(true);
                }
                else
                {
                    roomIndex = 1;
                    entryWay.SetActive(false);
                    catRoom.SetActive(true);
                }
            }
        }
    }
}
