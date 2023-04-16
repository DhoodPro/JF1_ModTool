using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadToNextScene : MonoBehaviour
{
    public static string SceneToLoadTo = "MainMenu";
    public Image fadeImg;
    public RawImage loadSymbol;
    public Texture[] loadSymbols;
    int x = 0;

    private void Start()
    {
        StartCoroutine(loadProcess());
        StartCoroutine(loadSymbolProcess());
    }

    IEnumerator loadProcess()
    {
        yield return new WaitForSeconds(1);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneToLoadTo);
        asyncLoad.allowSceneActivation = false;

        // Wait until the asynchronous scene fully loads
        while (asyncLoad.progress < 0.9f)
        {
            print(asyncLoad.progress);
            yield return new WaitForSeconds(0.5f);
        }

        while (fadeImg.color.a < 1)
        {
            yield return new WaitForEndOfFrame();
            Color c = fadeImg.color;
            c.a += 0.01f;
            fadeImg.color = c;
        }

        DontDestroyOnLoad(gameObject);
        for(var i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name != "fadeImg")
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }

        asyncLoad.allowSceneActivation = true;

        while (fadeImg.color.a > 0)
        {
            yield return new WaitForEndOfFrame();
            Color c = fadeImg.color;
            c.a += -0.01f;
            fadeImg.color = c;
        }

        Destroy(gameObject);
    }

    IEnumerator loadSymbolProcess()
    {
        yield return new WaitForSeconds(1);
        x += 1;
        if (x >= loadSymbols.Length)
        {
            x = 0;
        }

        loadSymbol.texture = loadSymbols[x];
        StartCoroutine(loadSymbolProcess());
    }
}
