using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GradientMenu : MonoBehaviour
{
    public CustomGradient myGradient;
    public List<Color> colors;

    public List<GradientColorKey> colorKeys;
    public List<GradientAlphaKey> alphaKeys;

    private void Start()
    {
        colorKeys = new List<GradientColorKey>();
        alphaKeys = new List<GradientAlphaKey>();

        Renderer renderer = gameObject.GetComponent<Renderer>();

        //renderer.material.SetTexture("_MainTex", myGradient.GetTexture((int)renderer.bounds.size.x));

        //renderer.material.SetTexture("_BumpMap", myGradient.GetTexture((int)renderer.bounds.size.x));

        //renderer.material.SetColor("_Color", myGradient.GradientResult);

        //renderer.material.SetColor("_Color", Color.red);

        //renderer.material.mainTexture = myGradient.GetTexture((int)renderer.bounds.size.x);

        //colors = new List<Color>();

        //for(int i = 0; i < (int)renderer.bounds.size.x; i++)
        //{
        //    colors.Add(myGradient.Evaluate(i));
        //}

        //lastest sucess
        foreach (CustomGradient.ColourKey key in myGradient.keys)
        {
            colors.Add(key.Colour);
        }

        //renderer.material.SetColorArray("_Color", colors.ToArray());

        foreach (CustomGradient.ColourKey key in myGradient.keys)
        {
            colorKeys.Add(new GradientColorKey(key.Colour,key.Time));
        }

        foreach (CustomGradient.ColourKey key in myGradient.keys)
        {
            alphaKeys.Add(new GradientAlphaKey(1f, key.Time));
        }

        Gradient gradient;
        gradient = new Gradient();

        gradient.SetKeys(colorKeys.ToArray(),alphaKeys.ToArray());

    }

    private void Update()
    {

    }
}
