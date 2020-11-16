using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This script is used to manage scene.
/// functionList :
/// BackToMenu() is used to back to first menu.
/// MoveToLevel(lvl) is used to move level  with parameter the lvl index.
/// </summary>

public static class SCENE_MANAGER
{
    private static int index = 0;
    
    //commented because probrably not used.
    //public static void NextScene()
    //{
    //    index++;
    //    MoveScene();
    //}

    //public static void PrevScene()
    //{
    //    index--;
    //    MoveScene();
    //}

    public static void BackToMenu()
    {
        index = 0;
        MoveScene();
    }

    public static void MoveToLevel(int lvl)
    {
        index = lvl;
        MoveScene();
    }

    public static void Quit()
    {
        Application.Quit();
    }

    private static void MoveScene()
    {
        SceneManager.LoadScene(index);
    }

}
