using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    [SerializeField]
    private GameObject panelForPostGame;

    [SerializeField]
    private TextMeshProUGUI scoreText;

    private int index;
    private  int correctAnswer;
    private int studentId;

    private int preTestScore;
    private int experimentScore;
    private int postTestScore;

    private bool paused;
    private bool isPostTest;
    private bool finish;

    // Start is called before the first frame update
    void Start()
    {
        currentLvl = PlayerPrefs.GetInt("currentLevel");
        studentId = PlayerPrefs.GetInt("studentID");
        index = 0;
        paused = false;
        isPostTest = false;
        finish = false;
        GetQuestionDataFromAPI();
        answerField.ActivateInputField();
        LoadQuestion();
    }

    // Update is called once per frame
    void Update()
    {
        if (!finish)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                answerField.Select();
                answerField.text = "";
                Submit();
                answerField.ActivateInputField();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SceneManager.LoadScene("ChooseLevel");
            }
        }
    }

    private void LoadQuestion()
    {
        questionLabel.text = questionList[index].Question;
    }
    
    private void Submit()
    {
        if(answerField.text.Equals(questionList[index].Answer, StringComparison.InvariantCultureIgnoreCase))
        {
            correctAnswer += 1;
        }

        index += 1;
        
        if(index >= questionList.Length)
        {
            if (isPostTest)
            {
                //do something
                panelForPostGame.SetActive(true);
                scoreText.text = "";

                PostScoreToAPI(preTestScore,"Pre",studentId);
                PostScoreToAPI(experimentScore,"Experiment",studentId);
                PostScoreToAPI(postTestScore, "Post", studentId);
                finish = true;
            }
            else
            {
                preTestScore = (correctAnswer / questionList.Length) * 100;
                index = 0;
                cameraForQuiz.SetActive(false);
                paused = true;
                gameManager.GetComponent<GameManagerAlt>().UnPause();
                questionLabel.text = questionList[index].Question;
                Debug.Log("else called");
            }
        }
        else
        {
            LoadQuestion();
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
