using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfNowhereSpawnSetup : MonoBehaviour
{
    public GameObject CarKillerObj;
    public Transform playerTransform;
    public CollideCheck[] spawnSets;
    public Transform keyHolder;
    public float secondSet = 4.5f;
    int originalKeys;
    GameObject created;
    public bool activateKiller;

    IEnumerator Start()
    {
        originalKeys = keyHolder.childCount;
        yield return new WaitForEndOfFrame();
        StartCoroutine(spawnKiller(secondSet));
    }

    IEnumerator spawnKiller(float seconds)
    {
        yield return new WaitForSeconds(seconds * keyHolder.childCount);

        if (created == null)
        {
            StartCoroutine(checkCollision(spawnSets[0]));
            yield return new WaitForEndOfFrame();
            StartCoroutine(checkCollision(spawnSets[1]));
        }
    }

    IEnumerator checkCollision(CollideCheck spawnSetup)
    {
        yield return new WaitForSeconds(1);
        if (!spawnSetup.colliding)
        {
            if (created == null)
            {

                Transform t = spawnSetup.transform;

                    yield return new WaitForEndOfFrame();
                    Vector3 vec = playerTransform.TransformDirection(Vector3.forward);
                    Vector3 player = playerTransform.position - t.position;

                    float v = Vector3.Dot(vec, player);
                    //steeringDirection = v;
                    //print(v + t.name);
                    if (v > 0)
                    {
                        created = Instantiate(CarKillerObj, t.position, Quaternion.identity);
                        //created.transform.position = t.position;
                        created.transform.rotation = t.rotation;
                        created.SetActive(activateKiller);
                        StartCoroutine(checkCreatedDestroyed());
                    }
            }
        }

        if (created == null) StartCoroutine(checkCollision(spawnSetup));
    }

    IEnumerator checkCreatedDestroyed()
    {
        while(created != null)
        {
            yield return new WaitForSeconds(1);
        }
        StartCoroutine(spawnKiller(secondSet));
    }
}
