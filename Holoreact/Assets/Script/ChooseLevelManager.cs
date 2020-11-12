using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Linq;
using System.IO;

public class ChooseLevelManager : MonoBehaviour
{
    private Level[] levels;
    private int currentViewingLevel = 0;
   
    // Start is called before the first frame update
    void Start()
    {
        GetLevelList();
        Debug.Log(levels[0].Description);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentViewingLevel++;
            Debug.Log(currentViewingLevel);
        }
    }

    #region API Functionality
    private string FixJson(string value)
    {
        value = "{\"Items\":" + value + "}";
        return value;
    }
    private void GetLevelList()
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("https://api.backendless.com/09476775-387A-4C56-FFE4-B663DC24FC00/DED29ABA-8FAC-4985-86E0-FCCDA5A290B5/data/Level"));
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string JSONResponse = reader.ReadToEnd();
        JSONResponse = JsonHelper.FixJSon(JSONResponse);

        levels = JsonHelper.FromJson<Level>(JSONResponse);
    }

    private void CheckLevelList()
    {
        foreach(Level data in levels)
        {
            Debug.Log(data.lvlID);
            Debug.Log(data.Description);
            Debug.Log(data.lvlName);
        }
    }
    #endregion
}
