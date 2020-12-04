using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDrag : MonoBehaviour
{
    [SerializeField]
    Vector3 originPosition, snapPosition;

    GameObject hittedObject;
    public bool isWithinRange = false;

    private Vector3 mOffset;
    private float mZCoord;

    //Samuel 4 des 2020 - For testting
    private LayerMask experimentObjectLayer;


    private void Start()
    {
        isWithinRange = false;
        originPosition = this.gameObject.transform.position;

        experimentObjectLayer = LayerMask.GetMask("ExperimentObject");
    }

    private void Update()
    {
        
    }
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

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mZCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);
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
            isWithinRange = false;
            transform.position = GetMouseWorldPos() + mOffset;
        }

    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.name == "Plane")
        {
            isWithinRange = true;
            snapPosition = collision.gameObject.transform.position;
            //Debug.Log("dragged object is on collision with plane");
        }
    }

    private void OnMouseUp()
    {
        if (isWithinRange)
        {
            transform.position = snapPosition;
           // Debug.Log("snapped within range");
        } else
        {
            transform.position = originPosition;
           // Debug.Log("snapped now within range");
        }
    }
    
}
