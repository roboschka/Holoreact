﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SubmitManager : MonoBehaviour
{
    [SerializeField]
    private GameObject cameraForSubmitNotif, gameManager;

    [SerializeField]
    private Button yesSubmit;

    private bool paused;
    private GameObject highlightedButton;

    private void Awake()
    {
        paused = true;
    }

    // Update is called once per frame
    private void Update()
    {
        highlightedButton = EventSystem.current.currentSelectedGameObject;
        if (!paused)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                switch (highlightedButton.name)
                {
                    case "Yes":
                        gameManager.GetComponent<GameManager>().FinishExperiment();
                        break;
                    case "No":
                        cameraForSubmitNotif.SetActive(false);
                        gameManager.GetComponent<GameManager>().UnPause();
                        break;
                }
            } 
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                cameraForSubmitNotif.SetActive(false);
                gameManager.GetComponent<GameManager>().UnPause();
            }
        }
    }

    public void UnPause()
    {
        Debug.Log("Show Submit notif");
        paused = false;
        cameraForSubmitNotif.SetActive(true);
        yesSubmit.Select();
    }
    
}
