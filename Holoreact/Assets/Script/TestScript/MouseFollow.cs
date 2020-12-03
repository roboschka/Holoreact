using UnityEngine;

public class MouseFollow : MonoBehaviour
{
    bool selected;
    GameObject hittedObject;
    private void Start()
    {
        selected = false;
    }
    void Update()
    {
        Vector3 temp = Input.mousePosition;
        temp.z = 10f; // Set this to be the distance you want the object to be placed in front of the camera.
        this.transform.position = Camera.main.ScreenToWorldPoint(temp);

        //Vector3 worldPosition;




        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //RaycastHit hitData;
        //if (Physics.Raycast(ray, out hitData, 1000))
        //{
        //    //raycast hit object
        //    worldPosition = hitData.point;
        //    hittedObject = hitData.collider.gameObject;
        //    if (Input.GetMouseButtonDown(0))
        //    {
        //        Debug.Log(hittedObject.name);
        //        selected = true;
        //    }

        //}

        //if (selected)
        //{
        //    Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        //    hittedObject.transform.position = this.transform.position;
        //}
    }


}