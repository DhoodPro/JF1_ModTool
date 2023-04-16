using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerManager : MonoBehaviour
{
    [System.Serializable]
    public class GUI_Var
    {
        public string variableName;
        public RawImage checkMark;
    }

    public static bool invertY;
    public static bool invertX;
    public Texture checkMarkUnchecked;
    public Texture checkMarkChecked;
    public GUI_Var[] Variables;

    private void Start()
    {
        invertY = (PlayerPrefs.GetInt("invertY", 0) > 0) ? true : false;
        invertX = (PlayerPrefs.GetInt("invertX", 0) > 0) ? true : false;

        setTextures();
    }

    public void ChangeVariable(string var)
    {
        int i = 0;
        switch (var)
        {
            case "invertY":
                invertY = !invertY;
                i = (invertY) ? 1 : 0;
                PlayerPrefs.SetInt("invertY", i);
                break;
            case "invertX":
                invertX = !invertX;
                i = (invertX) ? 1 : 0;
                PlayerPrefs.SetInt("invertX", i);
                break;
        }
        setTextures();
        PlayerPrefs.Save();
    }

    void setTextures()
    {
        foreach(GUI_Var v in Variables)
        {
            bool check = (PlayerPrefs.GetInt(v.variableName, 0) > 0) ? true : false;
            Texture ch = checkMarkUnchecked;
            if (check) { ch = checkMarkChecked; }
            v.checkMark.texture = ch;
        }
    }
}
