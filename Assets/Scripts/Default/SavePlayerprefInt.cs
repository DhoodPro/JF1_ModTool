using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePlayerprefInt : MonoBehaviour
{
    [SerializeField] string playerPrefSave;

    private void Start()
    {
        PlayerPrefs.SetInt(playerPrefSave, 1);
        PlayerPrefs.Save();
        Destroy(this);
    }
}
