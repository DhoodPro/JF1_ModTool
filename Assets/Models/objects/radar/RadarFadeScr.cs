using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarFadeScr : MonoBehaviour
{
    public int speed = 5;
    public int fov = 60;
    Camera cam;

    public Transform ET_Parent;

    private void Start()
    {
        cam = GetComponent<Camera>();
        StartCoroutine(runFOV());
        StartCoroutine(checkETs());
    }
    IEnumerator runFOV()
    {
        while (cam.fieldOfView != fov)
        {
            yield return new WaitForEndOfFrame();
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fov, speed * Time.deltaTime);
        }
    }
    IEnumerator checkETs()
    {
        yield return new WaitForSeconds(1);

        if (ET_Parent.childCount > 1)
        {
            StartCoroutine(checkETs());
        }
        else
        {
            StartCoroutine(exitFOV(0));
        }
    }
    IEnumerator exitFOV(float x)
    {
        float get = x;
        while (cam.fieldOfView != 0)
        {
            yield return new WaitForEndOfFrame();
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 0, get * Time.deltaTime);
            get += 0.1f;
        }
    }
}
