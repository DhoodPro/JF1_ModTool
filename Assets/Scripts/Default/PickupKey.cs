using UnityEngine;

public class PickupKey : MonoBehaviour
{
    public GameObject Charger;
    public Transform chargerSpawn;
    public static bool Activated;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameManager.GM.pickupKey();

            if (Charger && Activated == false)
            {
                Charger.transform.position = chargerSpawn.position;
                Charger.transform.rotation = chargerSpawn.rotation;
                Activated = true;
                Charger.SetActive(true);
            }

            Destroy(transform.parent.gameObject);
        }
    }
}
