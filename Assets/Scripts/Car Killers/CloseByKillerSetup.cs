using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseByKillerSetup : MonoBehaviour
{
    public float distanceSpot = 50;
    public CarKiller car;
    public SCC_Audio aud;
    public GameObject lights;
    public Transform player;
    bool search;
    GameManager GM;
    Health h;

    IEnumerator Start()
    {
        StartCoroutine(checkUser());
        yield return new WaitForEndOfFrame();
        GM = GameManager.GM;
        yield return new WaitForEndOfFrame();
        h = Health.health;
    }

    IEnumerator checkUser()
    {
        yield return new WaitForSeconds(1);
        float getDis = Vector3.Distance(transform.position, player.position);

        if (getDis < 50)
        {
            RaycastHit rc;
            Physics.Linecast(transform.position, player.position, out rc);
            if (rc.collider.tag == "Player")
            {
                search = true;
            }
        }
        StartCoroutine(checkUser());
    }

    private void Update()
    {
        if (!search) return;

        float getDis = Vector3.Distance(transform.position, player.position);

        if (getDis < distanceSpot)
        {
            car.enabled = true;
            aud.enabled = true;
            lights.SetActive(true);
            GM.InitiateChase(true, 4);
            h.addCar(transform);
            Destroy(this);
        }
    }
}
