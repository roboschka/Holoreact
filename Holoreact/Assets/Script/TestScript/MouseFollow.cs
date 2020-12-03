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
    }


}