using System.Collections;
using UnityEngine;

public class TransitionToSceneTrigger : MonoBehaviour
{
    public string sceneGoTo = "Assets/Scenes/Mod/level3.unity";

    bool triggered;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        if (other.tag == "Player")
        {
            Transition.TransitionToScene(sceneGoTo);
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
