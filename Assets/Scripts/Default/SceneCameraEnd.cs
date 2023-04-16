using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneCameraEnd : MonoBehaviour
{
    [System.Serializable]
    public class ExplodeEffect
    {
        public float timer;
        public float effectSize = 10;
        public float decrement = 0.2f;
        public GameObject explodeObj;
    }

    public Vector3 speed;
    public Transform lerpRot;
    public ExplodeEffect[] explodeEffects;
    float origFOV;
    Camera cam;

    public AudioSource rainAud;
    public float timeBeforeReduceSound;

    IEnumerator Start()
    {
        cam = GetComponent<Camera>();
        origFOV = cam.fieldOfView;
        AudioListener.volume = 1;

        StartCoroutine(effectSet(explodeEffects, 0));

        yield return new WaitForSeconds(timeBeforeReduceSound);
        while (rainAud.volume > 0)
        {
            yield return new WaitForEndOfFrame();
            rainAud.volume += -0.01f;
        }

    }
    IEnumerator effectSet(ExplodeEffect[] effect, int x)
    {
        ExplodeEffect ee = effect[x];

        yield return new WaitForSeconds(ee.timer);
        if (ee.explodeObj) ee.explodeObj.SetActive(true);

        float effectSz = ee.effectSize;

        while (effectSz > 0)
        {
            yield return new WaitForEndOfFrame();
            transform.position += new Vector3(Random.Range(-effectSz, effectSz), Random.Range(-effectSz / 2, effectSz/1.5f), 0);
            effectSz += ee.decrement;
        }

        x += 1;

        if (x < effect.Length)
        {
            StartCoroutine(effectSet(effect, x));
        }
    }

    private void Update()
    {
        transform.position += speed;
        transform.rotation = Quaternion.Lerp(transform.rotation, lerpRot.rotation, 1 * Time.deltaTime);
    }
}
