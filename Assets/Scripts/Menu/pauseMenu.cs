using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pauseMenu : MonoBehaviour
{
    public static pauseMenu pm;
    public GameObject pauseObj;
    public bool fading;

    private void Start()
    {
        if (pm != null) { return; }
        pm = this;
        DontDestroyOnLoad(gameObject);
        //StartCoroutine(pauseCheck());
    }
    IEnumerator pauseCheck()
    {
        while(true)
        {
            yield return new WaitForEndOfFrame();
            if (Input.GetButton("pause"))
            {
                enablePauseFunction(true);
                break;
            }
        }
    }

    public void enablePauseFunction(bool active)
    {
        if (active)
        {
            pauseObj.SetActive(true);
            StopAllCoroutines();
            Time.timeScale = 0;
            AudioListener.volume = 0;
        }
        else
        {
            pauseObj.SetActive(false);
            Time.timeScale = 1;

            if (fading == false)
            {
                StartCoroutine(pauseCheck());
                AudioListener.volume = PlayerPrefs.GetFloat("volume", 1);
            }
        }
    }
    
    public void startPauseFunction()
    {
        StartCoroutine(pauseCheck());
    }
}
