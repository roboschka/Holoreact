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
    [SerializeField]
    private AudioSource source;
    [SerializeField]
    private AudioClip selectAudio, notificationAudio, pressAudio;
    // Start is called before the first frame update
    void Start()
    {
        paused = true;
        isNotifOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!paused)
        {
            highlightedButton = EventSystem.current.currentSelectedGameObject;
            //UI Navigation ketika notifPanel tidak aktif / pausePanel sedang aktif
            if (!isNotifOn)
            {
                if (highlightedButton == null)
                {
                    backToMenu.Select();
                }

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
                                source.PlayOneShot(notificationAudio, 0.5f);
                                ToggleNotification(false, true, yesBack);
                                break;
                        }
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
                if (highlightedButton == null)
                {
                    yesBack.Select();
                }
                //UINavigation ketika notifPanel sedang aktif
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    switch (highlightedButton.name)
                    {
                        case "YesBack":
                            StartCoroutine(DelayedSceneLoad(pressAudio));
                            break;
                        case "NoBack":
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
        paused = true;
        UICamera.SetActive(false);
        pausePanel.SetActive(false);
        gameManager.GetComponent<GameManager>().UnPause();
    }
    #endregion
    
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

    private IEnumerator DelayedSceneLoad(AudioClip audioToBePlayed)
    {
        source.PlayOneShot(audioToBePlayed, 0.6f);
        yield return new WaitForSeconds(audioToBePlayed.length);
        SceneManager.LoadScene("ChooseLevel");
    }
}
