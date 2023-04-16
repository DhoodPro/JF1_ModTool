//using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToSceneOnTrigger : MonoBehaviour
{
    public int levelSave = 2;
    public bool save = true;
    bool checkBool = false;

    bool triggered;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        if (other.tag == "Player")
        {
            Transition.TransitionToScene("Intro" + levelSave);
            if (save)
            {
                PlayerPrefs.SetInt("level", levelSave);
                PlayerPrefs.Save();
                string s = "lvl" + (levelSave - 1);
                //Debug.Log(s);
                Achievement.Set(s);
            }
            StartCoroutine(reduceAudioListener());
            triggered = true;
        }
    }

    IEnumerator reduceAudioListener()
    {
        yield return new WaitForEndOfFrame();
        if (AudioListener.volume > 0)
        {
            AudioListener.volume += -0.03f;
            StartCoroutine(reduceAudioListener());
        }
    }
}
