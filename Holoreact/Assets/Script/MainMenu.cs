using System.Collections;
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
    private GameObject quitNotificationCanvas, mainMenuCanvas, studentDataCanvas, settingsCanvas, creditsCanvas, tutorialCanvas;

    [SerializeField]
    private AudioSource source;

    [SerializeField]
    private AudioClip highlight, select, notification;

    private int lvl;
    
    private bool isFirstPlay;

    private GameObject highlightedButton;

    // Start is called before the first frame update
    private void Start()
    {
        isFirstPlay = true;

        isFirstPlay = PlayerPrefs.GetInt("isFirstPlay") == 1 ? true : false;

        if (isFirstPlay)
        {
            studentDataCanvas.gameObject.SetActive(true);
            mainMenuCanvas.gameObject.SetActive(false);
        }
        else
        {
            studentDataCanvas.gameObject.SetActive(false);
            mainMenuCanvas.gameObject.SetActive(true);
            playButton.Select();
        }
    }

    // Update is called once per frames
    void Update()
    {
        highlightedButton = EventSystem.current.currentSelectedGameObject;
        
        if (mainMenuCanvas.activeInHierarchy)
        {
            if (!quitNotificationCanvas.activeInHierarchy && highlightedButton == null)
            {
                playButton.Select();
            }
            
            if (Input.GetKeyDown(KeyCode.Return))
            {
                switch (highlightedButton.name)
                {
                    case "Play":
                        StartCoroutine(DelayedSceneLoad(select));
                        break;
                    case "HowToPlay":
                        ToggleCanvas(tutorialCanvas, mainMenuCanvas);
                        source.PlayOneShot(select);
                        break;
                    case "Settings":
                        ToggleCanvas(settingsCanvas, mainMenuCanvas);
                        source.PlayOneShot(select);
                        break;
                    case "Credits":
                        ToggleCanvas(creditsCanvas, mainMenuCanvas);
                        source.PlayOneShot(select);
                        break;
                    case "Quit":
                        //Open Notification Canvas
                        ToggleNotification(true, false, yesQuitButton);
                        source.PlayOneShot(notification);
                        break;
                }
            }
        }

        if (quitNotificationCanvas.activeInHierarchy)
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
                        StartCoroutine(DelayedQuit());
                        break;
                    case "NoQuit":
                        ToggleNotification(false, true, playButton);
                        source.PlayOneShot(select);
                        break;
                }
            }

            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                ToggleNotification(false, true, playButton);
            }
        }

        if (settingsCanvas.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ToggleCanvas(mainMenuCanvas, settingsCanvas);
                playButton.Select();
            }
        }

        if (creditsCanvas.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ToggleCanvas(mainMenuCanvas, creditsCanvas);
                playButton.Select();
            }
        }

        if (tutorialCanvas.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ToggleCanvas(mainMenuCanvas, tutorialCanvas);
                playButton.Select();
            }
        }
    }

    private void ToggleCanvas (GameObject activeCanvas, GameObject inactiveCanvas)
    {
        activeCanvas.SetActive(true);
        inactiveCanvas.SetActive(false);
    }
  
    public void ToggleNotification(bool isNotifOn, bool isMainMenuOn, Button toHighlight)
    {
        quitNotificationCanvas.SetActive(isNotifOn);
        mainMenuCanvas.SetActive(isMainMenuOn);
        toHighlight.Select();
    }

    public bool GetFirstPlay()
    {
        return isFirstPlay;
    }

    public void SetFirstPlay(bool value)
    {
        isFirstPlay = value;
    }

    #region Main Menu SFX
    public void PlayAudioForButtonHighlight()
    {
        source.PlayOneShot(highlight);
    }
    
    private IEnumerator DelayedSceneLoad(AudioClip audioToBePlayed)
    {
        source.PlayOneShot(audioToBePlayed);
        yield return new WaitForSeconds(audioToBePlayed.length);
        SceneManager.LoadScene("ChooseLevel");
    }

    private IEnumerator DelayedQuit()
    {
        source.PlayOneShot(select);
        yield return new WaitForSeconds(select.length);
        Application.Quit();
    }
    #endregion
}
