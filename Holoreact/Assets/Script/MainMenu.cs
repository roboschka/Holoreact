﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

/// <summary>
/// this script is used to manage MainMenu scene
/// </summary>

public class MainMenu : MonoBehaviour
{

    public Button playButton;

    [SerializeField]
    private Button yesQuitButton;

    [SerializeField]
    private Canvas quitNotificationCanvas, mainMenuCanvas, studentDataCanvas;

    private int lvl;
    
    public bool isFirstPlay = true;

    private GameObject highlightedButton;

    // Start is called before the first frame update
    private void Start()
    {
        isFirstPlay = PlayerPrefs.GetInt("isFirstPlay") == 1 ? true : false;
        Debug.Log("isFirstPlay = " + isFirstPlay);

        if (isFirstPlay)
        {
            studentDataCanvas.gameObject.SetActive(true);
            mainMenuCanvas.gameObject.SetActive(false);
        }
        else
        {
            studentDataCanvas.gameObject.SetActive(false);
            mainMenuCanvas.gameObject.SetActive(true);
            ToggleNotification(false, true, playButton);
        }
    }

    // Update is called once per frames
    void Update()
    {
        highlightedButton = EventSystem.current.currentSelectedGameObject;

        if (!quitNotificationCanvas.isActiveAndEnabled && highlightedButton == null)
        {
            playButton.Select();
        } 

        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log(highlightedButton.name);
            switch (highlightedButton.name)
            {
                case "Play":
                    SceneManager.LoadScene("ChooseLevel");
                    break;
                case "Settings":
                    break;
                case "Credits":
                    break;
                case "Quit":
                    //Open Notification Canvas
                    Debug.Log("Open Quit");
                    ToggleNotification(true, false, yesQuitButton);
                    break;
            }
        }

        if (quitNotificationCanvas.isActiveAndEnabled)
        {
            if (highlightedButton == null)
            {
                yesQuitButton.Select();
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                switch (highlightedButton.name)
                {
                    case "YesQuit":
                        print("Game Quits");
                        Application.Quit();
                        break;
                    case "NoQuit":
                        ToggleNotification(false, true, playButton);
                        break;
                }
            }

            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                ToggleNotification(false, true, playButton);
            }
        }
    }

    public void ToggleNotification(bool isNotifOn, bool isMainMenuOn, Button toHighlight)
    {
        quitNotificationCanvas.gameObject.SetActive(isNotifOn);
        mainMenuCanvas.gameObject.SetActive(isMainMenuOn);
        toHighlight.Select();
    }
}
