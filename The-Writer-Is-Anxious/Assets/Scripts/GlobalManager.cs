﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalManager : MonoBehaviour
{
    public static GlobalManager instance;
    public SaveData saveData;
    public SaveManager saveManager;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            
            Destroy(gameObject);
        }
        
        DontDestroyOnLoad(gameObject);

    }
    // Start is called before the first frame update
    void Start()
    {
        saveManager.Load(0);
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}