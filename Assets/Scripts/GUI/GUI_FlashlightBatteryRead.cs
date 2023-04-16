using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUI_FlashlightBatteryRead : MonoBehaviour
{
    public RectTransform pivotPoint;

    private void OnEnable()
    {
        StartCoroutine(readBattery());
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator readBattery()
    {
        yield return new WaitForSeconds(1);
        pivotPoint.localScale = new Vector3(FlashLightBattery.power / 100f, 1, 1);
        StartCoroutine(readBattery());
    }
}
