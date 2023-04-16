using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioTurnOn : MonoBehaviour
{
    AudioSource aud;
    public float timer = 5;
    public float increment = 0.01f;
    public float pitchLimit = 1;
    public GameObject optionalGameobject;
    bool act;

	IEnumerator Start ()
    {
        aud = GetComponent<AudioSource>();
        yield return new WaitForSeconds(timer);
        aud.Play();
        act = true;
	}

	void Update ()
    {
        if (!act) return;
		if (aud.pitch < pitchLimit)
        {
            aud.pitch += increment;
        }
        else
        {
            if (optionalGameobject) optionalGameobject.SetActive(true);
            Destroy(this);
        }
	}
}
