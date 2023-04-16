using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GUI_StaminaRead : MonoBehaviour
{
    public static GUI_StaminaRead GSR;

    bool numeratorRunning;
    public RectTransform staminaBar;
    public Image staminaImg;
    FPC getFPC;

    private void Start()
    {
        GSR = this;
    }

    public void RunFunction(FPC fpc)
    {
        if (numeratorRunning) return;

        if (!getFPC) getFPC = fpc;
        numeratorRunning = true;
        staminaBar.gameObject.SetActive(true);
        StartCoroutine(readStamina());
    }

    IEnumerator readStamina()
    {
        yield return new WaitForEndOfFrame();
        if (getFPC.sprintOn)
        {
            if (staminaImg.color.a < 1)
            {
                Color c = staminaImg.color;
                c.a += 0.01f;
                staminaImg.color = c;
            }
        }
        else
        {
            if (staminaImg.color.a > 0)
            {
                Color c = staminaImg.color;
                c.a += -0.01f;
                staminaImg.color = c;
            }
        }

        staminaBar.localScale = new Vector3(getFPC._stamina / 100f, 1, 1);

        if (staminaImg.color.a > 0)
        {
            StartCoroutine(readStamina());
        }
        else
        {
            numeratorRunning = false;
            staminaBar.gameObject.SetActive(false);
        }
    }
}
