using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private List<GameObject> itemList, objectsOnPlane;
    private Combination[] combinationList;
    private Collider[] collidedColliders;
    
    [SerializeField]
    private GameObject cameraForGameplay, handBookManager, submitManager, quizManager, panelConfirmation;
    
    
    //private int selectedIndex;
    private int currentIndex, currentLvl, combinationPerformed;

    private bool paused, selectedItem;

    // Start is called before the first frame update
    private void Awake()
    {
        currentIndex = 1;
        itemList = new List<GameObject>();
        objectsOnPlane = new List<GameObject>();
        selectedItem = false;
        //selectedIndex = -1;
        combinationPerformed = 0;

        currentLvl = PlayerPrefs.GetInt("currentLevel");

        GetItemList();

        GetCombinationDataFromAPI();

        itemList[0].SetActive(true);

        paused = true;

        itemList[currentIndex - 1].SetActive(true);
        itemList[currentIndex].SetActive(true);

        SetFirstItemPosition(itemList[currentIndex - 1]);
        SetSecondItemPosition(itemList[currentIndex]);

    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}

    private void OnCollisionEnter(Collision collision)
    {
        CheckCollidedObject();
    }

    private void OnCollisionExit(Collision collision)
    {
        for (int i = 0; i < objectsOnPlane.Count; i++)
        {
            if (collision.gameObject == objectsOnPlane[i])
            {
                objectsOnPlane.Clear();
            }
        }
    }

    public void Move(int direction)
    {
        //2 Objek pada 2 index sebelumnya dimatikan kecuali yang ada di plane
        if (objectsOnPlane != null && objectsOnPlane.Count > 0)
        {

            //supaya di list dia gak keliatan/terduplikasi
            if (itemList.Count % 2 != 0)
            {
                //kalau index terakhir maka hanya munculkan 1 item
                if (currentIndex == itemList.Count)
                {
                    if (itemList[currentIndex - 1] != objectsOnPlane[0])
                    {
                        itemList[currentIndex - 1].SetActive(false);
                    }
                }
                else
                {
                    //kalau bukan last index seperti normal
                    if (itemList[currentIndex - 1] != objectsOnPlane[0])
                    {
                        itemList[currentIndex - 1].SetActive(false);

                    }
                    if (itemList[currentIndex] != objectsOnPlane[0])
                    {
                        itemList[currentIndex].SetActive(false);
                    }
                }

            }
            else
            {
                //kalau genap seperti normal
                if (itemList[currentIndex - 1] != objectsOnPlane[0])
                {
                    itemList[currentIndex - 1].SetActive(false);

                }
                if (itemList[currentIndex] != objectsOnPlane[0])
                {
                    itemList[currentIndex].SetActive(false);
                }
            }

        }
        else
        {
            #region commented
            //itemList[currentIndex - 1].SetActive(false);
            //itemList[currentIndex].SetActive(false);
            #endregion

            if (itemList.Count % 2 != 0)
            {
                //kalau index terakhir maka hanya munculkan 1 item
                if (currentIndex == itemList.Count)
                {
                    itemList[currentIndex - 1].SetActive(false);
                }
                else
                {
                    //kalau bukan last index seperti normal
                    itemList[currentIndex - 1].SetActive(false);
                    itemList[currentIndex].SetActive(false);
                }

            }
            else
            {
                //kalau genap seperti normal
                itemList[currentIndex - 1].SetActive(false);
                itemList[currentIndex].SetActive(false);
            }

        }

        //ambil 2 objek selanjutnya
        currentIndex += direction;
        //Debug.Log("current index :" + currentIndex);
        //Debug.Log("itemlist count :" + itemList.Count);

        if (currentIndex <= 0)
        {
            //check apakah genap
            if (itemList.Count % 2 == 0)
            {
                currentIndex = itemList.Count - 1;
            }
            else
            {
                currentIndex = itemList.Count;
            }
        }
        //Check apakah jumlah item genap
        else if (currentIndex > itemList.Count - 1 && itemList.Count % 2 == 0)
        {
            currentIndex = 1;
        }
        //jika ganjil
        else
        {
            if(currentIndex > itemList.Count)
            {
                currentIndex = 1;
            }
        }


        //check apakah next object yang akan ditampilkan berada di plane
        if (objectsOnPlane.Count != 0)
        {


            if (itemList.Count % 2 != 0)
            {
                //kalau index terakhir maka hanya munculkan 1 item
                if (currentIndex == itemList.Count)
                {
                    if (itemList[currentIndex - 1] != objectsOnPlane[0])
                    {
                        SetFirstItemPosition(itemList[currentIndex - 1]);
                    }
                }
                else
                {
                    //kalau bukan last index seperti normal
                    if (itemList[currentIndex - 1] != objectsOnPlane[0])
                    {
                        SetFirstItemPosition(itemList[currentIndex - 1]);
                    }
                    if (itemList[currentIndex] != objectsOnPlane[0])
                    {
                        SetSecondItemPosition(itemList[currentIndex]);
                    }
                }

            }
            else
            {
                //kalau genap seperti normal
                if (itemList[currentIndex - 1] != objectsOnPlane[0])
                {
                    SetFirstItemPosition(itemList[currentIndex - 1]);
                }
                if (itemList[currentIndex] != objectsOnPlane[0])
                {
                    SetSecondItemPosition(itemList[currentIndex]);
                }
            }

        }
        else
        {
            #region commneted
            //SetFirstItemPosition(itemList[currentIndex - 1]);
            //SetSecondItemPosition(itemList[currentIndex]);
            #endregion

            if (itemList.Count % 2 != 0)
            {

                //kalau index terakhir maka hanya munculkan 1 item
                if (currentIndex == itemList.Count)
                {
                    SetFirstItemPosition(itemList[currentIndex - 1]);
                }
                else
                {
                    //kalau bukan last index seperti normal
                    SetFirstItemPosition(itemList[currentIndex - 1]);
                    SetSecondItemPosition(itemList[currentIndex]);
                }

            }
            else
            {
                //kalau genap seperti normal
                SetFirstItemPosition(itemList[currentIndex - 1]);
                SetSecondItemPosition(itemList[currentIndex]);
            }

        }


        //aktivasikan 2 objek selanjutnya jika bukan objek di plane.
        if (objectsOnPlane != null && objectsOnPlane.Count > 0)
        {

            //supaya di list dia gak keliatan/terduplikasi.
            Debug.Log(objectsOnPlane.Count);

            if (itemList.Count % 2 != 0)
            {
                //kalau index terakhir maka hanya munculkan 1 item
                if (currentIndex == itemList.Count)
                {
                    if (itemList[currentIndex - 1] != objectsOnPlane[0])
                    {
                        itemList[currentIndex - 1].SetActive(true);
                    }
                }
                else
                {
                    //kalau bukan last index seperti normal
                    if (itemList[currentIndex - 1] != objectsOnPlane[0])
                    {
                        itemList[currentIndex - 1].SetActive(true);
                    }
                    if (itemList[currentIndex] != objectsOnPlane[0])
                    {
                        itemList[currentIndex].SetActive(true);
                    }
                }

            }
            else
            {
                //kalau genap seperti normal
                if (itemList[currentIndex - 1] != objectsOnPlane[0])
                {
                    itemList[currentIndex - 1].SetActive(true);

                }
                if (itemList[currentIndex] != objectsOnPlane[0])
                {
                    itemList[currentIndex].SetActive(true);
                }
            }

        }
        else
        {
            #region commented
            //itemList[currentIndex - 1].SetActive(true);
            //itemList[currentIndex].SetActive(true);
            #endregion
            
            if(itemList.Count % 2 != 0)
            {
                //kalau index terakhir maka hanya munculkan 1 item
                if (currentIndex == itemList.Count)
                {
                    itemList[currentIndex - 1].SetActive(true);
                }
                else
                {
                    //kalau bukan last index seperti normal
                    itemList[currentIndex - 1].SetActive(true);
                    itemList[currentIndex].SetActive(true);
                }

            }
            else
            {
                //kalau genap seperti normal
                itemList[currentIndex - 1].SetActive(true);
                itemList[currentIndex].SetActive(true);
            }

        }


    }

    private void SetFirstItemPosition(GameObject item)
    {
        //Change to the postion of first item latter
        item.transform.position = new Vector3(20, -4f, 4);
        MouseBehaviour mouseDrag = item.GetComponent<MouseBehaviour>();
        mouseDrag.originPosition = new Vector3(20, -4f, 4);
    }

    private void SetSecondItemPosition(GameObject item)
    {
        //Change to the postion of second item latter
        item.transform.position = new Vector3(20, -4f, -4);
        MouseBehaviour mouseDrag = item.GetComponent<MouseBehaviour>();
        mouseDrag.originPosition = new Vector3(20, -4f, -4);
    }

    private void CheckCollidedObject()
    {
        Collider planeCollider = gameObject.GetComponent<Collider>();
        collidedColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale*10, Quaternion.identity, LayerMask.GetMask("ExperimentObject"));
        //Collider[] collidedColliders = Physics.OverlapBox(gameObject.transform.TransformPoint(planeCollider.bounds.center), gameObject.transform.TransformVector(planeCollider.bounds.size), gameObject.transform.rotation);
        int i = 0;
        objectsOnPlane.Clear();
        while (i < collidedColliders.Length)
        {
            objectsOnPlane.Add(collidedColliders[i].gameObject);
            Debug.Log("List: " + objectsOnPlane[i].gameObject.name);
            i++;
        }
        if (i == 2)
        {
            //FindCombinationResult(collidedColliders[0].gameObject, collidedColliders[1].gameObject);\
            Combine();
        }
    }


    private int CalculateExperimentScore()
    {
        int result;
        return result = combinationPerformed / combinationList.Count() * 100;
    }

    public void ShowHandbook()
    {
        //Debug.Log("selectedIndex at showHandbook: " + selectedIndex);
        cameraForGameplay.SetActive(false);
        itemList[currentIndex].SetActive(false);
        paused = true;
        handBookManager.GetComponent<HandBookManager>().UnPause();
    }

    public void UnPause()
    {
        paused = false;
        cameraForGameplay.SetActive(true);
        itemList[currentIndex].SetActive(true);

        //if (selectedIndex != -1)
        //{
        //    itemList[selectedIndex].SetActive(true);
        //}

    }

    private void Combine()
    {
        if( FindCombinationResult(objectsOnPlane[0], objectsOnPlane[1]) )
        {
            #region commneted
            //deactive to off all object and then active the combine result object
            //DeactiveAllItemAndResetPosition();

            //currentIndex = itemList.Count - 1;
            //itemList[currentIndex].SetActive(true);
            #endregion

            //Reset all coliiding object with the plane
            foreach (GameObject objectOnPlane in objectsOnPlane)
            {
                objectOnPlane.SetActive(false);
            }

            objectsOnPlane.Clear();

            //Set combination result object to the plane
            objectsOnPlane.Add(itemList[(itemList.Count - 1)]);

            string animationName =
            (
                from anim in combinationList
                where anim.Result == itemList[itemList.Count - 1].name
                select anim.AnimationName
            ).FirstOrDefault();

            if (!String.IsNullOrEmpty(animationName))
            {
                itemList[(itemList.Count - 1)].GetComponent<Animator>().Play(animationName);
            }

            //set combination result to plane postion
            itemList[itemList.Count - 1].transform.position = gameObject.transform.position;

            if( (itemList.Count - 1) % 2 == 0)
            {
                itemList[itemList.Count - 1].GetComponent<MouseBehaviour>().originPosition = new Vector3(15, -2.5f, 4);
            }
            else
            {
                itemList[itemList.Count - 1].GetComponent<MouseBehaviour>().originPosition = new Vector3(15, -2.5f, -4);
            }

            combinationPerformed += 1;
        }
        else
        {
            foreach (GameObject objectOnPlane in objectsOnPlane)
            {
                objectOnPlane.SetActive(false);
            }

            objectsOnPlane.Clear();

            //itemList[currentIndex - 1].SetActive(true);
            //itemList[currentIndex].SetActive(true);

            //SetFirstItemPosition(itemList[currentIndex - 1]);
            //SetSecondItemPosition(itemList[currentIndex]);

            //show the current object
            ShowCurrentIndexObject();
        }
        #region commented
        //else
        //{
        //    DeactiveAllItemAndResetPosition();

        //    itemList[currentIndex].SetActive(true);

        //}
        //selectedItem = false;
        //selectedIndex = selectedIndex - 1;
        #endregion
    }

    public void ShowCurrentIndexObject()
    {
        //genap
        if (itemList.Count % 2 == 0)
        {
            itemList[currentIndex - 1].SetActive(true);
            itemList[currentIndex].SetActive(true);

            SetFirstItemPosition(itemList[currentIndex - 1]);
            SetSecondItemPosition(itemList[currentIndex]);
        }
        else
        {
            if (currentIndex == itemList.Count)
            {
                itemList[currentIndex - 1].SetActive(true);
                SetFirstItemPosition(itemList[currentIndex - 1]);
            }
            else
            {
                //Sama seperti genap
                itemList[currentIndex - 1].SetActive(true);
                itemList[currentIndex].SetActive(true);

                SetFirstItemPosition(itemList[currentIndex - 1]);
                SetSecondItemPosition(itemList[currentIndex]);
            }
        }

    }

    public void Submit()
    {
        cameraForGameplay.SetActive(false);
        panelConfirmation.SetActive(true);
        paused = true;
        submitManager.GetComponent<SubmitManager>().UnPause();
    }

    public void FinishExperiment()
    {
        quizManager.GetComponent<QuizManager>().SetExperimentScore(CalculateExperimentScore());
        quizManager.GetComponent<QuizManager>().PostTest();
    }

    //private void DeactiveAllItemAndResetPosition()
    //{
    //    foreach (GameObject data in itemList)
    //    {
    //        data.transform.position = new Vector3(0, 0, 0);

    //        data.SetActive(false);
    //    }
    //}

    #region Get Data from API
    /// <summary>
    /// GetCombinationDataFromAPI()
    /// GetHandbookData()
    /// GetItemList()
    /// </summary>

    private void GetItemList()
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("https://api.backendless.com/09476775-387A-4C56-FFE4-B663DC24FC00/DED29ABA-8FAC-4985-86E0-FCCDA5A290B5/data/ItemList?pageSize=50&where=levelid%3D" + currentLvl));
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        jsonResponse = JsonHelper.FixJSon(jsonResponse);

        Item[] items;

        items = JsonHelper.FromJson<Item>(jsonResponse);

        foreach (Item item in items)
        {
            GameObject instance = Instantiate(Resources.Load("Prefab/" + item.Name) as GameObject);
            itemList.Add(instance);
            instance.SetActive(false);
        }
    }

    private void GetCombinationDataFromAPI()
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("https://api.backendless.com/09476775-387A-4C56-FFE4-B663DC24FC00/DED29ABA-8FAC-4985-86E0-FCCDA5A290B5/data/CombinationList?pageSize=50&offset=0&where=levelid%3D" + currentLvl));
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
            where (data.FirstItem == a.name.Replace("(Clone)", "") && data.SecondItem == b.name.Replace("(Clone)", "") || (data.FirstItem == b.name.Replace("(Clone)", "") && data.SecondItem == a.name.Replace("(Clone)", "")))
            select data.Result
           ).ToList();

        if (resultName != null && resultName.Count > 0)
        {
            bool exist = false;
            try
            {
                String.IsNullOrEmpty(itemList.Where(x => x.name.Replace("(Clone)", "") == resultName.FirstOrDefault()).FirstOrDefault().name);
                exist = true;
            }
            catch (Exception)
            {
                exist = false;
            }

            if (!exist)
            {
                string result = resultName.FirstOrDefault();
                GameObject instance = Instantiate(Resources.Load("Prefab/" + result) as GameObject);

                //string animationName =
                //    (
                //        from anim in combinationList
                //        where anim.Result == result
                //        select anim.AnimationName
                //    ).FirstOrDefault();

                //if (!String.IsNullOrEmpty(animationName))
                //{
                //    instance.GetComponent<Animator>().Play(animationName);
                //}
                Debug.Log("combination sucess");
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
        foreach (Combination data in combinationList)
        {
            //Debug.Log(data.LevelID);
            Debug.Log(data.FirstItem);
            Debug.Log(data.SecondItem);
            Debug.Log(data.Result);
        }
    }

    #endregion

    public bool GetPause()
    {
        return paused;
    }

    public void SetPause (bool pauseValue)
    {
        paused = pauseValue;
    }
}
