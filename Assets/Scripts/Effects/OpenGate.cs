using UnityEngine;

public class OpenGate : MonoBehaviour
{
    public Behaviour[] behaviorEnable;
    public bool overrideGate;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;

        int getKeys = GameManager.GM.addKeys;
        if (getKeys == 5 || overrideGate)
        {
            foreach (Behaviour b in behaviorEnable) b.enabled = true;
            Destroy(this);
        }
    }
}
