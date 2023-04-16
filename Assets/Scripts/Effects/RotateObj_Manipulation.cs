using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObj_Manipulation : MonoBehaviour
{
    [SerializeField]
    public RotateObj rot;

    public float timeBeforeAction = 5;
    public float decrementSpeed = 5;

    public Vector3 moveTransition;

    bool activate;

    void Start()
    {
        InvokeRepeating("decrementMove", timeBeforeAction, 0.1f);
    }

    void decrementMove()
    {
        rot.rotate = Vector3.Lerp(rot.rotate, moveTransition, decrementSpeed * Time.deltaTime);
    }
}
