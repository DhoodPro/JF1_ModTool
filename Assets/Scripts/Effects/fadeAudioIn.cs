using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fadeAudioIn : MonoBehaviour
{
    public AudioSource aud;
    public float increment = 0.01f;

    private void Start()
    {
        aud.volume = 0;
        aud.enabled = true;
        InvokeRepeating("raiseVol", 0, 0.1f);
    }

    void raiseVol()
    {
        if (aud.volume < 1)
        {
            aud.volume += increment;
        }
        else
        {
            Destroy(this);
        }
    }
}
