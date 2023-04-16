using UnityEngine;

public class MenuScroll : MonoBehaviour
{
    public Transform[] objsToMove;
    public Vector3 whereToMove;

    private void Update()
    {
        foreach(Transform t in objsToMove)
        {
            t.position += (whereToMove / 2);
        }
    }
}
