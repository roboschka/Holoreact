using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DescriptionManager : MonoBehaviour
{
    [SerializeField]
    private GameObject descriptionPanel;
    [SerializeField]
    private GameManager gameManager;

    public void ShowDescription(GameObject hoveredObject)
    {
        if (hoveredObject.layer == 9)
        {
            descriptionPanel.SetActive(true);
            descriptionPanel.GetComponentInChildren<TextMeshProUGUI>().text = gameManager.GetItemDescription(hoveredObject.name.Replace("(Clone)", ""));
        }

    }

    public void HideDescription(GameObject hoveredObject)
    {
        descriptionPanel.SetActive(false);
    }
}
