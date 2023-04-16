using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOnLights : MonoBehaviour
{
    public float timer = 5;
    List<Light> lights = new List<Light>();
    public float lightIncrement = 0.1f;
    public float brightness = 4;

    bool activate;

    IEnumerator Start()
    {
        foreach(Transform t in transform)
        {
            if (t.GetComponent<Light>()) lights.Add(t.GetComponent<Light>());
        }
        yield return new WaitForSeconds(timer);
        activate = true;
    }

    private void Update()
    {
        if (!activate) return;

        foreach(Light l in lights)
        {
            if (l.intensity >= brightness) Destroy(this);
            else l.intensity += lightIncrement;
        }
    }
}
