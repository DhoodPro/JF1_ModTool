using System.Collections;
using UnityEngine;

public class SetupRandomColors : MonoBehaviour
{
    public Color[] randomColors;

    IEnumerator Start()
    {
        foreach(Transform t in transform)
        {
            yield return new WaitForEndOfFrame();
            MeshRenderer mr = t.GetComponent<MeshRenderer>();
            if (mr)
            {
                Color c = randomColors[Random.Range(0, randomColors.Length)];
                foreach(Material m in mr.materials)
                {
                    yield return new WaitForEndOfFrame();
                    m.color = c;
                }
            }
        }
        Destroy(this);
    }
}
