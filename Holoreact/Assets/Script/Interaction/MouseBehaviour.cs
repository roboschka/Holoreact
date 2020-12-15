using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseBehaviour : MonoBehaviour
{
    public Vector3 originPosition;
    private Vector3 snapPosition;

    GameObject hittedObject;
    public bool isWithinRange = false;

    private Vector3 mOffset;
    private float mZCoord;

    //Samuel 4 des 2020 - For testting
    private LayerMask experimentObjectLayer;

    GameManager gameManager;
    DescriptionManager descriptionManager;
    private void Awake()
    {
        descriptionManager = FindObjectOfType<DescriptionManager>() as DescriptionManager;
        isWithinRange = false;
        originPosition = this.gameObject.transform.position;
        gameManager = FindObjectOfType<GameManager>() as GameManager;
        experimentObjectLayer = LayerMask.GetMask("ExperimentObject");
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mZCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    private void OnMouseOver()
    {
        Debug.Log(descriptionManager);
        descriptionManager.ShowDescription(gameObject);
    }

    private void OnMouseExit()
    {
        descriptionManager.HideDescription(gameObject);
    }

    #region MouseDrag Handling
    private void OnMouseDown()
    {
        //Vector3 worldPosition;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitData;
        
    
        if (Physics.Raycast(ray, out hitData))
        {
            //hitData layer = "ExperimentObject"
            hittedObject = hitData.collider.gameObject;
            if (hitData.transform.gameObject.layer == 9)
            {
                mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
                mOffset = gameObject.transform.position - GetMouseWorldPos();
               // Debug.Log("9");
            }
            else if (hitData.transform.gameObject.layer == 10)
            {
                // Debug.Log("10");
                //string name = hitData.transform.gameObject.GetComponent<Collider>();
                //Debug.Log(name);
            }
        }
    }
    
    private void OnMouseDrag()
    {
        #region Commented
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //RaycastHit hitData;

        //if (Physics.Raycast(ray, out hitData))
        //{
        //if (hittedObject.layer == 9)
        //{
        //    isWithinRange = false;
        //    transform.position = GetMouseWorldPos() + mOffset;
        //}
        //else if (hitData.transform.gameObject.layer == 10)
        //{
        //Debug.Log("10");
        //}
        //}
        #endregion

        if (hittedObject.layer == 9)
        {
            //isWithinRange = false;
            transform.position = GetMouseWorldPos() + mOffset;
        }

    }

    private void OnMouseUp()
    {
        if (isWithinRange)
        {
            transform.position = snapPosition;
            Debug.Log("snapped within range " + snapPosition);

        } else
        {
            transform.position = originPosition;
            Debug.Log("masuk else");
            //buat testing navigation
            if (gameObject.name != "Plane")
            {
                //disable self
                gameObject.SetActive(false);
            }

            //Show current index
            gameManager.ShowCurrentIndexObject();
        }
    }

    #endregion

    #region Collision Handling
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Plane")
        {
            isWithinRange = true;
            snapPosition = collision.gameObject.transform.position;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == "Plane")
        {
            isWithinRange = false;
        }
    }
    #endregion
}
