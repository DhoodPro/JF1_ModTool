using UnityEngine;

public class RotateObj : MonoBehaviour
{
    public Vector3 rotate;

    private void Update()
    {
        transform.Rotate(rotate);
    }
}
