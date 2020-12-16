using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DescriptionManager : MonoBehaviour
{
    [SerializeField]
    private GameObject descriptionPanel;

    public void ShowDescription(GameObject hoveredObject)
    {
        if (hoveredObject.layer == 9)
        {
            descriptionPanel.SetActive(true);
        }
        
    }

    public void HideDescription(GameObject hoveredObject)
    {
        descriptionPanel.SetActive(false);
    }
}
