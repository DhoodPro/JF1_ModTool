using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class trueEnding : MonoBehaviour
{
    public Text txt;
    public float secondsFBeforeSwicth = 8;
    public string sceneGoTo = "MainMenu";

    IEnumerator Start()
    {
        StartCoroutine(nextRoom());

        while(txt.color.a < 1)
        {
            yield return new WaitForSeconds(0.01f);
            Color c = txt.color;
            c.a += 0.01f;
            txt.color = c;
        }
    }

    IEnumerator nextRoom()
    {
        yield return new WaitForSeconds(secondsFBeforeSwicth);
        Transition.TransitionToScene(sceneGoTo);
        while (AudioListener.volume > 0)
        {
            yield return new WaitForEndOfFrame();
            AudioListener.volume += -0.1f;
        }
    }
}

