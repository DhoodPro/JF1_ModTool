using UnityEngine;

public class EnableOnPlayerPref : MonoBehaviour
{
    public GameObject playerprefTrue;
    public GameObject playerprefFalse;

    public string playerprefRead = "secretEnding";
    public string bonusPrefCheck = "";

    private void Start()
    {
        bool check = PlayerPrefs.GetInt(playerprefRead, 0) > 0 ? true : false;
        if (check == false)
        {
            bool check2 = PlayerPrefs.GetInt(bonusPrefCheck, 0) > 0 ? true : false;
            check = check2;
        }

        if (check)
        {
            playerprefTrue.SetActive(true);
        }
        else
        {
            playerprefFalse.SetActive(true);
        }
    }
}
