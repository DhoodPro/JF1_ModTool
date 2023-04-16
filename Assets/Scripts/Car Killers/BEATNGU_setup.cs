using System.Collections;
using UnityEngine;

public class BEATNGU_setup : MonoBehaviour
{
    public CollideCheck cc;
    public GameObject beatngu;
    GameObject car;

    private void OnEnable()
    {
        //if (car != null) print("already a car");
        StartCoroutine(startSearching());
    }
    IEnumerator startSearching()
    {
        yield return new WaitForSeconds(Random.Range(1f, 5f));
        if (cc.colliding == false)
        {
            GameObject bngu = Instantiate(beatngu);
            bngu.transform.position = cc.transform.position;
            bngu.transform.LookAt(cc.transform.parent.position);
            bngu.SetActive(true);
            car = bngu;
            enabled = false;
        }
        else
        {
            StartCoroutine(startSearching());
        }
    }
}
