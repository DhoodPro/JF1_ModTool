using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IfPlaystation : MonoBehaviour
{
    public GameObject playstationObj;
    public GameObject otherObj;

    void Start()
    {
#if UNITY_PS4
        otherObj.SetActive(false);
        playstationObj.SetActive(true);
#else
        otherObj.SetActive(true);
        playstationObj.SetActive(false);
#endif
    }

}
