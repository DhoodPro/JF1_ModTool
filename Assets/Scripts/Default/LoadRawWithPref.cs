using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadRawWithPref : MonoBehaviour
{
    public string playerPrefString = "ca";
    public Texture unlockText;
    RawImage img;

    private void Start()
    {
        img = GetComponent<RawImage>();

        if (PlayerPrefs.GetInt(playerPrefString, 0) > 0)
        {
            img.texture = unlockText;
        }
    }
}
