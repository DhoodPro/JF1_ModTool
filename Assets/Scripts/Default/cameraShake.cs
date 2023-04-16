using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraShake : MonoBehaviour
{
    public static cameraShake CS;
    bool ready;
    public float decrement = 0.01f;
    public float shakeEffect = 0.5f;
    float setShake;
    Vector3 originalCamSpot;

    private void Start()
    {
        originalCamSpot = transform.localPosition;
        ready = true;
        CS = this;
        enabled = false;
    }
    private void OnEnable()
    {
        setShake = shakeEffect;
        if (!ready) return;

        StartCoroutine(shake());
    }
    IEnumerator shake()
    {
        yield return new WaitForEndOfFrame();
        Vector3 vec = originalCamSpot;
        vec.x += Random.Range(-setShake, setShake);
        vec.y += Random.Range(-setShake, setShake);
        transform.localPosition = vec;
        if (setShake > 0)
        {
            setShake += -decrement;
            StartCoroutine(shake());
        }
        else
        {
            transform.localPosition = originalCamSpot;
            enabled = false;
        }
    }
}
