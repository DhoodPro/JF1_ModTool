using UnityEngine;

public class SwitchLevelPresetsOnEnter : MonoBehaviour
{
    public GameObject TargetLevel;
    public GameObject previousLevel;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (TargetLevel.activeSelf != true)
            {
                TargetLevel.SetActive(true);
                previousLevel.SetActive(false);
            }
        }
    }
}
