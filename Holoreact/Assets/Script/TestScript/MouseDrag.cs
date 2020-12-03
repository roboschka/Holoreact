using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDrag : MonoBehaviour
{
    [SerializeField]
    Vector3 originPosition;
    private void Start()
    {
        originPosition = this.gameObject.transform.position;
        Debug.Log(originPosition);
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
        transform.position = GetMouseWorldPos() + mOffset;
    }

    private void OnMouseUp()
    {
        transform.position = originPosition;
    }
}
