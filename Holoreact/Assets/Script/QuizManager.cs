using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class QuizManager : MonoBehaviour
{
    private Questions[] questionList;
    private Level[] currentLevelInfo;

    [SerializeField]
    private TMP_InputField answerField;
    
    [SerializeField]
    private TextMeshProUGUI questionLabel, preScoreText, expScoreText, postScoreText, currentLevelName;

    [SerializeField]
    private GameObject UICamera, gameManager, panelForPostGame, panelForQuiz, warningLabel;

    [SerializeField]
    private SpriteRenderer[] stars;

    [SerializeField]
    private Sprite starFilled, starEmpty;

    private int currentLvl, index, preTestScore, experimentScore, postTestScore, studentId;
    private float correctAnswer;
    private bool paused, isPostTest, finish;

   

    // Start is called before the first frame update
    private void Awake()
    {
        currentLvl = PlayerPrefs.GetInt("currentLevel");
        studentId = PlayerPrefs.GetInt("studentID");

        index = 0;

        paused = false;
        isPostTest = false;
        finish = false;
        warningLabel.SetActive(false);

        GetQuestionDataFromAPI();
        GetLevelFromAPI();

        answerField.ActivateInputField();
        LoadQuestion();
        
    }

    // Update is called once per frame
    void Update()
    {   
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            answerField.ActivateInputField();
        }

        if (!finish)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (answerField.text.Trim().Length == 0)
                {
                    answerField.ActivateInputField();
                    warningLabel.SetActive(true);
                }
                else
                {
                    Submit();
                    answerField.text = "";
                    answerField.ActivateInputField();
                    warningLabel.SetActive(false);
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SceneManager.LoadScene("MainMenu");
            }
        }
    }

    private void LoadQuestion()
    {
        questionLabel.text = questionList[index].Question;
    }
    
    private void Submit()
    {
        //Debug.Log("answer: " + questionList[index].Answer);
        //Debug.Log("user: " + answerField.text);
        if(answerField.text.Equals(questionList[index].Answer, StringComparison.InvariantCultureIgnoreCase))
        {
            correctAnswer++;
            //Debug.Log(correctAnswer);
        }

        index += 1;
        
        
        if(index >= questionList.Length)
        {
            if (isPostTest)
            {
                panelForQuiz.SetActive(false);
                panelForPostGame.SetActive(true);

                postTestScore = (int) (correctAnswer * 100) / questionList.Length;

                preScoreText.text = preTestScore.ToString();
                expScoreText.text = experimentScore.ToString();
                postScoreText.text = postTestScore.ToString();
                currentLevelName.text = currentLevelInfo[0].LvlName + " Terselesaikan";
                CalculateScore();

                PostScoreToAPI(preTestScore,"Pre", studentId);
                PostScoreToAPI(experimentScore,"Experiment", studentId);
                PostScoreToAPI(postTestScore, "Post", studentId);
                finish = true;
            }
            else
            {
                preTestScore = (int) (correctAnswer * 100) / questionList.Length;
                Debug.Log(preTestScore);

                index = 0;
                UICamera.SetActive(false);
                panelForQuiz.SetActive(false);
                paused = true;
                gameManager.GetComponent<GameManager>().UnPause();
                questionLabel.text = questionList[index].Question;

                correctAnswer = 0;
            }
        }
        else
        {
            LoadQuestion();
        }

    }

    private void CalculateScore()
    {
        int totalScore = preTestScore + postTestScore + experimentScore;
        ChangeStarSprite(3, starEmpty);
        switch (totalScore / 100)
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

    public void SetExperimentScore(int score)
    {
        experimentScore = score;
    }

    public void PostTest()
    {
        isPostTest = true;
        UICamera.SetActive(true);
        panelForQuiz.SetActive(true);
        paused = false;
        index = 0;
        LoadQuestion();
    }

    #region Get Data From API

    private void GetQuestionDataFromAPI()
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("https://api.backendless.com/09476775-387A-4C56-FFE4-B663DC24FC00/DED29ABA-8FAC-4985-86E0-FCCDA5A290B5/data/Quiz?where=LevelID%3D" + currentLvl));
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        jsonResponse = JsonHelper.FixJSon(jsonResponse);

        questionList = JsonHelper.FromJson<Questions>(jsonResponse);
    }

    private void GetLevelFromAPI()
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("https://api.backendless.com/09476775-387A-4C56-FFE4-B663DC24FC00/DED29ABA-8FAC-4985-86E0-FCCDA5A290B5/data/Level?where=levelid%3D" + currentLvl));
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        jsonResponse = JsonHelper.FixJSon(jsonResponse);

        currentLevelInfo = JsonHelper.FromJson<Level>(jsonResponse);
    }

    #endregion

    #region Post Data to API
    /// <summary>
    /// PostDataToAPI
    /// Example : PostScoreToAPI(0, 100, "pre", 1);
    /// </summary>
    /// <param name="levelID"></param>
    /// <param name="name"></param>
    /// <param name="schoolName"></param>
    /// <param name="TestType"></param>

    private void PostScoreToAPI(int score, string quizType, int studentID)
    {
        #region Create Request
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("https://api.backendless.com/09476775-387A-4C56-FFE4-B663DC24FC00/DED29ABA-8FAC-4985-86E0-FCCDA5A290B5/data/Score"));
        request.ContentType = "application/json";
        request.Method = "POST";

        using (var streamWriter = new StreamWriter(request.GetRequestStream()))
        {
            string json = "{\"LevelID\":\"" + currentLvl + "\"," +
                          "\"QuizScore\":\"" + score + "\"," +
                          "\"QuizType\":\"" + quizType + "\"," +
                          "\"StudentID\":\"" + studentID + "\"}";
            streamWriter.Write(json);
        }
        #endregion 

        #region Get Response
        var httpResponse = (HttpWebResponse)request.GetResponse();
        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        {
            var result = streamReader.ReadToEnd();
        }

        #endregion

    }

    #endregion

}
