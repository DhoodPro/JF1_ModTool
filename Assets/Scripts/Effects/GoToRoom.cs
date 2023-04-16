using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToRoom : MonoBehaviour
{
    public float seconds;
    public string room;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(seconds);
        Transition.TransitionToScene(room);
    }
}
