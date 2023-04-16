using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CA1_setup : MonoBehaviour
{
    public List<Transform> getKeys = new List<Transform>();
    public SCC_Drivetrain drive;
    public SCC_Audio aud;
    public AudioSource getCreepyAud;

    IEnumerator Start()
    {
        foreach(Transform t in transform)
        {
            getKeys.Add(t);
        }

        int calm = 1;
        float max = drive.maximumSpeed;
        float pitchMax = aud.maximumPitch;

        while(true)
        {
            yield return new WaitForSeconds(0.5f);
            //int x = 0;
            bool changeCalm = false;
            foreach(Transform t in getKeys)
            {
                if (t == null)
                { getKeys.Remove(t); changeCalm = true; break; }
                //x++;
            }
            //getKeys.Remove(getKeys[x]);
            if (changeCalm)
            {
                calm++;
            }

            drive.maximumSpeed = max * calm;
            aud.maximumPitch = pitchMax * calm;
            aud.minimumVolume = 0.15f * calm;
            float decrement = 0.1f * calm;
            getCreepyAud.pitch = 1.1f - decrement;
        }
    }

}
