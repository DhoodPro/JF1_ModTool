using System.Collections;
using UnityEngine;

public class ActivateAfterSeconds : MonoBehaviour
{
    public GameObject obj;
    public float timer = 6;

	IEnumerator Start ()
    {
        yield return new WaitForSeconds(timer);
        obj.SetActive(true);
        Destroy(this);
	}

}
