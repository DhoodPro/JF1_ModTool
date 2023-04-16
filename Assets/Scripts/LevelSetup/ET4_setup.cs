using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ET4_setup : MonoBehaviour
{
    public Transform[] ET_locations;
    public GameObject ET4;
    public GameObject[] objsToEnable;
    public GameObject placeETnotify;
    public GameObject endOfPlacements;
    public GameObject unlockKillerCar;
    public Text textShowPlacements;
    public Transform player;
    public int minDis = 50;
    public float fogDensitySet = 0.025f;

    AudioSource aud;
    bool active;
    Transform activeET;

    private void Start()
    {
        aud = GetComponent<AudioSource>();
        StartCoroutine(measureDistance());
    }

    IEnumerator measureDistance()
    {
        yield return new WaitForEndOfFrame();
        bool inRange = false;
        bool setDown = false;

        foreach(Transform t in ET_locations)
        {
            if (t != null)
            {
                float getDis = Vector3.Distance(player.position, t.position);
                //Debug.Log(getDis);
                if (getDis < minDis)
                {
                    if (aud.isPlaying == false) aud.Play();

                    aud.volume = Mathf.Lerp(0, 1, 3 / getDis);
                    inRange = true;

                    if (aud.volume > 0.5f)
                    {
                        setDown = true;
                        active = true;
                        activeET = t;
                        placeETnotify.SetActive(true);
                    }
                }
                yield return new WaitForEndOfFrame();
            }
        }

        if (!inRange)
        {
            if (aud.isPlaying) aud.Stop();
        }
        if (!setDown)
        {
            active = false;
            activeET = null;
            placeETnotify.SetActive(false);
        }

        if (ET_locations.Length != 0) StartCoroutine(measureDistance());
        else
        {
            endOfPlacements.SetActive(true);
            placeETnotify.SetActive(false);
            StartCoroutine(lerpFog());
            StartCoroutine(spawnKillerCar());
        }
    }
    IEnumerator lerpFog()
    {
        yield return new WaitForEndOfFrame();
        RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity, fogDensitySet, 0.1f * Time.deltaTime);
        if (RenderSettings.fogDensity != fogDensitySet) StartCoroutine(lerpFog());
    }
    IEnumerator spawnKillerCar()
    {
        yield return new WaitForSeconds(3);
        unlockKillerCar.SetActive(true);
    }

    private void Update()
    {
        if (!active) return;

        if (Input.GetButtonUp("Fire1"))
        {
            GameObject go = Instantiate(ET4, activeET.position, ET4.transform.rotation);
            go.SetActive(true);
            active = false;
            List<Transform> newSet = new List<Transform>();
            foreach(Transform t in ET_locations)
            {
                if (t != activeET)
                {
                    newSet.Add(t);
                }
            }
            ET_locations = newSet.ToArray();
            Destroy(activeET.gameObject);
            int i = 4 - ET_locations.Length;
            textShowPlacements.text = i + " / 4 ET placements";
        }
    }
}
