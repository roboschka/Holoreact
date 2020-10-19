using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// this script is used to manage MainMenu scene
/// </summary>

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private Button btnMenu; 

    private int lvl;


    // Start is called before the first frame update
    void Start()
    {
        lvl = 1;
        //Change latter
        HighLight();
    }

    // Update is called once per frame
    void Update()
    {
        //keycode Return mean enter since keycode enter in unity refer to enter in the numpad
        if(Input.GetKeyDown(KeyCode.Return))
        {
            //SCENE_MANAGER.MoveToLevel(lvl);
        }
        else if(Input.GetKeyDown(KeyCode.Escape))
        {
            SCENE_MANAGER.Quit();
        }
    }

    private void HighLight()
    {
        btnMenu.Select();
    }

}
