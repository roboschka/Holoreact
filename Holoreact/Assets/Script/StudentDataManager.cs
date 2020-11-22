using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class StudentDataManager : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField studentNameField, studentSchoolField;
    
    [SerializeField]
    private Canvas mainMenuCanvas, self;

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

                mainMenu.isFirstPlay = false;
                self.gameObject.SetActive(false);
                
                mainMenuCanvas.gameObject.SetActive(true);
                mainMenu.playButton.Select();
            }
        }
    }
}
