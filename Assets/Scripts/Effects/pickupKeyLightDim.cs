using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickupKeyLightDim : MonoBehaviour
{
    public Color colorFadeProcess;
    int keys = 5;
    int currentKeys = 5;

    public MeshRenderer[] lightRends;
    public Light[] lights;

    void Start()
    {
        //keys = transform.childCount;
        StartCoroutine(checkChildCount());
    }
    IEnumerator checkChildCount()
    {
        yield return new WaitForSeconds(1);
        if (currentKeys != transform.childCount)
        {
            float f = ((float)transform.childCount / (float)keys);
            StartCoroutine(lerpColor(f, 0));
            currentKeys = transform.childCount;
            //print("starting process");
        }
        StartCoroutine(checkChildCount());
    }
    IEnumerator lerpColor(float targetAmount, int runtime)
    {
        yield return new WaitForEndOfFrame();
        //print(targetAmount);
        Color targetCol = Color.Lerp(colorFadeProcess, Color.white, targetAmount);
        Color currentCol = Color.white;

        foreach (MeshRenderer mr in lightRends)
        {
            yield return new WaitForEndOfFrame();
            mr.materials[1].color = Color.Lerp(mr.materials[1].color, targetCol, 5 * Time.deltaTime);
        }
        foreach(Light l in lights)
        {
            yield return new WaitForEndOfFrame();
            l.color = Color.Lerp(l.color, targetCol, 5 * Time.deltaTime);
            currentCol = l.color;
        }

        if (runtime < 20)
        {
            int x = runtime + 1;
            StartCoroutine(lerpColor(targetAmount, x));
            //print("changing..." + runtime);
        }
        else
        {
           //print("complete");
        }
    }
}
