using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineActivate : MonoBehaviour
{
    public Texture[] machineTexturesInOrder;
    public float timeBeforeStart = 0.5f;
    public float switchText = 0.2f;
    Material getMat;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(timeBeforeStart);
        getMat = GetComponent<MeshRenderer>().material;
        int i = 0;

        while (getMat.mainTexture != machineTexturesInOrder[machineTexturesInOrder.Length - 1])
        {
            yield return new WaitForSeconds(switchText);
            i++;
            getMat.mainTexture = machineTexturesInOrder[i];
        }
    }
}
