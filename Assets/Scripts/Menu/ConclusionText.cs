using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConclusionText : MonoBehaviour
{
    Text txt;
    public string[] paragraphs;
    AudioSource aud;
    public float updateTimeType = 0.1f;
    public int pauseTime = 3;
    public GameObject optionalActivate;
    public GameObject endOption;
    public bool allowSkip = true;
    Coroutine cor;

    IEnumerator Start()
    {
        AudioListener.volume = PlayerPrefs.GetFloat("volume", 1);
        yield return new WaitForSeconds(pauseTime);
        txt = GetComponent<Text>();
        txt.text = "";
        aud = GetComponent<AudioSource>();
        cor = StartCoroutine(type(paragraphs, 0));
        StartCoroutine(overrideSkip());
    }
    void linebreak()
    {
        txt.text += "\n" + "\n";
    }

    IEnumerator type(string[] set, int i)
    {
        char[] n = set[i].ToCharArray();
        int characterCount = 0;
        while (characterCount < n.Length)
        {
            txt.text += n[characterCount];
            characterCount += 1;
            if (characterCount < n.Length)
            {
                txt.text += n[characterCount];
                characterCount += 1;
            }
            if (aud.isPlaying == false)
            {
                //aud.pitch = Random.Range(0.8f, 1.15f);
                aud.Play();
            }
            //yield return new WaitForSeconds(updateTimeType);
            yield return new WaitForEndOfFrame();
        }
        i++;
        if(i >= set.Length)
        {
            if (optionalActivate) optionalActivate.SetActive(true);
        }
        yield return new WaitForSeconds(pauseTime);
        if (i < set.Length)
        {
            linebreak();
            cor = StartCoroutine(type(set, i));
        }
        else
        {
            if (endOption) endOption.SetActive(true);
            StopAllCoroutines();
        }
    }

    IEnumerator overrideSkip()
    {
        if (!allowSkip) { yield break; }
        while (true)
        {
            yield return new WaitForEndOfFrame();
            if (Input.GetButton("Jump"))
            {
                StopCoroutine(cor);
                break;
            }
        }

        yield return new WaitForEndOfFrame();

        int i = 0;
        txt.text = "";

        while(i < paragraphs.Length)
        {
            yield return new WaitForEndOfFrame();
            txt.text += paragraphs[i];
            linebreak();
            i++;
        }

        yield return new WaitForSeconds(1);

        if (endOption) { endOption.SetActive(true); }
    }
}
