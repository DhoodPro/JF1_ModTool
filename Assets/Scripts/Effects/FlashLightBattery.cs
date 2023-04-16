using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLightBattery : MonoBehaviour
{
    public static int power = 100;
    public float powerDepleteRate = 1;
    Light flashLight;
    float originalIntensity = 0;
    public GameObject flashlightReader;

    int powerLevel = 3;
    int readLevel = 3;

    bool batteryCheck;

    private void Start()
    {
        originalIntensity = flashLight.intensity;
        power = 100;
    }
    private void OnEnable()
    {
        power += -1;
        if (!flashLight) flashLight = GetComponent<Light>();
        checkPower();
        StartCoroutine(depleteBattery());
        flashlightReader.SetActive(true);
    }
    private void OnDisable()
    {
        StopAllCoroutines();
        batteryCheck = false;
        if (flashlightReader) flashlightReader.SetActive(false);
    }

    IEnumerator depleteBattery()
    {
        yield return new WaitForSeconds(powerDepleteRate);
        power += -1;
        checkPower();
        StartCoroutine(depleteBattery());
    }

    void checkPower()
    {
        if (power <= 0)
        {
            power = 0;
            flashLight.enabled = false;
        }
        else
        {
            flashLight.enabled = true;
        }

        //print(power);
    }
}
