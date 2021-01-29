using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseBehaviour : MonoBehaviour
{
    private Vector3 originPosition;
    private Vector3 snapPosition;

    private GameObject hittedObject;
    private bool isWithinRange = false;

    private Vector3 mOffset;
    private float mZCoord;

    private GameManager gameManager;
    private DescriptionManager descriptionManager;
    private void Awake()
    {
        descriptionManager = FindObjectOfType<DescriptionManager>() as DescriptionManager;
        isWithinRange = false;
        originPosition = this.gameObject.transform.position;
        gameManager = FindObjectOfType<GameManager>() as GameManager;
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mZCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    public void SetOriginPosition(Vector3 newPosition)
    {
        originPosition = newPosition;
    }

    #region MouseHover Handling
    private void OnMouseOver()
    {
        descriptionManager.ShowDescription(gameObject);
    }

    private void OnMouseExit()
    {
        descriptionManager.HideDescription(gameObject);
    }
    #endregion

    #region MouseDrag Handling
    private void OnMouseDown()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hitData))
        {
            hittedObject = hitData.collider.gameObject;
            if (hitData.transform.gameObject.layer == 9)
            {
                //menyamakan koordinat z mouse dengan koordinat z objek
                mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
                //offset antara posisi asli objek dengan posisi cursor dibuat menjadi 0
                mOffset = gameObject.transform.position - GetMouseWorldPos();
            }
        }
    }
    
    private void OnMouseDrag()
    {
        if (hittedObject.layer == 9)
        {
            //objek dipindahkan sesuai dengan posisi offset yang didapat
            transform.position = GetMouseWorldPos() + mOffset;
        }
    }

    private void OnMouseUp()
    {
        if (isWithinRange)
        {
            transform.position = snapPosition;
        }
        else
        {
            transform.position = originPosition;
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
            //Debug.Log(gameObject.GetComponent<Collider>().bounds.size.y / 2);
            //snapPosition.y += (gameObject.transform.localScale.y / 2);
            snapPosition.y += (gameObject.GetComponent<Collider>().bounds.size.y / 2);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == "Plane")
        {
            isWithinRange = false;
            Debug.Log(gameObject.name + " is out of plane");
        }
    }
    #endregion
}
