using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;
using System.IO;

public class StudentDataManager : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField studentNameField, studentSchoolField;
    
    [SerializeField]
    private Canvas self;

    private string studentName, studentSchool;

    public MainMenu mainMenu;

    GameObject highlightedButton;

    // Start is called before the first frame update
    void Start()
    {
        studentNameField.ActivateInputField();
        studentSchoolField.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (studentNameField.IsActive())
            {
                studentName = studentNameField.text;
                Debug.Log("studentName: " + studentName);

                studentNameField.gameObject.SetActive(false);
                studentNameField.DeactivateInputField();
                studentSchoolField.gameObject.SetActive(true);
                studentSchoolField.ActivateInputField();
            }
            else
            {
                studentSchool = studentSchoolField.text;
                Debug.Log("studentSchool: " + studentSchool);
                studentSchoolField.DeactivateInputField();

                PostStudentData(studentName, studentSchool);

                mainMenu.isFirstPlay = false;
                self.gameObject.SetActive(false);

                mainMenu.ToggleNotification(false, true, mainMenu.playButton);
            }
        }
    }

    #region POST API
    private void PostStudentData(string studentName, string studentSchool)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("https://api.backendless.com/09476775-387A-4C56-FFE4-B663DC24FC00/DED29ABA-8FAC-4985-86E0-FCCDA5A290B5/data/Student"));
        request.ContentType = "application/json";
        request.Method = "POST";

        using (var streamWriter = new StreamWriter(request.GetRequestStream()))
        {
            string json = "{\"Name\":\"" + studentName + "\"," +
                          "\"SchoolName\":\"" + studentSchool + "\"}";
            streamWriter.Write(json);
        }

        var httpResponse = (HttpWebResponse)request.GetResponse();
        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        {
            var result = streamReader.ReadToEnd();
            Debug.Log(result);
        }
    }
    #endregion
}
