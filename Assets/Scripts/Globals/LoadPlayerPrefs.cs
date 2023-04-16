using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadPlayerPrefs : MonoBehaviour
{
    public bool[] ca = { false, false, false, false, false };
    public GameObject locked;
    public GameObject unlocked;

    public static bool PS4;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(1);
        ca[0] = getPref("ca1");
        ca[1] = getPref("ca2");
        ca[2] = getPref("ca3");
        ca[3] = getPref("ca4");
        ca[4] = getPref("ca5");

        for (var i = 0; i < 5; i++)
        {
            if (!ca[i])
            {
                string n = (i + 1) + "";
                string get = ("lvl" + (10 + (10 * (i + 1))));
                ca[i] = Achievement.Get("lvl" + (10 + (10 * (i + 1))));
                //print(get);
                //print(ca[i]);
                yield return new WaitForSeconds(1);
                if (ca[i])
                {
                    if (!PS4) { PlayerPrefs.SetInt("ca" + n, 1); }
                }
            }
        }

        bool check = true;

        foreach(bool b in ca)
        {
            if (b == false)
            {
                check = false;
            }
        }

        if (check)
        {
            unlocked.SetActive(true);
        }
        else
        {
            locked.SetActive(true);
        }
    }

    bool getPref(string ca)
    {
        bool get = false;

        if (!PS4)
        {
            get = (PlayerPrefs.GetInt(ca, 0)) > 0 ? true : false;
        }

        return get;
    }
}
