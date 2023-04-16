using System.Collections;
using UnityEngine;

public class DisableAfterSeconds : MonoBehaviour
{
    public float timeBeforeDisable = 3;

    private void OnEnable()
    {
        StartCoroutine(disable());
    }
    IEnumerator disable()
    {
        yield return new WaitForSeconds(timeBeforeDisable);
        gameObject.SetActive(false);
    }
}
