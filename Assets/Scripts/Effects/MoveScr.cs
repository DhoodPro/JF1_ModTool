using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScr : MonoBehaviour
{
    public Vector3 move;
    public Vector3 scale = Vector3.one;
    public Color color = Color.white;
    public float colorLerpSPeed = 5;
    public float scaleLerpSpeed = 5;

    MeshRenderer mr;

    private void Start()
    {
        mr = GetComponent<MeshRenderer>();
    }

    private void FixedUpdate()
    {
        transform.position += move;
        transform.localScale = Vector3.Lerp(transform.localScale, scale, scaleLerpSpeed * Time.deltaTime);
        if (mr) { mr.material.color = Color.Lerp(mr.material.color, color, colorLerpSPeed * Time.deltaTime); }
    }
}
