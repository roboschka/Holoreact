using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitClick : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnMouseDown()
    {
        Debug.Log(name + "was clicked");
    }
}
