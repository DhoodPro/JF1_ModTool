using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StareCarKillerSetup : MonoBehaviour
{
    public CollideCheck[] collideChecks;
    public int timer = 45;
    public GameObject carKiller;
    GameObject car;
    GameManager GM;
    Transform pool;

    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        GM = GameManager.GM;
        yield return new WaitForSeconds(0.1f);
        GameObject g = new GameObject();
        g.name = "lightPool";
        carKiller.transform.parent = g.transform;
        while (GM.createKillerCar() == false)
        {
            yield return new WaitForSeconds(0.5f);
        }
        GameObject newKillerCar = Instantiate(carKiller);
        newKillerCar.transform.parent = g.transform;
        pool = g.transform;
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(spawnCar());
    }

    IEnumerator spawnCar()
    {
        yield return new WaitForSeconds(timer);
        while (car == null)
        {
            yield return new WaitForSeconds(1);

            CollideCheck cc = collideChecks[Random.Range(0, collideChecks.Length)];
            
            if (cc.colliding == false)
            {
                car = pool.GetChild(1).gameObject;
                car.transform.parent = null;
                car.transform.position = cc.transform.position;
                car.transform.rotation = cc.transform.rotation;
                StartCoroutine(createSpawn());
                car.SetActive(true);
                StartCoroutine(readCar());
            }
            
        }
    }
    IEnumerator readCar()
    {
        yield return new WaitForSeconds(1);
        while (car != null)
        {
            yield return new WaitForSeconds(1);
        }
        StartCoroutine(spawnCar());
    }

    IEnumerator createSpawn()
    {
        yield return new WaitForSeconds(0.5f);
        while (GM.createKillerCar() == false)
        {
            yield return new WaitForSeconds(0.5f);
        }
        GameObject newKillerCar = Instantiate(carKiller);
        newKillerCar.transform.parent = pool;
    }
}
