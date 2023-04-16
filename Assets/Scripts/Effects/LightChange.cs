using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightChange : MonoBehaviour
{
    public Color colorFadeProcess;

    public float speed = 5;

    public MeshRenderer[] lightRends;
    public MeshRenderer[] emissionRends;
    public Light[] lights;
    AudioSource lightNoise;

    public MeshRenderer motelLights;
    public GameObject particleObj;

    IEnumerator Start()
    {
        motelLights.materials[1].SetColor("_EmissionColor", Color.black);
        particleObj.SetActive(true);

        StartCoroutine(lightChange());
        yield return new WaitForEndOfFrame();
        StartCoroutine(lightRendChange());
        yield return new WaitForEndOfFrame();
        StartCoroutine(lightEmissionChange());

        lightNoise = GetComponent<AudioSource>();
        while (lightNoise.volume < 1)
        {
            yield return new WaitForSeconds(0.02f);
            lightNoise.volume += 0.01f;
        }
        yield return new WaitForSeconds(0.01f);
        while (lightNoise.volume > 0)
        {
            yield return new WaitForSeconds(0.02f);
            lightNoise.volume += -0.01f;
        }
    }
    IEnumerator lightChange()
    {
        while (lights[0].color != colorFadeProcess)
        {
            yield return new WaitForEndOfFrame();

            foreach(Light l in lights)
            {
                yield return new WaitForSeconds(0.01f);
                l.color = Color.Lerp(l.color, colorFadeProcess, speed * Time.deltaTime);
            }

            Color fogRender = Color.Lerp(Color.red, Color.black, 0.8f);

            RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, fogRender, speed * Time.deltaTime);
        }
    }
    IEnumerator lightRendChange()
    {
        while (lightRends[0].materials[1].color != colorFadeProcess)
        {
            yield return new WaitForEndOfFrame();

            foreach (MeshRenderer mr in lightRends)
            {
                yield return new WaitForSeconds(0.01f);
                mr.materials[1].color = Color.Lerp(mr.materials[1].color, colorFadeProcess, speed * Time.deltaTime);
            }
        }
    }
    IEnumerator lightEmissionChange()
    {
        while (emissionRends[0].materials[1].GetColor("_EmissionColor") != colorFadeProcess)
        {
            yield return new WaitForEndOfFrame();

            foreach (MeshRenderer mr in emissionRends)
            {
                yield return new WaitForSeconds(0.01f);
                mr.materials[0].EnableKeyword("_EMISSION");

                Color set = Color.Lerp(mr.materials[0].GetColor("_EmissionColor"), colorFadeProcess, speed * Time.deltaTime);
                print(set);
                mr.materials[0].SetColor("_EmissionColor", new Vector4(set.r, set.g, set.b, set.a));
                mr.materials[0].color = set;
            }
        }
    }
}
