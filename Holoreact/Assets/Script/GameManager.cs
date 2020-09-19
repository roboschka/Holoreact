using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] tools;
    [SerializeField]
    private GameObject[] material;

    // Start is called before the first frame update
    void Start()
    {
        tools = GameObject.FindGameObjectsWithTag("Tools");
        material = GameObject.FindGameObjectsWithTag("Material");
    }

    private GameObject FindResult(GameObject tool, GameObject material)
    {

        //the logic will goes to here
        //find the result from array using linQ
        return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
