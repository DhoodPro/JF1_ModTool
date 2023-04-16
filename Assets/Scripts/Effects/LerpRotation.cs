using System.Collections;
using UnityEngine;

public class LerpRotation : MonoBehaviour
{
    public float angleToRotate;
    float value;
    public float increment = 0.01f;
    Vector3 euler;
    public int timer = 8;

    private void Start()
    {
        euler = transform.eulerAngles;
        euler.z += angleToRotate;
        StartCoroutine(deleteAfterSeconds());
    }
    IEnumerator deleteAfterSeconds()
    {
        yield return new WaitForSeconds(timer);
        Destroy(this);
    }

    private void Update()
    {
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, euler, value * Time.deltaTime);
        value += increment;
    }
}
