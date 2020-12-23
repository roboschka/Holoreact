using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;
using System.Linq;
using System.IO;

public class StudentDataManager : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField studentNameField, studentSchoolField;
    
    [SerializeField]
    private Canvas self;

    private string studentName, studentSchool, studentID;

    [SerializeField]
    private MainMenu mainMenu;
    private StudentData student;
    private GameObject highlightedButton;

    // Start is called before the first frame update
    private void Start()
    {
        studentNameField.ActivateInputField();
        studentSchoolField.gameObject.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            if (studentNameField.IsActive())
            {
                studentNameField.ActivateInputField();
            }
            else if (studentSchoolField.IsActive())
            {
                studentSchoolField.ActivateInputField();
            }
            
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (studentNameField.IsActive())
            {
                studentName = studentNameField.text;

                studentNameField.gameObject.SetActive(false);
                studentNameField.DeactivateInputField();
                studentSchoolField.gameObject.SetActive(true);
                studentSchoolField.ActivateInputField();
            }
            else
            {
                studentSchool = studentSchoolField.text;
                studentSchoolField.DeactivateInputField();

                PostStudentData(studentName, studentSchool);

                mainMenu.setFirstPlay(false);
                PlayerPrefs.SetInt("isFirstPlay", mainMenu.getFirstPlay() ? 1 : 0);

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
        var result = "";

        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        {
            result = streamReader.ReadToEnd();
            result = FixSingularJSonResponse(result);
            Debug.Log(result);
            student = JsonHelper.FromJson<StudentData>(result).FirstOrDefault();
            SaveStudentData(student);
        }
    }

    private void SaveStudentData(StudentData student)
    {
        if (student == null)
        {
            Debug.Log("Retry to get data");
        } 
        else
        {
            Debug.Log("currentID=" + student.ID);
            PlayerPrefs.SetInt("studentID", student.ID);
        }
    }
    #endregion

    private string FixSingularJSonResponse(string value)
    {
        value = "{\"Items\":[" + value + "]}";
        return value;
    }
}
