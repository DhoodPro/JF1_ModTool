using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarElectricFail : MonoBehaviour
{
    float originalLightIntensity = 0;
    float originalRadioPitch = 0;

    public Light[] lights;
    public AudioSource radio;
    public int minimumOccurance = 15;
    public int maximumOccurance = 20;
    public float duration = 5;

    private void Start()
    {
        originalRadioPitch = radio.pitch;
        StartCoroutine(startFail());
    }
    IEnumerator startFail()
    {
        yield return new WaitForSeconds(Random.Range(minimumOccurance, maximumOccurance));
        originalLightIntensity = lights[0].intensity;
        float halfRadioPitch = originalRadioPitch / 2;
        float halfIntensity = originalLightIntensity / 2;

        while (halfRadioPitch < radio.pitch - 0.05f)
        {
            yield return new WaitForEndOfFrame();
            radio.pitch = Mathf.Lerp(radio.pitch, halfRadioPitch, duration * Time.deltaTime);
            foreach(Light l in lights)
            {
                l.intensity = Mathf.Lerp(l.intensity, halfIntensity, duration * Time.deltaTime);
            }
        }

        //yield return new WaitForSeconds(1);
        
        while (radio.pitch < originalRadioPitch - 0.01f)
        {
            yield return new WaitForEndOfFrame();
            radio.pitch = Mathf.Lerp(radio.pitch, originalRadioPitch, duration * Time.deltaTime);
            foreach (Light l in lights)
            {
                l.intensity = Mathf.Lerp(l.intensity, originalLightIntensity, duration * Time.deltaTime);
            }
        }

        radio.pitch = originalRadioPitch;
        foreach (Light l in lights)
        {
            l.intensity = originalLightIntensity;
        }

        StartCoroutine(startFail());
    }
}
