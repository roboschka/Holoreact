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
    private string sorting = "&sortBy=created%20desc";

    [SerializeField]
    private SpriteRenderer[] stars;
    [SerializeField]
    private Sprite starFilled, starEmpty;

    [SerializeField]
    private int currentViewingLevel = 0;

    [SerializeField]
    private string tempStudentID = "0";

    [SerializeField]
    private TextMeshProUGUI levelName, levelDescription, pagination;
   
    // Start is called before the first frame update
    void Start()
    {
        GetLevelList();
        GetStudentScore();

        showLevelInfo(currentViewingLevel);
    }

    // Update is called once per frame
    void Update()
    {

        navigation();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("Get level " + levels[currentViewingLevel].LvlID);
        }
        
    }

    private void navigation()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentViewingLevel < levels.Length - 1)
            {
                currentViewingLevel++;
            }
            else
            {
                currentViewingLevel = 0;
            }
            showLevelInfo(currentViewingLevel);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentViewingLevel > 0)
            {
                currentViewingLevel--;
            }
            else
            {
                currentViewingLevel = levels.Length - 1;
            }
            showLevelInfo(currentViewingLevel);
        }
    }

    private void showLevelInfo(int index)
    {
        levelName.text = levels[index].LvlName;
        levelDescription.text = levels[index].Description;
        pagination.text = index + 1 + "/" + levels.Length;

        determineStars(calculateTotalScore(scores, index));
        
    }


    #region Star System
    private int calculateTotalScore(Score[] scoreData, int currentIndex)
    {
        var data =
            from score in scoreData
            where Int32.Parse(score.LevelID) == (currentIndex + 1)
            select Int32.Parse(score.QuizScore);

        List<int> dataList = data.ToList();
        return dataList.Take(3).Sum(); ;
    }

    private void determineStars(int totalScore)
    {
        Debug.Log(totalScore);

        changeStarSprite(3, starEmpty);

        if (totalScore == 300)
        {
            changeStarSprite(3, starFilled);
        }
        else if (totalScore >= 200)
        {
            changeStarSprite(2, starFilled);
        }
        else if (totalScore >= 100)
        {
            changeStarSprite(1, starFilled);
        }
        else if (totalScore >= 0)
        {
            changeStarSprite(3, starEmpty);
        }
    }

    private void changeStarSprite(int amountOfStars, Sprite starType)
    {
        for (int i = 0; i < amountOfStars; i++)
        {
            stars[0].sprite = starType;
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
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("https://api.backendless.com/09476775-387A-4C56-FFE4-B663DC24FC00/DED29ABA-8FAC-4985-86E0-FCCDA5A290B5/data/Score?where=studentID%3D" + tempStudentID + sorting));
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string JSONResponse = reader.ReadToEnd();
        JSONResponse = JsonHelper.FixJSon(JSONResponse);

        scores = JsonHelper.FromJson<Score>(JSONResponse);
        
    }
    #endregion
}
