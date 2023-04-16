using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivateButton : MonoBehaviour
{
    public Button buttonToActivate;

    private void Start()
    {
        buttonToActivate.Select();
        Destroy(this);
    }
}
