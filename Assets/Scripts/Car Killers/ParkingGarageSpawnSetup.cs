using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkingGarageSpawnSetup : MonoBehaviour
{
    [System.Serializable]
    public class SpawnSetup
    {
        public CollideCheck collideBox;
    }
    [System.Serializable]
    public class LevelProgression
    {
        public Transform keyHolder;
        public GameObject[] fiveProgressionAdditions;
    }

    public GameObject CarKillerObj;
    public Transform playerTransform;
    public SpawnSetup[] spawnSets;
    public LevelProgression LP;
    int originalKeys;
    GameObject created;

    IEnumerator Start()
    {
        foreach(SpawnSetup ss in spawnSets)
        {
            yield return new WaitForEndOfFrame();
            StartCoroutine(checkCollision(ss));
        }
        originalKeys = LP.keyHolder.childCount;
        yield return new WaitForEndOfFrame();
        StartCoroutine(keyCheck());
    }

    IEnumerator checkCollision(SpawnSetup spawnSetup)
    {
        yield return new WaitForSeconds(1);
        if (spawnSetup.collideBox.colliding)
        {
            if (created == null)
            {
                foreach(Transform t in spawnSetup.collideBox.transform)
                {
                    yield return new WaitForEndOfFrame();
                    Vector3 vec = playerTransform.TransformDirection(-Vector3.forward);
                    Vector3 player = playerTransform.position - t.position;

                    float v = Vector3.Dot(vec, player);
                    //steeringDirection = v;
                    //print(v + t.name);
                    if (v < 0)
                    {
                        created = Instantiate(CarKillerObj);
                        created.transform.position = t.position;
                        created.transform.rotation = t.rotation;
                        created.SetActive(true);
                        break;
                    }
                }
            }
        }
        StartCoroutine(checkCollision(spawnSetup));
    }
    IEnumerator keyCheck()
    {
        yield return new WaitForSeconds(1);

        if (LP.keyHolder.childCount != originalKeys)
        {
            originalKeys = LP.keyHolder.childCount;
            LP.fiveProgressionAdditions[4 - originalKeys].SetActive(true);
        }

        StartCoroutine(keyCheck());
    }
}
