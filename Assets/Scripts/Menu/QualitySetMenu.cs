using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QualitySetMenu : MonoBehaviour
{
    public Transform sensitivityRead;
    Text getSensText;
    public Transform NegEnd;
    public Transform PosEnd;
    int quality;

    private void Start()
    {
        quality = PlayerPrefs.GetInt("Q", 3);
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
        PlayerPrefs.SetInt("Q", quality);
        QualitySettings.SetQualityLevel(quality - 1);
        PlayerPrefs.Save();
    }
}
