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
    [SerializeField]
    private AudioSource source;
    [SerializeField]
    private AudioClip navigation, back, enter;

    private int currentViewingLevel = 0;
    private string studentID;

    [SerializeField]
    private TextMeshProUGUI levelName, levelDescription, pagination;
   
    // Start is called before the first frame update
    private void Start()
    {
        studentID = PlayerPrefs.GetInt("studentID").ToString();
        Debug.Log(studentID);
        GetLevelList();
        GetStudentScore();
        ShowLevelInfo();
    }

    // Update is called once per frame
    private void Update()
    {
        Navigation();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StartCoroutine(DelayedSceneLoad(back, "MainMenu"));
        }
        //Debug.Log(currentViewingLevel);
        //Debug.Log("Get level " + levels[currentViewingLevel].LevelID);

        if (Input.GetKeyDown(KeyCode.Return))
        {
            
            PlayerPrefs.SetInt("currentLevel", levels[currentViewingLevel].LevelID);
            StartCoroutine(DelayedSceneLoad(enter, "Gameplay"));
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
            ShowLevelInfo();
            source.PlayOneShot(navigation, 0.6f);
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
            ShowLevelInfo();
            source.PlayOneShot(navigation, 0.6f);
        }
    }

    private void ShowLevelInfo()
    {
        levelName.text = levels[currentViewingLevel].LvlName;
        levelDescription.text = levels[currentViewingLevel].Description;
        pagination.text = (currentViewingLevel + 1) + "/" + levels.Length;

        DetermineStars(CalculateTotalScore());
    }
    
    #region Star System
    private int CalculateTotalScore()
    {
        List<int> dataList =
            (
                from score in scores
                where score.LevelID == (currentViewingLevel + 1)
                select score.QuizScore
            ).ToList();
        return dataList.Sum();
    }

    private void DetermineStars(int totalScore)
    {
        //Debug.Log(totalScore/100);
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
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("https://api.backendless.com/09476775-387A-4C56-FFE4-B663DC24FC00/DED29ABA-8FAC-4985-86E0-FCCDA5A290B5/data/Level?sortBy=LevelID%20asc"));
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string JSONResponse = reader.ReadToEnd();
        JSONResponse = JsonHelper.FixJSon(JSONResponse);

        levels = JsonHelper.FromJson<Level>(JSONResponse);
    }

    private void GetStudentScore()
    {
        List<Score> scorelist = new List<Score>();
        foreach (Level level in levels)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("https://api.backendless.com/09476775-387A-4C56-FFE4-B663DC24FC00/DED29ABA-8FAC-4985-86E0-FCCDA5A290B5/data/Score?pageSize=3&where=studentid%3D" + studentID + "%20and%20levelid%3D" + level.LevelID + "&sortBy=created%20desc"));
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string JSONResponse = reader.ReadToEnd();
            JSONResponse = JsonHelper.FixJSon(JSONResponse);

            scorelist.AddRange(JsonHelper.FromJson<Score>(JSONResponse).ToList());
        }
        scores = scorelist.ToArray();
        Debug.Log(scores);
    }

    #endregion

    #region SFX
    private IEnumerator DelayedSceneLoad(AudioClip audioToBePlayed, string sceneName)
    {
        source.PlayOneShot(audioToBePlayed, 0.6f);
        yield return new WaitForSeconds(audioToBePlayed.length);
        SceneManager.LoadScene(sceneName);
    }
    #endregion
}
