using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Transition
{
    public static void TransitionToScene(string scene)
    {
        GameObject go = GameObject.Instantiate(Resources.Load<GameObject>("FadePrefab"));
        Fade fade = go.GetComponent<Fade>();

        fade.nextRoom = scene;
    }

    public static void TransitionToScene(string scene, bool asyncLoad)
    {
        GameObject go = GameObject.Instantiate(Resources.Load<GameObject>("FadePrefab"));
        Fade fade = go.GetComponent<Fade>();

        fade.nextRoom = scene;
        fade.asyncToRoom = asyncLoad;
    }

    public static void TransitionToScene(string scene, float speedOfFade, float timeOfFade, bool asyncLoad)
    {
        GameObject go = GameObject.Instantiate(Resources.Load<GameObject>("FadePrefab"));
        Fade fade = go.GetComponent<Fade>();

        fade.nextRoom = scene;
        fade.speedOfFade = speedOfFade;
        fade.timeOfFade = timeOfFade;

        fade.asyncToRoom = asyncLoad;
    }
}
