using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDrag : MonoBehaviour
{
    [SerializeField]
    Vector3 originPosition;

    GameObject rayCastManager;
    public bool isWithinRange = false;

    private void Start()
    {
        isWithinRange = false;
        rayCastManager = GameObject.Find("RayCastManager");
        originPosition = this.gameObject.transform.position;
    }
    private Vector3 mOffset;
    private float mZCoord;

    private void OnMouseDown()
    {
        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        mOffset = gameObject.transform.position - GetMouseWorldPos();
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mZCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    private void OnMouseDrag()
    {
        isWithinRange = false;
        transform.position = GetMouseWorldPos() + mOffset;
        //GameObject currentlyHitting = rayCastManager.GetComponent<ClickPositionManager>().getHittedObject();
        //if (currentlyHitting.name == "Cube (1)")
        //{
        //    isWithinRange = true;
        //}
    }

    private void OnCollisionStay(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.name == "Plane")
        {
            isWithinRange = true;
        }
    }

    private void OnMouseUp()
    {
        if (isWithinRange)
        {
            Debug.Log("SNAP!");
        } else
        {
            transform.position = originPosition;
        }
    }
    
}
