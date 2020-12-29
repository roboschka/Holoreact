using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setting : MonoBehaviour
{
    [SerializeField]
    private GameObject turnOnSFX, turnOffSFX;
    private bool toggle;

    [SerializeField]
    private AudioSource source;

    [SerializeField]
    private AudioClip toggleSFX;

    // Start is called before the first frame update
    //void Start()
    //{
        
    //}

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            source.PlayOneShot(toggleSFX, 0.4f);
            ToggleSound();
        }
    }

    private void ToggleSound()
    {
        toggle = !toggle;

        if (toggle)
        {
            AudioListener.volume = 1f;
            Debug.Log("Audio active");
            turnOnSFX.SetActive(true);
            turnOffSFX.SetActive(false);
        } 
        else
        {
            Debug.Log("Audio inactive");
            AudioListener.volume = 0f;
            turnOnSFX.SetActive(false);
            turnOffSFX.SetActive(true);
        }
    }

}
