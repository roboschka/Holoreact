using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using TMPro;
using UnityEngine;

public class QuizManager : MonoBehaviour
{
    private int currentLvl;

    private Questions[] questionList;

    [SerializeField]
    private TMP_InputField answerField;

    [SerializeField]
    private GameObject panelForQuiz;

    [SerializeField]
    private TextMeshProUGUI questionLabel;

    [SerializeField]
    private GameObject cameraForQuiz;

    [SerializeField]
    private GameObject gameManager;

    private int index;
    private  int correctAnswer;

    private int preTestScore;
    private int experimentScore;
    private int postTestScore;

    private bool paused;
    private bool isPostTest;

    // Start is called before the first frame update
    void Start()
    {
        currentLvl = PlayerPrefs.GetInt("currentLevel");
        index = 0;
        paused = false;
        isPostTest = false;
        GetQuestionDataFromAPI();
        questionLabel.text = questionList[0].Question;
        answerField.ActivateInputField();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Submit();
        }
    }
    
    private void Submit()
    {
        if(answerField.text.Equals(questionList[index].Answer, StringComparison.InvariantCultureIgnoreCase))
        {
            correctAnswer += 1;
        }

        index += 1;
        
        if(index > questionList.Length)
        {
            if (isPostTest)
            {
                //do something
            }
            else
            {
                preTestScore = (correctAnswer / questionList.Length) * 100;
                index = 0;
                cameraForQuiz.SetActive(false);
                paused = true;
                gameManager.GetComponent<GameManager>().UnPause();
            }
        }

    }

    public void SetExperimentScore(int score)
    {
        experimentScore = score;
    }

    public void PostTest()
    {
        isPostTest = true;
        cameraForQuiz.SetActive(true);
        panelForQuiz.SetActive(true);
        paused = false;
        index = 0;
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
