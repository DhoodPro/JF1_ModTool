using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorSound : MonoBehaviour
{
    public AudioSource aud;
    public AudioClip[] clips;

    public void playSound(int sound)
    {
        aud.clip = clips[sound];
        aud.Play();
    }
}
