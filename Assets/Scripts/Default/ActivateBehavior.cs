using System.Collections;
using UnityEngine;

public class ActivateBehavior : MonoBehaviour
{
    public float timer = 6;
    public Behaviour behavior;

	IEnumerator Start ()
    {
        yield return new WaitForSeconds(timer);
        behavior.enabled = true;
        Destroy(this);
	}

}
