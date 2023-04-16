using System.Collections;
using UnityEngine;

public class carTurnOn : MonoBehaviour
{
    public Transform lerpCarSpot;
    Quaternion originalPos;
    public float timeBeforeStart = 4;
    public float timeOfCrank = 0.5f;
    public float lerpSpeed = 1;
    bool activate;
    bool started;
    public AudioSource carEngine;
    public AudioSource carCrank;
    public float carVolume = 0.6f;
    public bool sceneCar = true;
    public Behaviour[] behaviours;
    int z = 0;
    bool behaviorStarted = false;

	IEnumerator Start ()
    {
        originalPos = transform.rotation;
        yield return new WaitForSeconds(timeBeforeStart);
        activate = true;
        carCrank.Play();
        yield return new WaitForSeconds(timeOfCrank);
        started = true;
	}
	
	void Update ()
    {
        if (!activate) return;

        if (!started)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, lerpCarSpot.rotation, lerpSpeed * Time.deltaTime);
        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, originalPos, lerpSpeed * Time.deltaTime);

            if (sceneCar)
            {
                if (carEngine.volume < carVolume) carEngine.volume += 0.02f;
                if (transform.rotation == originalPos)
                {
                    Destroy(this);
                    Destroy(carCrank);
                }
            }
            else
            {
                if (!behaviorStarted) { StartCoroutine(behaviorSetup()); behaviorStarted = true; }
                if (transform.rotation != originalPos) transform.rotation = Quaternion.Lerp(transform.rotation, originalPos, lerpSpeed * Time.deltaTime);
            }
        }
	}

    IEnumerator behaviorSetup()
    {
        behaviours[z].enabled = true;
        yield return new WaitForSeconds(1);
        if (z < behaviours.Length)
        {
            StartCoroutine(behaviorSetup());
            z += 1;
        }
        else
        {
            Destroy(this);
        }

        if (carCrank) Destroy(carCrank);
    }
}
