using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class BloomSetMenu : MonoBehaviour
{
    public Transform sensitivityRead;
    Text getSensText;
    public Transform NegEnd;
    public Transform PosEnd;
    PostProcessVolume PPV;
    int quality;

    private void Start()
    {
        quality = PlayerPrefs.GetInt("Bloom", 3);
        Bloom bloomLayer;
        PPV = FindObjectOfType<PostProcessVolume>();
        if (PPV.profile.TryGetSettings(out bloomLayer))
        {
            bloomLayer.intensity.value = quality;
        }
        getSensText = sensitivityRead.GetChild(0).GetComponent<Text>();
        getSensText.text = quality.ToString();
        sensitivityRead.position = Vector2.Lerp(NegEnd.position, PosEnd.position, quality / 6f);
    }
    public void addSens(int add)
    {
        if (add > 0)
        {
            if (quality == 5)
            {
                return;
            }
        }
        if (add < 0)
        {
            if (quality == 1)
            {
                return;
            }
        }

        quality += add;
        getSensText.text = quality.ToString();
        float f = quality / 6f;
        sensitivityRead.position = Vector2.Lerp(NegEnd.position, PosEnd.position, f);
        PlayerPrefs.SetInt("Bloom", quality);
        Bloom bloomLayer;
        if (PPV.profile.TryGetSettings(out bloomLayer))
        {
            bloomLayer.intensity.value = quality;
        }
        PlayerPrefs.Save();
    }
}
