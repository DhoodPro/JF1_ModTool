using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SensitivityManager : MonoBehaviour
{
    public Transform sensitivityRead;
    Text getSensText;
    public Transform NegEnd;
    public Transform PosEnd;
    float getSens;
    public bool sensitivity;
    public bool volume;

    private void Start()
    {
        if (sensitivity)
        {
            getSens = PlayerPrefs.GetFloat("sens", 5);
            getSensText = sensitivityRead.GetChild(0).GetComponent<Text>();
            getSensText.text = getSens.ToString();
            sensitivityRead.position = Vector2.Lerp(NegEnd.position, PosEnd.position, getSens / 10);
        }
        if (volume)
        {
            float vol = Mathf.Clamp(PlayerPrefs.GetFloat("volume", 1), 0, 1);
            getSens = vol * 10;
            getSensText = sensitivityRead.GetChild(0).GetComponent<Text>();
            getSensText.text = getSens.ToString();
            sensitivityRead.position = Vector2.Lerp(NegEnd.position, PosEnd.position, getSens / 10);
        }
    }
    public void addSens(int add)
    {
        if (add > 0)
        {
            if (getSens == 10)
            {
                return;
            }
        }
        if (add < 0)
        {
            if (getSens == 1)
            {
                return;
            }
        }
        if (sensitivity)
        {
            getSens += add;
            getSensText.text = getSens.ToString();
            sensitivityRead.position = Vector2.Lerp(NegEnd.position, PosEnd.position, getSens / 10);
            GameManager.sensitivity = getSens;
            PlayerPrefs.SetFloat("sens", getSens);
            PlayerPrefs.Save();
        }
        if (volume)
        {
            getSens += add;
            getSensText.text = getSens.ToString();
            sensitivityRead.position = Vector2.Lerp(NegEnd.position, PosEnd.position, getSens / 10);
            float f = getSens;
            if (f == 1) { f = 0; }
            if (f > 10) { f = 10; }
            else { f /= 10f; }
            PlayerPrefs.SetFloat("volume", f);
            PlayerPrefs.Save();
            AudioListener.volume = f;
        }
    }
}
