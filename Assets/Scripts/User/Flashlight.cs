using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public Transform LightTransform;
    public Transform LightLerp;
    public Transform LightOriginal;

    bool lightOut;
    bool delay;

    Health h;

    IEnumerator Start()
    {
        while (h == null)
        {
            yield return new WaitForEndOfFrame();
            h = Health.health;
        }
    }

    private void FixedUpdate()
    {
        if (h)
        {
            if (h.IsDead)
            {
                StopAllCoroutines();
                LightTransform.gameObject.SetActive(false);
                enabled = false;
            }
        }

        if (Input.GetButton("flash") && !delay)
        {
            if (lightOut) { lightOut = false; }
            else { lightOut = true; }
            delay = true;
            StartCoroutine(activateLight(lightOut));
            //Debug.Log("run");
        }
    }

    IEnumerator activateLight(bool on)
    {
        if (on)
        {
            LightTransform.gameObject.SetActive(true);
            while (LightTransform.rotation != LightLerp.rotation)
            {
                yield return new WaitForEndOfFrame();
                LightTransform.rotation = Quaternion.RotateTowards(LightTransform.rotation, LightLerp.rotation, 5);

                //float getDis = Quaternion.Angle(LightTransform.rotation, lerpRot);
                //Debug.Log(getDis);
            }
        }
        else
        {
            while (LightTransform.rotation != LightOriginal.rotation)
            {
                yield return new WaitForEndOfFrame();
                LightTransform.rotation = Quaternion.RotateTowards(LightTransform.rotation, LightOriginal.rotation, 5);

                //float getDis = Quaternion.Angle(LightTransform.rotation, lerpRot);
                //Debug.Log(getDis);
            }
            LightTransform.gameObject.SetActive(false);
        }

        delay = false;
    }
}
