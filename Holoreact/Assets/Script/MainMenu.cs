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
    private Button playButton;

    private int lvl;
    
    GameObject highlightedButton;

    // Start is called before the first frame update
    void Start()
    {
        highlightButton();
    }

    // Update is called once per frame
    void Update()
    {

        highlightedButton = EventSystem.current.currentSelectedGameObject;
        if(Input.GetKeyDown(KeyCode.Return))
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
                    Application.Quit();
                    break;
            }
        }
        else if(Input.GetKeyDown(KeyCode.Escape))
        {
            //SCENE_MANAGER.Quit();
        }
    }

    private void highlightButton()
    {
        playButton.Select();
    }

}
