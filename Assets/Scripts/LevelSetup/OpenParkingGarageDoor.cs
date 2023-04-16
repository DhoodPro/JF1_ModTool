using System.Collections;
using UnityEngine;

public class OpenParkingGarageDoor : MonoBehaviour
{
    public Animator garageDoor;
    public Collider colliderToDisable;
    public float timeBeforeDisable = 3;
    public bool overrideGate;
    bool entered;

    private void OnTriggerEnter(Collider other)
    {
        if (entered) return;
        if (other.tag != "Player") return;

        int getKeys = GameManager.GM.addKeys;
        if (getKeys == 5 || overrideGate)
        {
            garageDoor.enabled = true;
            entered = true;
            StartCoroutine(opening());
        }
    }

    IEnumerator opening()
    {
        yield return new WaitForSeconds(timeBeforeDisable);
        colliderToDisable.enabled = false;

        Destroy(this);
    }
}
