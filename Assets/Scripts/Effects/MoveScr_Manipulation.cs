using System.Collections;
using UnityEngine;

public class MoveScr_Manipulation : MonoBehaviour
{
    public MoveScr move;

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
        move.move = Vector3.Lerp(move.move, moveTransition, decrementSpeed * Time.deltaTime);
    }
}
