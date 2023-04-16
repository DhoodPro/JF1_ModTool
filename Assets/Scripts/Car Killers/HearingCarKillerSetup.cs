using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HearingCarKillerSetup : MonoBehaviour
{
    public int OccuranceInSeconds = 25;
    public float updateCheckAvailable = 1;
    public int waitTimeBeforeGone = 5;
    public float SpotDistance = 10;
    public AudioClip idleSound;
    //public AudioClip turnOffClip;
    AudioClip getStartup;
    public float timeBeforeSwitchClip = 1;

    AudioSource aud;
    GameObject killerCar;
    Vector3 spotToSpawn;
    Quaternion rot;
    Transform player;
    GameManager GM;
    Health h;
    Transform pool;

    public bool colliding;
    bool checkInput;

    IEnumerator Start()
    {
        aud = GetComponent<AudioSource>();
        getStartup = aud.clip;
        player = transform.parent;
        if (transform.childCount > 0)
        {
            killerCar = transform.GetChild(0).gameObject;
        }
        else
        {
            killerCar = GameObject.Find("CarKillers").transform.Find("SpawnBehind").GetChild(0).gameObject;
        }
        yield return new WaitForEndOfFrame();
        GM = GameManager.GM;
        yield return new WaitForEndOfFrame();
        h = Health.health;
        yield return new WaitForSeconds(0.3f);
        GameObject g = new GameObject();
        g.name = "flamePool";
        killerCar.transform.parent = g.transform;

        while (GM.createKillerCar() == false)
        {
            yield return new WaitForSeconds(0.5f);
        }
        GameObject newKillerCar = Instantiate(killerCar);
        newKillerCar.transform.parent = g.transform;
        pool = g.transform;
        yield return new WaitForSeconds(0.5f);

        StartCoroutine(spawnKiller());
    }
    IEnumerator spawnKiller()
    {
        //print("starting...");
        yield return new WaitForSeconds(OccuranceInSeconds);
        while (colliding == true)
        {
            yield return new WaitForSeconds(updateCheckAvailable);
        }
        if (GM.playerDead == true) { yield break; }
        aud.Play();
        spotToSpawn = transform.position;
        rot = transform.rotation;
        yield return new WaitForSeconds(timeBeforeSwitchClip);
        if (!colliding)
        {
            spotToSpawn = transform.position;
            rot = transform.rotation;
        }
        aud.clip = idleSound;
        aud.loop = true;
        aud.Play();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        checkInput = true;
        yield return new WaitForSeconds(waitTimeBeforeGone);
        checkInput = false;
        while(aud.volume > 0)
        {
            aud.volume += -0.02f;
            yield return new WaitForEndOfFrame();
        }
        aud.Stop();
        aud.loop = false;
        aud.volume = 1;
        yield return new WaitForSeconds(1);
        aud.clip = getStartup;
        StartCoroutine(spawnKiller());
    }
    IEnumerator SearchForUser(GameObject activeCar)
    {
        while (true)
        {
            yield return new WaitForSeconds(updateCheckAvailable);

            if (!activeCar)
            {
                StartCoroutine(spawnKiller());
                aud.clip = getStartup;
                yield break;
            }
        }
        //StartCoroutine(SearchForUser(activeCar));
    }

    IEnumerator createSpawn()
    {
        yield return new WaitForSeconds(0.5f);
        while (GM.createKillerCar() == false)
        {
            yield return new WaitForSeconds(0.5f);
        }
        GameObject newKillerCar = Instantiate(killerCar);
        newKillerCar.transform.parent = pool;
    }

    private void FixedUpdate()
    {
        if (checkInput)
        {
            if (Input.anyKey || Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0 || Input.GetAxis("HorizontalMouse") != 0)
            {
                GM.InitiateChase(true, 2);
                StopAllCoroutines();
                GameObject newCar = pool.GetChild(1).gameObject;
                newCar.transform.parent = null;
                newCar.transform.position = transform.position;
                newCar.transform.rotation = transform.rotation;
                StartCoroutine(SearchForUser(newCar));
                StartCoroutine(createSpawn());
                newCar.SetActive(true);
                h.addCar(newCar.transform);
                aud.Stop();
                checkInput = false;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        colliding = true;
    }
    private void OnTriggerExit(Collider other)
    {
        colliding = false;
    }
}
