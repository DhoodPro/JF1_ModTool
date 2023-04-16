using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public Light lightManipulate;
    public float minSwitch = 1;
    public float maxSwitch = 4;

    public Color[] colorSwitch;
    AudioSource aud;
    MeshRenderer mr;

    private void Start()
    {
        aud = GetComponent<AudioSource>();
        mr = GetComponent<MeshRenderer>();
        StartCoroutine(switchColor());
    }

    IEnumerator switchColor()
    {
        yield return new WaitForSeconds(Random.Range(minSwitch, maxSwitch));
        lightManipulate.enabled = false;
        aud.volume = 0;
        mr.materials[1].color = Color.grey;
        yield return new WaitForSeconds(0.1f);
        aud.volume = 1;
        lightManipulate.enabled = true;
        switchColorFunc();
        StartCoroutine(switchColor());
    }

    void switchColorFunc()
    {
        int i = Random.Range(0, colorSwitch.Length);
        Color c = colorSwitch[i];
        mr.materials[1].color = c;
        lightManipulate.color = c;
    }
}
