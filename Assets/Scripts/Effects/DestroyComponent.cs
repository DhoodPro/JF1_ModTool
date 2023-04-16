using UnityEngine;

public class DestroyComponent : MonoBehaviour
{
    public Behaviour com;

    private void Start()
    {
        Destroy(com);
        Destroy(this);
    }
}
