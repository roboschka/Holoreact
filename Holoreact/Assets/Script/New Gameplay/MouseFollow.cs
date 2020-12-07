using UnityEngine;

public class MouseFollow : MonoBehaviour
{
    [SerializeField]
    GameManager gameManager;
    
    void Update()
    {
        //Debug.Log(gameManager.getPause());
        if (!gameManager.GetPause())
        {
            Vector3 temp = Input.mousePosition;
            temp.z = 10f; // Set this to be the distance you want the object to be placed in front of the camera.
            this.transform.position = Camera.main.ScreenToWorldPoint(temp);

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitData;

                if (Physics.Raycast(ray, out hitData))
                {
                    Debug.Log(hitData.collider.gameObject.name + " is clicked");
                    if (hitData.collider.gameObject.name == "Next")
                    {
                        gameManager.Move(2);
                    }
                    else if (hitData.collider.gameObject.name == "Prev")
                    {
                        gameManager.Move(-2);
                    }
                    else if (hitData.collider.gameObject.name == "Handbook(Clone)")
                    {
                        gameManager.ShowHandbook();
                    }
                }
            }
        }
        
    }
    
}