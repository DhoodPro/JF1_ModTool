using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endingSetPlayerPrefs : MonoBehaviour
{
    public bool normalEnding;

	void Start ()
    {
		if (normalEnding)
        {
            PlayerPrefs.SetInt("normalEnding", 1);
            PlayerPrefs.Save();
        }
        else
        {
            PlayerPrefs.SetInt("secretEnding", 1);
            PlayerPrefs.Save();
        }
        PlayerPrefs.SetInt("level", 5);
        PlayerPrefs.Save();
    }
}
