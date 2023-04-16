using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collideWithJunk : MonoBehaviour
{
    SCC_Drivetrain drive;
    public ParticleSystem particles;
    ParticleSystem.MainModule pmm;
    AudioSource aud;

    private void Start()
    {
        drive = GetComponent<SCC_Drivetrain>();
        pmm = particles.main;
        aud = particles.gameObject.GetComponent<AudioSource>();
    }

    private void OnTriggerStay(Collider collision)
    {
        float getVar = drive.speed / 100;
        getVar = Mathf.Clamp(getVar, 0, 0.5f);
        pmm.startSize = getVar;
        aud.volume = getVar;

        if (drive.enabled == false)
        {
            aud.volume = 0;
            pmm.startSize = 0;
            enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        aud.volume = 0;
        pmm.startSize = 0;
    }
}
