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
    private List<GameObject> itemList;

    [SerializeField]
    private Combination[] combinationList;

    [SerializeField]
    private HandBook[] handBookData;

    private int counter;

    bool selectedItem;
    int selectedIndex;

    // Start is called before the first frame update
    void Start()
    {
        counter = 0;
        itemList = new List<GameObject>();
        selectedItem = false;
        selectedIndex = -1;

        //PostDataToAPI(1, "test", "test");

        //GetHandbookData();
        //FindHandBookContent(1);

        GetItemList();

        GetCombinationDataFromAPI();

        itemList[0].SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("left"))
        {
            Move(-1);
        }
        else if(Input.GetKeyDown("right"))
        {
            Move(1);
        }
        else if(Input.GetKeyDown("return"))
        {
            Select();
        }
    }

    private string FixJson(string value)
    {
        value = "{\"Items\":" + value + "}";
        return value;
    }

    private void CalculateScore()
    {
        float result = counter / combinationList.Count();
    }

    private void Move(int direction)
    {
        itemList[counter].SetActive(false);
        if (counter + direction < 0)
        {
            counter = itemList.Count - 1;
        }
        else if( counter + direction > (itemList.Count - 1) )
        {
            counter = 0;
        }
        else
        {
            counter += direction;
        }
        itemList[counter].SetActive(true);
    }

    private void Select()
    {
        if (selectedItem)
        {
            Combine();
        }
        else
        {
            itemList[counter].transform.Translate(2, 0, 0);
            selectedItem = true;
            selectedIndex = counter;

            int temp = counter;
            Move(1);
            itemList[temp].SetActive(true);
        }
    }

    private void Combine()
    {
        if(FindCombinationResult(itemList[selectedIndex],itemList[counter]))
        {
            //deactive to off all object and then active the combine result object
            DeactiveAllItemAndResetPosition();

            counter = itemList.Count - 1;
            itemList[counter].SetActive(true);
        }
        else
        {
            //give marning and reduce score
            DeactiveAllItemAndResetPosition();

            itemList[counter].SetActive(true);
        }
        selectedItem = false;
        selectedIndex = -1;
    }

    private void DeactiveAllItemAndResetPosition()
    {
        foreach(GameObject data in itemList)
        {
            data.transform.position = new Vector3(0,0,0);

            data.SetActive(false);
        }
    }

    #region Get Data from API
    /// <summary>
    /// GetCombinationDataFromAPI()
    /// GetHandbookData()
    /// GetItemList()
    /// </summary>

    private void GetItemList()
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("https://api.backendless.com/09476775-387A-4C56-FFE4-B663DC24FC00/DED29ABA-8FAC-4985-86E0-FCCDA5A290B5/data/ItemList?where=LvlID%3D0"));
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        jsonResponse = FixJson(jsonResponse);

        Item[] items; 

        items = JsonHelper.FromJson<Item>(jsonResponse);

        foreach(Item item in items)
        {
            GameObject instance = Instantiate(Resources.Load("Prefab/Test/" + item.Name) as GameObject);
            itemList.Add(instance);
            instance.SetActive(false);
        }
    }

    private void GetCombinationDataFromAPI()
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("https://api.backendless.com/09476775-387A-4C56-FFE4-B663DC24FC00/DED29ABA-8FAC-4985-86E0-FCCDA5A290B5/data/CombinationList?where=lvlID%3D1"));
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        jsonResponse = FixJson(jsonResponse);

        combinationList = JsonHelper.FromJson<Combination>(jsonResponse);
    }

    private void GetHandbookData()
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("https://api.backendless.com/09476775-387A-4C56-FFE4-B663DC24FC00/DED29ABA-8FAC-4985-86E0-FCCDA5A290B5/data/Handbook?where=lvlid%3D1"));
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        jsonResponse = FixJson(jsonResponse);

        handBookData = JsonHelper.FromJson<HandBook>(jsonResponse);
    }

    #endregion

    #region Post Data to API
    /// <summary>
    /// PostDataToAPI
    /// </summary>
    /// <param name="lvlID"></param>
    /// <param name="name"></param>
    /// <param name="schoolName"></param>

    private void PostDataToAPI(int lvlID, string name, string schoolName)
    {
        #region Create Request
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("https://api.backendless.com/09476775-387A-4C56-FFE4-B663DC24FC00/DED29ABA-8FAC-4985-86E0-FCCDA5A290B5/data/StudentData"));
        request.ContentType = "application/json";
        request.Method = "POST";

        using (var streamWriter = new StreamWriter(request.GetRequestStream()))
        {
            string json = "{\"LvlID\":\"" + lvlID + "\"," +
                          "\"Name\":\"" + name + "\"," +
                          "\"SchoolName\":\"" + schoolName + "\"}";
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

    #region Find

    /// <summary>
    /// FindCombinationResult
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns>GameObject</returns>
    /// 

    /// <summary>
    /// FindHandBookContent
    /// </summary>
    /// <param name="page"></param>
    /// <returns>string</returns>

    private bool FindCombinationResult(GameObject a, GameObject b)
    {
        List<string> resultName =
           (
            from data in combinationList
            where ( data.FirstItem == a.name.Replace("(Clone)","") && data.SecondItem == b.name.Replace("(Clone)", "") || (data.FirstItem == b.name.Replace("(Clone)", "") && data.SecondItem == a.name.Replace("(Clone)", "")) )
            select data.Result
           ).ToList();

        if (resultName != null)
        {
            bool exist = false;
            try
            {
                String.IsNullOrEmpty(itemList.Where(x => x.name.Replace("(Clone)", "") == resultName.FirstOrDefault() ).FirstOrDefault().name);
                exist = true;
            }
            catch (Exception)
            {
                exist = false;
            }

            if (!exist)
            {
                string result = resultName.FirstOrDefault();
                GameObject instance = Instantiate(Resources.Load("Prefab/Test/" + result) as GameObject);

                itemList.Add(instance);

                return true;
            }
        }
        return false;
    }

    private string FindHandBookContent(int page)
    {
        string handBookContent =
            (
                from content in handBookData
                where content.Page == page
                select content.Text
                
            ).ToList().FirstOrDefault();

        if(String.IsNullOrEmpty(handBookContent))
        {
            Debug.Log("not found");
            return "something goes wrong please contact the developer";
        }

        Debug.Log(handBookContent);
        return handBookContent;
    }

    #endregion

    #region Check Data by Debug.log

    /// <summary>
    /// CheckHandBookData()
    /// CheckCombinationData()
    /// </summary>

    private void CheckHandBookData()
    {

        foreach (HandBook data in handBookData)
        {
            Debug.Log(data.Page);
            Debug.Log(data.Text);
            //Debug.Log(data.LvlID);
        }
    }

    private void CheckCombinationData()
    {
        foreach(Combination data in combinationList)
        {
            //Debug.Log(data.LvlID);
            Debug.Log(data.FirstItem);
            Debug.Log(data.SecondItem);
            Debug.Log(data.Result);
        }
    }

    #endregion

}
