using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseManager : MonoBehaviour
{
    [SerializeField]
    private GameObject pausePanel, notifPanel, settingPanel, UICamera, gameManager;
    private GameObject highlightedButton;
    private bool paused, isNotifOn, isSettingsOn;
    [SerializeField]
    private Button backToMenu, yesBack;
    // Start is called before the first frame update
    void Start()
    {
        paused = true;
        isNotifOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        highlightedButton = EventSystem.current.currentSelectedGameObject;
        if (paused)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                gameManager.GetComponent<GameManager>().Pause();
            }
        }
        else
        {
            //UI Navigation ketika notifPanel tidak aktif / pausePanel sedang aktif
            if (!isNotifOn)
            {
                if (!settingPanel.activeInHierarchy)
                {
                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        switch (highlightedButton.name)
                        {
                            case "BackToGame":
                                HidePause();
                                break;
                            case "Settings":
                                ToggleSettings(false, true);
                                break;
                            case "Level":
                                ToggleNotification(false, true, yesBack);
                                break;
                        }
                    }
                    else if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        HidePause();
                    }
                }
                //UINavigation ketika settingsPanel sedang aktif
                else
                {
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        ToggleSettings(true, false);
                        backToMenu.Select();
                    }
                }
            }
            else
            {
                //UINavigation ketika notifPanel sedang aktif
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    switch (highlightedButton.name)
                    {
                        case "YesBack":
                            SceneManager.LoadScene("ChooseLevel");
                            break;
                        case "NoBack":
                            Debug.Log("Show Pause");
                            ToggleNotification(true, false, backToMenu);
                            break;
                    }
                }
            }
        }
    }

    #region Pause
    public void ShowPause()
    {
        paused = false;
        UICamera.SetActive(true);
        pausePanel.SetActive(true);
        backToMenu.Select();
    }

    private void HidePause()
    {
        UICamera.SetActive(false);
        pausePanel.SetActive(false);
        paused = true;
        gameManager.GetComponent<GameManager>().UnPause();
    }
    #endregion

    //private void ShowNotif()
    //{
    //    notifPanel.SetActive(true);
    //    pausePanel.SetActive(false);
    //    yesBack.Select();
    //    isNotifOn = true;
    //}

    
    public void ShowSettings()
    {
        Debug.Log("Show Settings");

    }

    private void ToggleSettings(bool isPauseMenuActive, bool isSettingActive)
    {
        pausePanel.SetActive(isPauseMenuActive);
        settingPanel.SetActive(isSettingActive);
        isSettingsOn = isSettingActive;
    }

    private void ToggleNotification(bool isPauseMenuActive, bool isNotifActive, Button toHighlight)
    {
        pausePanel.SetActive(isPauseMenuActive);
        notifPanel.SetActive(isNotifActive);
        toHighlight.Select();
        isNotifOn = isNotifActive;
    }

    
    
}
