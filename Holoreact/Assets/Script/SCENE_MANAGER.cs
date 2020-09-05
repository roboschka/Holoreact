using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This script is used to manage scene.
/// eg : nextscene() is used to move to next scene.
/// </summary>

public static class SCENE_MANAGER
{
    private static int index = 0;
    
    public static void NextScene()
    {
        index++;
        MoveScene();
    }

    public static void PrevScene()
    {
        index--;
        MoveScene();
    }

    public static void BackToMenu()
    {
        index = 0;
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
