using System.Collections;
using UnityEngine;

public class DisableSpecificObject : MonoBehaviour
{
    [SerializeField] float timer = 0;
    public GameObject objToDisable;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(timer);
        objToDisable.SetActive(false);
    }
}
