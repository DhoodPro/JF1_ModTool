using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ET_collision : MonoBehaviour
{
    public bool Collided;

    private void OnCollisionEnter(Collision collision)
    {
        if (Collided) return;

        if (collision.transform.tag == "Killer")
        {
            Collided = true;
            transform.parent.GetChild(1).gameObject.SetActive(true);
            GetComponent<MeshRenderer>().material.mainTexture = GetComponent<MachineActivate>().machineTexturesInOrder[0];
            cameraShake.CS.enabled = true;
        }
    }
}
