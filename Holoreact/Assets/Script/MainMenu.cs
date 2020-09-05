using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// this script is used to manage MainMenu scene
/// </summary>

public class MainMenu : MonoBehaviour
{   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //keycode Return mean enter since keycode enter in unity refer to enter in the numpad
        if(Input.GetKeyDown(KeyCode.Return))
        {
            SCENE_MANAGER.NextScene();
        }
        else if(Input.GetKeyDown(KeyCode.Escape))
        {
            SCENE_MANAGER.Quit();
        }
    }
}
