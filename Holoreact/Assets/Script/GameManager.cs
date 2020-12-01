using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{

    private List<GameObject> itemList;
    private Combination[] combinationList;

    [SerializeField]
    private GameObject cameraForGameplay;

    [SerializeField]
    private GameObject handBookManager;

    [SerializeField]
    private GameObject quizManager;

    private int counter;

    private bool selectedItem;
    private int selectedIndex;
    private bool paused;

    private int currentLvl;

    // Start is called before the first frame update
    void Start()
    {
        counter = 0;
        itemList = new List<GameObject>();
        selectedItem = false;
        selectedIndex = 0;

        currentLvl = PlayerPrefs.GetInt("currentLevel");

        GetItemList();

        GetCombinationDataFromAPI();

        itemList[0].SetActive(true);

        paused = true;

        DeactiveAllItemAndResetPosition();
        Debug.Log("start called");
    }

    // Update is called once per frame
    void Update()
    {
        if (!paused)
        {
            if (Input.GetKeyDown("left"))
            {
                Move(-1);
            }
            else if (Input.GetKeyDown("right"))
            {
                Move(1);
            }
            else if (Input.GetKeyDown("return"))
            {
                Select();
            }
        }
    }

    private int CalculateExperimentScore()
    {
        int result;
        return  result = counter / combinationList.Count() * 100;
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
        if (String.Equals(itemList[counter].name.Replace("(Clone)", ""),"Handbook"))
        {
            ShowHandbook();
        }
        else if (String.Equals(itemList[counter].name.Replace("(Clone)", ""), "ButtonSubmit"))
        {
            quizManager.GetComponent<QuizManager>().SetExperimentScore(CalculateExperimentScore());
            quizManager.GetComponent<QuizManager>().PostTest();
        }
        else
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
    }

    private void ShowHandbook()
    {
        Debug.Log("selectedIndex at showHandbook: " + selectedIndex);
        cameraForGameplay.SetActive(false);
        itemList[selectedIndex].SetActive(false);
        paused = true;
        handBookManager.GetComponent<HandBookManager>().UnPause();
    }

    public void UnPause()
    {
        paused = false;
        cameraForGameplay.SetActive(true);
        itemList[selectedIndex].SetActive(true);
        Debug.Log("selectedIndex at UnPause: " + selectedIndex);
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
        selectedIndex = selectedIndex - 1;
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
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("https://api.backendless.com/09476775-387A-4C56-FFE4-B663DC24FC00/DED29ABA-8FAC-4985-86E0-FCCDA5A290B5/data/ItemList?pageSize=50&where=levelid%3D"+currentLvl));
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        jsonResponse = JsonHelper.FixJSon(jsonResponse);

        Item[] items; 

        items = JsonHelper.FromJson<Item>(jsonResponse);

        foreach(Item item in items)
        {
            Debug.Log(item.Name);
            GameObject instance = Instantiate(Resources.Load("Prefab/" + item.Name) as GameObject);
            itemList.Add(instance);
            instance.SetActive(false);
        }
    }

    private void GetCombinationDataFromAPI()
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("https://api.backendless.com/09476775-387A-4C56-FFE4-B663DC24FC00/DED29ABA-8FAC-4985-86E0-FCCDA5A290B5/data/CombinationList?pageSize=50&offset=0&where=levelid%3D"+currentLvl));
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        jsonResponse = JsonHelper.FixJSon(jsonResponse);

        combinationList = JsonHelper.FromJson<Combination>(jsonResponse);
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

        if (resultName != null && resultName.Count > 0)
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

    #endregion

    #region Check Data by Debug.log

    /// <summary>
    /// CheckCombinationData()
    /// </summary>

    private void CheckCombinationData()
    {
        foreach(Combination data in combinationList)
        {
            //Debug.Log(data.LevelID);
            Debug.Log(data.FirstItem);
            Debug.Log(data.SecondItem);
            Debug.Log(data.Result);
        }
    }

    #endregion

}
