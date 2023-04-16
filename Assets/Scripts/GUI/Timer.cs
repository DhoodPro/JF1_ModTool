using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public static Timer tm;

    public Text timerCounter;

    bool started;
    float millis = 0;
    float seconds = 0;
    float minutes = 0;
    float deltaTime = 0;

    private void OnEnable()
    {
        if (tm != null)
        {
            Destroy(tm.gameObject);
        }
        tm = this;
    }
    private void OnDisable()
    {
        tm = null;
    }

    public void setup()
    {
        DontDestroyOnLoad(gameObject);
    }
    public void triggerTimer()
    {
        if (!started)
        {
            StartCoroutine(count());
        }
        else
        {
            StopAllCoroutines();
        }

        started = !started;
    }

    public void toggle()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    IEnumerator count()
    {
        while(true)
        {
            yield return new WaitForEndOfFrame();
            deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
            float fps = 1.0f / deltaTime;
            while(Time.timeScale == 0)
            {
                yield return new WaitForEndOfFrame();
            }

            millis++;
            if (millis > fps)
            {
                seconds++;
                millis = 0;
            }
            if (seconds > 59)
            {
                minutes++;
                seconds = 0;
            }

            string ml = (millis < 10) ? "0" : "";
            string sc = (seconds < 10) ? "0" : "";
            string m = (minutes < 10) ? "0" : "";

            string ch = (millis < 100) ? millis.ToString() : Random.Range(11, 99).ToString();

            timerCounter.text = m + minutes + ":" + sc + seconds + ":" + ml + ch;
        }
    }
}
