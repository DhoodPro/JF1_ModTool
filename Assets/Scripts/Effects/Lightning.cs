using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    public Vector2 randomTimer = new Vector2(10, 25);
    public AudioSource lightningAud;
    public AudioClip[] thunderSounds;
    public Vector2 delayPlayThunder = new Vector2(1, 5);
    //RenderSettings rs;
    public GameObject lightningImg;
    public bool UseFog;

    Color origFog;

    private void Start()
    {
        if (UseFog) origFog = RenderSettings.fogColor;
        StartCoroutine(lightning());
    }

    IEnumerator lightning()
    {
        yield return new WaitForSeconds(Random.Range(randomTimer.x, randomTimer.y));

        if (UseFog) RenderSettings.fogColor = Color.Lerp(origFog, Color.white, 0.1f);
        if (lightningImg) lightningImg.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        if (UseFog) RenderSettings.fogColor = origFog;
        if (lightningImg) lightningImg.SetActive(false);
        yield return new WaitForSeconds(Random.Range(delayPlayThunder.x, delayPlayThunder.y));
        lightningAud.clip = thunderSounds[Random.Range(0, thunderSounds.Length)];
        lightningAud.Play();

        StartCoroutine(lightning());
    }
}
