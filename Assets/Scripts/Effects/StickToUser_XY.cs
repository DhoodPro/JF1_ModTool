using UnityEngine;

public class StickToUser_XY : MonoBehaviour
{
    public Transform player;
    public bool rotateAround;
    public int rotSpeed = 5;
	
	void FixedUpdate ()
    {
        Vector3 vec = player.transform.position;
        transform.position = new Vector3(vec.x, transform.position.y, vec.z);

        if (rotateAround)
        {
            transform.Rotate(Vector3.up * (rotSpeed * Time.deltaTime));
        }
	}
}
