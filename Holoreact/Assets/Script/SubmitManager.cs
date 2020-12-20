using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SubmitManager : MonoBehaviour
{
    [SerializeField]
    private GameObject cameraForSubmitNotif, gameManager, submitPanel;

    [SerializeField]
    private Button yesSubmit;

    private bool paused;
    private GameObject highlightedButton;

    private void Start()
    {
        paused = true;
    }

    // Update is called once per frame
    private void Update()
    {
        
        if (!paused)
        {
            highlightedButton = EventSystem.current.currentSelectedGameObject;
            if (highlightedButton == null)
            {
                yesSubmit.Select();
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                switch (highlightedButton.name)
                {
                    case "Yes":
                        paused = true;
                        submitPanel.SetActive(false);
                        gameManager.GetComponent<GameManager>().FinishExperiment();
                        break;
                    case "No":
                        paused = true;
                        cameraForSubmitNotif.SetActive(false);
                        submitPanel.SetActive(false);
                        gameManager.GetComponent<GameManager>().UnPause();
                        break;
                }
            }
            //else if (Input.GetKeyDown(KeyCode.Escape))
            //{
            //    paused = true;
            //    cameraForSubmitNotif.SetActive(false);
            //    submitPanel.SetActive(false);
            //    gameManager.GetComponent<GameManager>().UnPause();
            //}
        }
    }

    public void ShowSubmit()
    {
        Debug.Log("Show Submit notif");
        paused = false;
        cameraForSubmitNotif.SetActive(true);
        yesSubmit.Select();
    }
    
}
