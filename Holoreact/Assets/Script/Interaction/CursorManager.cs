using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;

    [SerializeField]
    private AudioSource source;

    [SerializeField]
    private AudioClip itemClick;

    private void Update()
    {
        if (!gameManager.GetPause())
        {
            Vector3 temp = Input.mousePosition;
            temp.z = 10f; // Set this to be the distance you want the object to be placed in front of the camera.
            this.transform.position = Camera.main.ScreenToWorldPoint(temp);

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                
                //The out keyword here is used to tell the compiler that the method will return a value in that parameter
                if (Physics.Raycast(ray, out RaycastHit hitData))
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
                    else if (hitData.collider.gameObject.name == "SubmitButton(Clone)") {
                        gameManager.Submit();
                    }
                    else if (hitData.collider.gameObject.layer == 9)
                    {
                        source.PlayOneShot(itemClick, 0.8f);
                    }
                }
            }
        }
        
    }
    
}