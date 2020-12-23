using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Net;
using System.Linq;
using System.IO;
using TMPro;

public class ChooseLevelManager : MonoBehaviour
{
    private Level[] levels;
    private Score[] scores;

    [SerializeField]
    private SpriteRenderer[] stars;
    [SerializeField]
    private Sprite starFilled, starEmpty;

    private int currentViewingLevel = 0;
    private string studentID;

    [SerializeField]
    private TextMeshProUGUI levelName, levelDescription, pagination;
   
    // Start is called before the first frame update
    private void Start()
    {
        studentID = PlayerPrefs.GetInt("studentID").ToString();

        GetLevelList();
        GetStudentScore();
        
        ShowLevelInfo(currentViewingLevel);
    }

    // Update is called once per frame
    private void Update()
    {

        Navigation();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }
        //Debug.Log(currentViewingLevel);
        //Debug.Log("Get level " + levels[currentViewingLevel].LevelID);

        if (Input.GetKeyDown(KeyCode.Return))
        {
            
            PlayerPrefs.SetInt("currentLevel", levels[currentViewingLevel].LevelID);
            SceneManager.LoadScene("Gameplay");
        }
    }

    private void Navigation()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentViewingLevel < levels.Length - 1)
            {
                currentViewingLevel++;
                //Debug.Log("++: " + currentViewingLevel);
            }
            else
            {
                currentViewingLevel = 0;
            }
            ShowLevelInfo(currentViewingLevel);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentViewingLevel > 0)
            {
                currentViewingLevel--;
                //Debug.Log("--: " + currentViewingLevel);
            }
            else
            {
                currentViewingLevel = levels.Length - 1;
                //Debug.Log("length - 1: " + currentViewingLevel);
            }
            ShowLevelInfo(currentViewingLevel);
        }
    }

    private void ShowLevelInfo(int index)
    {
        levelName.text = levels[index].LvlName;
        levelDescription.text = levels[index].Description;
        pagination.text = index + 1 + "/" + levels.Length;

        DetermineStars(CalculateTotalScore(scores, index));
        
    }


    #region Star System
    private int CalculateTotalScore(Score[] scoreData, int currentIndex)
    {
        List<int> dataList =
            (
                from score in scoreData
                where score.LevelID == (currentIndex + 1)
                select score.QuizScore
            ).ToList();

        return dataList.Take(3).Sum();
    }

    private void DetermineStars(int totalScore)
    {
        Debug.Log(totalScore/100);

        ChangeStarSprite(3, starEmpty);

        switch(totalScore/100)
        {
            case 1:
                ChangeStarSprite(1, starFilled);
                break;
            case 2:
                ChangeStarSprite(2, starFilled);
                break;
            case 3:
                ChangeStarSprite(3, starFilled);
                break;
            default:
                ChangeStarSprite(3, starEmpty);
                break;
        }
    }

    private void ChangeStarSprite(int amountOfStars, Sprite starType)
    {
        for (int i = 0; i < amountOfStars; i++)
        {
            stars[i].sprite = starType;
        }
    }
    #endregion

    #region API Functionality
    private void GetLevelList()
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("https://api.backendless.com/09476775-387A-4C56-FFE4-B663DC24FC00/DED29ABA-8FAC-4985-86E0-FCCDA5A290B5/data/Level"));
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string JSONResponse = reader.ReadToEnd();
        JSONResponse = JsonHelper.FixJSon(JSONResponse);

        levels = JsonHelper.FromJson<Level>(JSONResponse);
    }

    private void GetStudentScore()
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("https://api.backendless.com/09476775-387A-4C56-FFE4-B663DC24FC00/DED29ABA-8FAC-4985-86E0-FCCDA5A290B5/data/Score?where=studentID%3D" + studentID + "&sortBy=created%20desc"));
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string JSONResponse = reader.ReadToEnd();
        JSONResponse = JsonHelper.FixJSon(JSONResponse);

        scores = JsonHelper.FromJson<Score>(JSONResponse);
    }
    #endregion
}
