using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] items;

    [SerializeField]
    private DataFromAPI[] resultList;

    private const string appKey = "09476775-387A-4C56-FFE4-B663DC24FC00";
    private const string apiKey = "DED29ABA-8FAC-4985-86E0-FCCDA5A290B5";


    // Start is called before the first frame update
    void Start()
    {
        GetDataFromAPI();
        //items = GameObject.FindGameObjectsWithTag("Item");
    }

    private GameObject FindResult(GameObject a, GameObject b)
    {
        List<string> resultName =
           (
            from DataFromAPI in resultList
            where DataFromAPI.ItemA == a.name && DataFromAPI.ItemB == b.name
            select DataFromAPI.Result
           ).ToList<string>();

        if(resultName != null)
        {
            string result = resultName.FirstOrDefault();
            List<GameObject> Result =
                (
                    from data in items
                    where data.name == result
                    select data
                ).ToList<GameObject>();

            return Result.FirstOrDefault();
        }

        return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private string FixJson(string value)
    {
        value = "{\"Items\":" + value + "}";
        return value;
    }

    private void GetDataFromAPI()
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("https://api.backendless.com/09476775-387A-4C56-FFE4-B663DC24FC00/DED29ABA-8FAC-4985-86E0-FCCDA5A290B5/data/Thesis?where=lvl%3D1"));
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        jsonResponse = FixJson(jsonResponse);

        resultList = JsonHelper.FromJson<DataFromAPI>(jsonResponse);
    }

}
