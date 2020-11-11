using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuTest : MonoBehaviour
{
    [SerializeField]
    private Button otherPlayButton;
    // Start is called before the first frame update
    void Start()
    {
        otherPlayButton.Select();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
