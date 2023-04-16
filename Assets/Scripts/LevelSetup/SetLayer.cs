using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLayer : MonoBehaviour
{
    public string layerSet = "PostProcessing";

    private void Start()
    {
        gameObject.layer = LayerMask.NameToLayer(layerSet);
    }
}
