using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCloseByKiller : MonoBehaviour
{
    public GameObject carKiller;
    public Transform spawnSet;
    public Transform player;
    public int distanceSpawn = 50;
    public int distanceDespawn = 120;
    GameObject car;
    Transform previousSet;
    GameManager GM;
    Transform pool;

    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        GM = GameManager.GM;
        yield return new WaitForSeconds(0.5f);
        GameObject g = new GameObject();
        g.name = "sparkPool";
        carKiller.transform.parent = g.transform;
        while(GM.createKillerCar() == false)
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
        yield return new WaitForSeconds(1);
        //Debug.Log("running");
        if (car == null)
        {
            Transform getRandom = spawnSet.GetChild(Random.Range(0, spawnSet.childCount));
            float dis = Vector3.Distance(player.position, getRandom.position);
            float getMin = distanceSpawn;
            //Debug.Log("dis");

            RaycastHit rc;
            Physics.Linecast(getRandom.position, player.position, out rc);
            if (rc.collider.tag != "Player") getMin = 0;

            if (dis > getMin && dis < distanceDespawn)
            {
                car = pool.GetChild(1).gameObject;
                car.transform.parent = null;
                car.transform.position = getRandom.position;
                car.transform.rotation = getRandom.rotation;
                StartCoroutine(createSpawn());
                car.SetActive(true);
                StartCoroutine(despawnCheck());
            }
            else
            {
                StartCoroutine(spawnCar());
            }
        }
        else
        {
            StartCoroutine(spawnCar());
        }
    }
    IEnumerator despawnCheck()
    {
        yield return new WaitForSeconds(1);
        if (car)
        {
            float dis = Vector3.Distance(player.position, car.transform.position);

            if (dis > distanceDespawn)
            {
                Destroy(car);
                StartCoroutine(spawnCar());
            }
            else
            {
                StartCoroutine(despawnCheck());
            }
        }
        else
        {
            StartCoroutine(spawnCar());
        }
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
