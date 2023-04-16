using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    AudioSource aud;
    AudioDistortionFilter adf;
    public AudioClip killAudio;
    public GameObject particleObj;

    float backwardCamShot = 5f;
    Transform cam;
    Vector3 originalLocalSpot;
    Transform lerpPoint;
    Animator getAnim;

    public static Health health;

    public AudioSource closeAud;
    public float distanceShake = 50;
    public float shakeSize = 1;

    public float healthState = 100;
    float originalState;
    bool heal;

    float check;
    float checkReset;
    bool running;

    public bool IsDead;

    //public GameObject hurtUI;
    AudioSource beatSound;
    RawImage hurtUI_img;
    bool _a;

    private void Start()
    {
        GameObject go = new GameObject();
        cam = transform.GetChild(0);
        originalLocalSpot = cam.localPosition;
        lerpPoint = transform.GetChild(2);
        aud = go.AddComponent<AudioSource>();
        adf = go.AddComponent<AudioDistortionFilter>();
        adf.distortionLevel = 0.8f;
        go.transform.parent = transform;
        health = this;
        originalState = healthState;
        getAnim = transform.GetChild(0).GetComponent<Animator>();
        //hurtUI_img = hurtUI.GetComponent<RawImage>();
        //beatSound = hurtUI.GetComponent<AudioSource>();
    }

    IEnumerator runTestCar(Transform carKiller)
    {
        //Debug.Log("coroutine running");
        yield return new WaitForEndOfFrame();
        if (carKiller == null)
        {
            closeAud.volume = 0;
            yield break;
        }
        else
        {
            float getCarDis = Vector3.Distance(transform.position, carKiller.position);
            if (getCarDis < distanceShake)
            {
                Vector3 newVec = originalLocalSpot;
                newVec.x += Random.Range(-shakeSize, shakeSize) * (distanceShake / getCarDis);
                newVec.y += Random.Range(-shakeSize, shakeSize) * (distanceShake / getCarDis);
                newVec.z += Random.Range(-shakeSize, shakeSize) * (distanceShake / getCarDis);

                if (check <= getCarDis)
                {
                    cam.localPosition = newVec;
                    closeAud.volume = 1.5f / getCarDis;
                }

                check = 1f / getCarDis;
                if (!running) StartCoroutine(runCheck());
            }
            else
            {
                if (check == 0)
                {
                    cam.localPosition = originalLocalSpot;
                    closeAud.volume = 0;
                }
            }

            if (!IsDead) StartCoroutine(runTestCar(carKiller));
            else
            {
                closeAud.volume = 0;
            }
        }
    }
    IEnumerator runCheck()
    {
        running = true;
        yield return new WaitForEndOfFrame();
        while (check != checkReset)
        {
            check = checkReset;
            yield return new WaitForSeconds(0.1f);
            if (check == checkReset)
            {
                check = 0;
                checkReset = 0;
                running = false;
                yield break;
            }
        }
    }

    public void addCar(Transform carKiller)
    {
        //Debug.Log("call Started");
        health.StartCoroutine(health.runTestCar(carKiller));
    }

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rg = collision.transform.GetComponent<Rigidbody>();
        if (!rg) return;

        if (collision.relativeVelocity.sqrMagnitude * rg.velocity.sqrMagnitude > 100)
        {
            dead();
            Physics.IgnoreCollision(GetComponent<Collider>(), collision.collider);
        }
    }
    public void dead()
    {
        IsDead = true;
        StopAllCoroutines();
        StartCoroutine(camMovement());
        GetComponent<FPC>().enabled = false;
        Rigidbody rg = GetComponent<Rigidbody>();
        rg.freezeRotation = false;
        rg.drag = 1;
        aud.clip = killAudio;
        aud.Play();
        closeAud.Stop();
        if (getAnim) getAnim.enabled = true;
        //hurtUI.SetActive(false);
        particleObj.SetActive(true);
        GameManager.GM.deactivateKillerTransforms();
    }

    public void healthHit(float decrement)
    {
        healthState += -decrement;
        if (healthState < 0)
        {
            dead();
            return;
        }

        if (!heal)
        {
            //hurtUI.SetActive(true);
            StartCoroutine(healing());
        }

        heal = true;
    }
    
    IEnumerator healing()
    {
        yield return new WaitForEndOfFrame();
        while (healthState < originalState)
        {
            float seconds = Mathf.Lerp(0.7f, 1.1f, healthState / originalState);
            float alpha = Mathf.Lerp(0.6f, 0f, healthState / originalState);
            float pitchGet = Mathf.Lerp(2f, 0.8f, healthState / originalState);

            Color c = hurtUI_img.color;
            c.a = alpha;
            hurtUI_img.color = c;

            beatSound.Play();
            beatSound.pitch = pitchGet;

            if(_a == false)
            {
                StartCoroutine(alphaChange());
            }

            yield return new WaitForSeconds(seconds);
            healthState += seconds - 0.1f;
        }

        //hurtUI.SetActive(false);
        heal = false;
    }
    IEnumerator alphaChange()
    {
        _a = true;
        while(_a)
        {
            yield return new WaitForEndOfFrame();
            Color c = hurtUI_img.color;
            c.a += -0.01f;
            hurtUI_img.color = c;

            if (c.a <= 0)
            {
                _a = false;
            }
        }
    }

    IEnumerator camMovement()
    {
        Vector3 vec = lerpPoint.localPosition;
        Quaternion lerpGet = lerpPoint.localRotation;
        while(vec != cam.localPosition || lerpGet != cam.localRotation)
        {
            yield return new WaitForEndOfFrame();
            cam.localPosition = Vector3.Lerp(cam.localPosition, vec, 5 * Time.deltaTime);
            cam.localRotation = Quaternion.Lerp(cam.localRotation, lerpGet, 5 * Time.deltaTime);
        }
    }
}
