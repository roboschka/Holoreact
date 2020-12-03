using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickPositionManager : MonoBehaviour
{
    GameObject hittedObject, currentlyClicked;
    bool isSelected;

    private void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Vector3 clickPosition = Vector3.one;
        //    //Method 1: screentoworld point
        //    clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, 5f));
        //    Debug.Log(clickPosition);
        //}       

        Vector3 worldPosition;
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitData;
        if (Physics.Raycast(ray, out hitData, 1000))
        {
            //raycast hit object
            worldPosition = hitData.point;
            hittedObject = hitData.collider.gameObject;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
    }

    public GameObject getHittedObject()
    {
        return hittedObject;
    }
    
}
