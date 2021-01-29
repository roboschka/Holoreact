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
    private TextMeshProUGUI textToShow, pageText;

    [SerializeField]
    private GameObject panelForHandbook, gameManager, UICamera;

    [SerializeField]
    private AudioSource source;

    [SerializeField]
    private AudioClip pageTurn;

    private int currentPage, currentLvl;

    public bool paused;

    private void Awake()
    {
        currentLvl = PlayerPrefs.GetInt("currentLevel");
        GetHandbookData();
        paused = true;
        currentPage = 1;
        pageText.text = currentPage.ToString() + "/" + handBookData.Count().ToString();
    }

    // Update is called once per frame
    private void Update()
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
                Exit();
            }
        }
    }

    private void Move(int move)
    {
        source.PlayOneShot(pageTurn, 0.4f);
        if(currentPage + move < 1)
        {
            currentPage = handBookData.Count();
        }
        else if(currentPage + move > handBookData.Count() )
        {
            currentPage = 1;
        }
        else
        {
            currentPage += move;
        }
        FindHandBookContent(currentPage);
    }

    private void GetHandbookData()
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("https://api.backendless.com/09476775-387A-4C56-FFE4-B663DC24FC00/DED29ABA-8FAC-4985-86E0-FCCDA5A290B5/data/Handbook?pageSize=50&where=levelID%3D" + currentLvl));
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
            pageText.text = "0";
        }
        else
        {
            textToShow.text = handBookContent;
            pageText.text = currentPage.ToString() + "/" + handBookData.Count().ToString();
        }
    }

    public void UnPause()
    {
        paused = false;
        panelForHandbook.SetActive(true);
        UICamera.SetActive(true);

        if (handBookData.Count() < 1)
        {
            textToShow.text = "Terjadi kesalahan. Mohon kontak developer.";
        }
        else
        {
            FindHandBookContent(1);
        }
    }

    private void Exit()
    {
        paused = true;
        panelForHandbook.SetActive(false);
        UICamera.SetActive(false);
        gameManager.GetComponent<GameManager>().UnPause();
    }

}
