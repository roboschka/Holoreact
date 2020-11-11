using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// this script is used to manage MainMenu scene
/// </summary>

public class MainMenu : MonoBehaviour
{

    [SerializeField]
    private Button playButton, yesQuitButton;
    [SerializeField]
    private Canvas quitNotificationCanvas, mainMenuCanvas;

    private int lvl;
    
    GameObject highlightedButton, highlightedButton2;

    // Start is called before the first frame update
    void Start()
    {
        quitNotificationCanvas.gameObject.SetActive(false);
        highlightButton(playButton);
    }

    // Update is called once per frame
    void Update()
    {
        highlightedButton = EventSystem.current.currentSelectedGameObject;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            //MainMenu Navigation
            switch(highlightedButton.name)
            {
                case "Play":
                    break;
                case "Settings":
                    break;
                case "Credits":
                    break;
                case "Quit":
                    //Open Notification Canvas
                    toggleNotification(true, false, yesQuitButton);
                    break;
            }
        }
        else if(Input.GetKeyDown(KeyCode.Escape))
        {
            //SCENE_MANAGER.Quit();
        }


        if (quitNotificationCanvas.isActiveAndEnabled)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                switch (highlightedButton.name)
                {
                    case "YesQuit":
                        print("Game Quits");
                        Application.Quit();
                        break;
                    case "NoQuit":
                        toggleNotification(false, true, playButton);
                        break;
                }
            }
            
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                toggleNotification(false, true, playButton);
            }
        }
    }

    private void highlightButton(Button toBeHighlighted)
    {
        toBeHighlighted.Select();
    }

    private void toggleNotification(bool isNotifOn, bool isMainMenuOn, Button toHighlight)
    {
        quitNotificationCanvas.gameObject.SetActive(isNotifOn);
        mainMenuCanvas.gameObject.SetActive(isMainMenuOn);
        highlightButton(toHighlight);
    }
}
