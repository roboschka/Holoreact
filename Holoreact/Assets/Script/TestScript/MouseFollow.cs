using UnityEngine;

public class MouseFollow : MonoBehaviour
{
    void Update()
    {
        Vector3 temp = Input.mousePosition;
        temp.z = 10f; // Set this to be the distance you want the object to be placed in front of the camera.
        this.transform.position = Camera.main.ScreenToWorldPoint(temp);
    }
}