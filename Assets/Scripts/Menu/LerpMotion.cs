using UnityEngine;

public class LerpMotion : MonoBehaviour
{
    public Transform positionToFollow;
    public float increment = 0.01f;
    float t = 0;
    public bool rotationLerp;

	void Update ()
    {
        transform.position = Vector3.Lerp(transform.position, positionToFollow.position, t * Time.deltaTime);
        t += increment;

        if (rotationLerp)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, positionToFollow.rotation, t * Time.deltaTime);
        }

        if (transform.position == positionToFollow.position) Destroy(this);
	}
}
