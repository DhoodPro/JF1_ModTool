using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StareCarKiller : MonoBehaviour
{
    List<Light> lights = new List<Light>();
    LensFlare[] lensFlares = new LensFlare[2];
    public float increment = 0.0001f;
    public Transform player;
    public CarKiller carKiller;
    public SCC_Audio aud;
    GameManager GM;
    Health h;

    IEnumerator Start()
    {
        player = player.GetChild(0);
        foreach(Transform t in transform)
        {
            Light l = t.GetComponent<Light>();
            if (l != null)
            {
                lights.Add(l);
            }
        }
        yield return new WaitForEndOfFrame();
        GM = GameManager.GM;
        yield return new WaitForEndOfFrame();
        h = Health.health;
        int x = 0;
        foreach(Light l in lights)
        {
            if (l.transform.childCount > 0)
            {
                lensFlares[x] = l.transform.GetChild(0).GetComponent<LensFlare>();
                x++;
            }
        }
        StartCoroutine(UpdateLights());
    }
    IEnumerator UpdateLights()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            float dis = Vector3.Dot(player.forward, transform.position - player.position);
            bool isFar = Vector3.Distance(player.position, transform.position) > carKiller.SpotDistance;

            if (dis < 0)
            {
                if (lights[0].intensity < 4.2f)
                {
                    foreach (Light l in lights)
                    {
                        yield return new WaitForEndOfFrame();
                        l.intensity += increment * 4;
                    }
                    foreach(LensFlare lf in lensFlares)
                    {
                        lf.brightness += increment * 4;
                    }
                }
                else
                {
                    if (h.IsDead == false)
                    {
                        carKiller.enabled = true;
                        aud.enabled = true;
                        GM.InitiateChase(true, 3);
                        h.addCar(transform);
                    }
                    Destroy(this);
                }

                if (isFar)
                {
                    Destroy(transform.parent.gameObject);
                }
            }
            else
            {
                if (lights[0].intensity > 0)
                {
                    foreach (Light l in lights)
                    {
                        yield return new WaitForEndOfFrame();
                        l.intensity += -increment * 4;
                    }

                    foreach (LensFlare lf in lensFlares)
                    {
                        lf.brightness += -increment * 4;
                    }
                }
            }
        }
    }
}
