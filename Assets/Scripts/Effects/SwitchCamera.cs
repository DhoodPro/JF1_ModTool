using System.Collections;
using UnityEngine;

public class SwitchCamera : MonoBehaviour
{
    public float timer = 5;
    public GameObject nextCamera;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(timer);
        nextCamera.SetActive(true);
        gameObject.SetActive(false);
    }
}
