using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Fade : MonoBehaviour
{
    public Image img;
    public float speedOfFade = 0.05f;
    public float timeOfFade = 3;
    public string nextRoom = "none";

    public bool asyncToRoom;
    public GameObject LoadObject;
    public GameObject LoadingBar;


    bool fadeIn;

	IEnumerator Start ()
    {
        DontDestroyOnLoad(gameObject);
        AsyncOperation load = null;

        if (asyncToRoom)
        {
            LoadObject.SetActive(true);

            load = SceneManager.LoadSceneAsync(nextRoom);
            load.allowSceneActivation = false;

            while (load.progress <= 0.85f)
            {
                yield return new WaitForEndOfFrame();

                LoadingBar.transform.localScale = new Vector3(load.progress, LoadingBar.transform.localScale.y);
            }

            asyncToRoom = false;
            LoadingBar.transform.localScale = new Vector3(1, LoadingBar.transform.localScale.y);
            LoadObject.SetActive(false);
        }
        Time.timeScale = 1;
        if (pauseMenu.pm)
        {
            pauseMenu.pm.fading = true;
            pauseMenu.pm.enablePauseFunction(false);
        }

        yield return new WaitForSeconds(timeOfFade / 2);

        fadeIn = true;

        if (load != null) { load.allowSceneActivation = true; }
        else
        {
            if (nextRoom != "none") SceneManager.LoadScene(nextRoom);
        }

        yield return new WaitForSeconds(timeOfFade / 2);
        Destroy(gameObject);
    }
	
	void Update ()
    {
        if (asyncToRoom) return;

		if (fadeIn)
        {
            fadeFunction(-speedOfFade);
        }
        else
        {
            fadeFunction(speedOfFade);
        }
	}

    void fadeFunction(float increment)
    {
        Color imgCol = img.color;

        imgCol.a += increment;

        img.color = imgCol;

    }
}
