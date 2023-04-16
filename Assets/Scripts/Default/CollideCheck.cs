using UnityEngine;

public class CollideCheck : MonoBehaviour
{
    public bool colliding;

    private void OnTriggerStay(Collider other)
    {
        colliding = true;
    }
    private void OnTriggerExit(Collider other)
    {
        colliding = false;
    }
}
