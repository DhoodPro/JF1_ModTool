#if UNITY_ANDROID || UNITY_IOS || UNITY_TIZEN || UNITY_TVOS || UNITY_WEBGL || UNITY_WSA || UNITY_PS4 || UNITY_WII || UNITY_XBOXONE || UNITY_SWITCH
#define DISABLESTEAMWORKS
#endif

#if !DISABLESTEAMWORKS
//using Steamworks;
#endif

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class MenuManager : MonoBehaviour
{
    public List<Transform> menus = new List<Transform>();
    bool loadingNextRoom;
    public static bool PS4;
    Button currentButton = null;
    GameObject lastMenuActivated;
    
	void Start ()
    {
        SetPostProcessing();
        QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("Q", 3) - 1);
        float vol = Mathf.Clamp(PlayerPrefs.GetFloat("volume", 1), 0, 1);
        AudioListener.volume = vol;
        foreach (Transform t in transform)
        {
            if (t.tag == "menu") menus.Add(t);
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        GameManager.sensitivity = PlayerPrefs.GetFloat("sens", 5);

        if (PS4) StartCoroutine(tryFind());
    }
    private void OnDisable()
    {
        loadingNextRoom = false;
    }

    public void nextMenu(string menuName)
    {
        foreach (Transform t in menus)
        {
            if (t.gameObject.activeSelf && t.name != menuName)
            {
                lastMenuActivated = t.gameObject;
            }
            if (t.name == menuName)
            {
                t.gameObject.SetActive(true);
            }
            else
            {
                t.gameObject.SetActive(false);
            }
        }

        if (PS4) findButton();
    }
    public void fadeToNextRoom(string roomName)
    {
        if (loadingNextRoom) return;
        if (roomName == "")
        {
            roomName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            Transition.TransitionToScene(roomName);
            StartCoroutine(reduceAudioListener());
            loadingNextRoom = true;
            return;
        }

        if (Timer.tm)
        {
            Timer.tm.setup();
        }

        bool load = false;

        if (roomName.ToCharArray()[0].ToString() == "J")
        {
            load = true;
        }
        switch(roomName)
        {
            case "Intro10":
                load = true;
                break;
            case "Office":
                load = true;
                break;
            case "MainMenu":
                load = true;
                break;
        }

        if (load == false)
        {
            Transition.TransitionToScene(roomName);
            StartCoroutine(reduceAudioListener());
            loadingNextRoom = true;
        }
        else
        {
            Transition.TransitionToScene("Load");
            LoadToNextScene.SceneToLoadTo = roomName;
            StartCoroutine(reduceAudioListener());
            loadingNextRoom = true;
        }
    }
    public void loadNextLevel()
    {
        int getLevel = PlayerPrefs.GetInt("level", 1);

        string scene = "Intro" + getLevel;
        fadeToNextRoom(scene);
    }
    public void LeaveGame()
    {
        Application.Quit();
    }
    public void setNextLevel(int level)
    {
        PlayerPrefs.SetInt("level", level);
        PlayerPrefs.Save();
    }
    public void lastMenu()
    {
        foreach (Transform t in menus)
        {
            t.gameObject.SetActive(false);
        }
        lastMenuActivated.SetActive(true);

        if (PS4) findButton();
    }
    
    public void setRestartBool(bool set)
    {
        GameManager.restart = set;
    }

    void findButton()
    {
        currentButton = null;
            if (menus.Count != 0)
            {
                for (var i = 0; i < menus.Count; i++)
                {
                    bool foundButton = false;
                    Transform t = menus[i];

                    if (t.gameObject.activeSelf)
                    {
                        foreach (Transform tr in t)
                        {
                            if (tr.GetComponent<Button>())
                            {
                                currentButton = tr.GetComponent<Button>();
                                currentButton.Select();
                                foundButton = true;
                                break;
                            }
                        }
                    }

                    if (foundButton) break;
                }
            }
            else
            {
                foreach (Transform tr in transform)
                {
                if (tr.gameObject.activeSelf)
                {
                    if (tr.GetComponent<Button>())
                    {
                        currentButton = tr.GetComponent<Button>();
                        currentButton.Select();
                        break;
                    }
                }
                }

            }
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus == true) StartCoroutine(tryFind());
    }

    IEnumerator tryFind()
    {
        findButton();
        print("finding...");
        yield return new WaitForSeconds(0.5f);
        if (currentButton == null)
        {
            StartCoroutine(tryFind());
        }
        else
        {
            print("found : " + currentButton.name);
        }
    }
    IEnumerator reduceAudioListener()
    {
        yield return new WaitForEndOfFrame();
        if (AudioListener.volume > 0)
        {
            AudioListener.volume += -0.03f;
            StartCoroutine(reduceAudioListener());
        }
    }

    void SetPostProcessing()
    {
        float blm = PlayerPrefs.GetInt("Bloom", 3);
        float col = PlayerPrefs.GetInt("ColorIntensity", 3);
        Bloom bloomLayer;
        ColorGrading colorGrade;
        PostProcessVolume PPV = FindObjectOfType<PostProcessVolume>();
        if (PPV == null) return;

        if (PPV.profile.TryGetSettings(out bloomLayer))
        {
            bloomLayer.intensity.value = blm;
        }
        if (PPV.profile.TryGetSettings(out colorGrade))
        {
            colorGrade.toneCurveToeStrength.value = col / 10f;
        }
    }
}

public static class Achievement
{
    public static void Set(string achievement)
    {
#if !DISABLESTEAMWORKS
        //EditorText.text.text = "Setting Achievement...";

        //if (!SteamManager.Initialized) return;

        //SteamUserStats.SetAchievement(achievement);
        //SteamUserStats.StoreStats();

        //Debug.Log("this part did run");
        
#endif
        int x = 0;

        switch(achievement)
        {
            case "lvl1":
                x = 1;
                break;
            case "lvl2":
                x = 2;
                break;
            case "lvl3":
                x = 3;
                break;
            case "lvl4":
                x = 4;
                break;
            case "lvl5":
                x = 5;
                break;
            case "lvl6":
                x = 6;
                break;
            case "lvl9":
                x = 7;
                break;
            case "lvl20":
                x = 8;
                break;
            case "lvl30":
                x = 9;
                break;
            case "lvl40":
                x = 10;
                break;
            case "lvl50":
                x = 11;
                break;
            case "lvl60":
                x = 12;
                break;
        }
        x += -1;
#if UNITY_PS4
        TrophyManager.TM.enabled = true;
        TrophyManager.TM.UnlockTrophy(x);
#endif
    }
    public static bool Get(string achievement)
    {
#if !DISABLESTEAMWORKS
        //EditorText.text.text = "Getting Achievement...";

        //if (!SteamManager.Initialized) return false;

        bool get = false;
        //Debug.Log("running");
        //SteamUserStats.GetAchievement(achievement, out get);
        //Debug.Log(get);
        return get;

        //Debug.Log("this part did run");

#endif

        return false;
    }
}
