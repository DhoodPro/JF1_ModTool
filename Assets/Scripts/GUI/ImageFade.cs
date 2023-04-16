using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageFade : MonoBehaviour
{
    Image img;
    public float decrement = 0.01f;
    public float startAlpha = 0.15f;

    private void Start()
    {
        img = GetComponent<Image>();
    }
    private void OnEnable()
    {
        StartCoroutine(countDown());
    }
    private void OnDisable()
    {
        Color c = img.color;
        c.a = startAlpha;
        img.color = c;
    }
    IEnumerator countDown()
    {
        yield return new WaitForEndOfFrame();
        Color c = img.color;
        c.a += -decrement;
        img.color = c;
        if (img.color.a > 0)
        {
            StartCoroutine(countDown());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
