using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using TMPro;
using UnityEngine;

public class HandBookManager : MonoBehaviour
{
    private HandBook[] handBookData;

    [SerializeField]
    private TextMeshProUGUI textToShow;

    [SerializeField]
    private GameObject panelForHandbook, gameManager, cameraForHandbook;

    private int index, currentLvl;

    private bool paused;

    void Awake()
    {
        currentLvl = PlayerPrefs.GetInt("currentLevel");
        GetHandbookData();
        paused = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!paused)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Move(-1);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                Move(1);
            }
            else if (Input.GetKey(KeyCode.Escape))
            {
                panelForHandbook.SetActive(false);
                cameraForHandbook.SetActive(false);
                gameManager.GetComponent<GameManager>().UnPause();
            }
        }
    }

    private void Move(int move)
    {
        if(index + move < 1)
        {
            index = handBookData.Count();
        }
        else if(index + move > handBookData.Count() )
        {
            index = 0;
        }
        else
        {
            index += move;
        }
        FindHandBookContent(index);
    }

    private void GetHandbookData()
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("https://api.backendless.com/09476775-387A-4C56-FFE4-B663DC24FC00/DED29ABA-8FAC-4985-86E0-FCCDA5A290B5/data/Handbook?where=levelid%3D"+currentLvl));
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        jsonResponse =  JsonHelper.FixJSon(jsonResponse);

        handBookData = JsonHelper.FromJson<HandBook>(jsonResponse);
    }

    private void FindHandBookContent(int page)
    {
        string handBookContent =
            (
                from content in handBookData
                where content.Page == page
                select content.Description

            ).ToList().FirstOrDefault();

        if (String.IsNullOrEmpty(handBookContent))
        {
            textToShow.text = "something goes wrong please contact the developer";
        }
        else
        {
            textToShow.text = handBookContent;
        }
    }

    public void UnPause()
    {
        Debug.Log("show handbook");
        paused = false;
        panelForHandbook.SetActive(true);
        cameraForHandbook.SetActive(true);
    }

    private void Exit()
    {
        paused = true;
        panelForHandbook.SetActive(false);
        cameraForHandbook.SetActive(false);
        gameManager.GetComponent<GameManager>().UnPause();
    }

}
