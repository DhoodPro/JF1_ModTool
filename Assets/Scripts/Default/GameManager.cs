using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class GameManager : MonoBehaviour
{
    public static GameManager GM;
    public static float sensitivity = 5;
    public static bool restart;
    public static bool carCreated;
    float chaseVolume;
    [SerializeField] KillerSpawnSetup KSS;
    GameObject currentCar;
    public Transform player;
    public Transform GameCanvas;
    bool firstCar;
    bool secondCar;
    bool thirdCar;
    bool fourthCar;
    bool fifthCar;
    public int addKeys;
    public bool playerDead;
    public int activatePlayerFPC = 5;
    public GameObject[] obsToEnableOnPlayerSet;
    public AudioSource audToEnableOnPlayerSet;
    public bool noManagerNeeded;

    [System.Serializable]
    private class KillerSpawnSetup
    {
        public Transform spawnPointHolder;
        public GameObject killerCar;
        public float update = 3;
    }

    IEnumerator Start()
    {
        SetPostProcessing();
        carCreated = false;
        GM = this;
        float vol = Mathf.Clamp(PlayerPrefs.GetFloat("volume", 1), 0, 1);
        float f = vol;
        StartCoroutine(raiseVolume(f));
        player.GetChild(0).gameObject.AddComponent<FlareLayer>();
        if (noManagerNeeded)
        {
            //gameObject.SetActive(false);
            yield break;
        }
        Cursor.visible = false;
        //print(KSS.killerCar);
        if (KSS.killerCar != null) StartCoroutine(spawnKiller(KSS));
        if (!restart)
        {
            yield return new WaitForSeconds(activatePlayerFPC);
        }
        Destroy(player.GetComponent<Animator>());
        //yield return new WaitForEndOfFrame();
        player.GetComponent<FPC>().enabled = true;
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        foreach(GameObject g in obsToEnableOnPlayerSet)
        {
            g.SetActive(true);
        }
        audToEnableOnPlayerSet.enabled = true;
    }

    public void InitiateChase(bool activation, int car)
    {
        if (car == 1)
        {
            firstCar = activation;
            if (!firstCar && KSS.killerCar != null)
            {
                StartCoroutine(spawnKiller(KSS));
            }
        }
        else if (car == 2)
        {
            secondCar = activation;
        }
        else if (car == 3)
        {
            thirdCar = activation;
        }
        else if (car == 4)
        {
            fourthCar = activation;
        }
        else if (car == 5)
        {
            fifthCar = activation;
            if (activation == false)
            {
                PickupKey.Activated = false;
            }
        }

        Transform n = null;
        foreach(Transform t in transform)
        {
            if (t.name == "Chase") n = t;
        }

        bool doubleCheck = activation;
        if (CheckActiveCars())
        {
            if (n.gameObject.activeSelf) doubleCheck = true;
        }
        else
        {
            doubleCheck = false;
        }

        n.gameObject.SetActive(doubleCheck);

        if (activation)
        {
            GameCanvas.Find("Caught").gameObject.SetActive(true);
        }
        
    }
    public void deactivateKillerTransforms()
    {
        InitiateChase(false, 1);
        InitiateChase(false, 2);
        InitiateChase(false, 3);
        InitiateChase(false, 4);
        InitiateChase(false, 5);
        GameObject[] killers = GameObject.FindGameObjectsWithTag("Killer");
        foreach (GameObject g in killers)
        {
            if (g.GetComponent<CarKiller>()) g.GetComponent<CarKiller>().targetDirection = null;
            if (g.GetComponent<CarKiller_ParkingGarage>()) g.GetComponent<CarKiller_ParkingGarage>().enabled = false;
            SCC_Drivetrain dr = g.GetComponent<SCC_Drivetrain>();
            dr.inputGas = 0;
            dr.inputSteering = 0;
            dr.inputHandbrake = 0.1f;
            //dr.enabled = false;
        }

        Transform t = null;
        foreach(Transform tl in transform)
        {
            if (tl.name == "Killed") t = tl;
        }

        t.gameObject.SetActive(true);
        StartCoroutine(activateRestartCanvas());
    }
    public void LostPlayer(int car)
    {
        InitiateChase(false, car);

        if (KSS.killerCar == null)
        {
            firstCar = false;
        }

        if (CheckActiveCars()) return;
        
        Transform t = findTransformChild("LostKiller");
        foreach (Transform tr in transform)
        {
            if (tr.name == "Chase") tr.gameObject.SetActive(false);
        }

        t.gameObject.SetActive(true);
    }
    public void pickupKey()
    {
        addKeys += 1;

        Transform t = findTransformChild("keyPickup");

        t.gameObject.SetActive(true);

        Transform kp = GameCanvas.Find("keyPopup");
        kp.GetComponent<UnityEngine.UI.Text>().text = addKeys + " / 5 keys collected";

        if (GetComponent<BEATNGU_setup>())
        {
            GetComponent<BEATNGU_setup>().enabled = true;
        }
    }

    public bool createKillerCar()
    {
        bool check = false;

        check = !carCreated;

        if (carCreated == false)
        {
            carCreated = true;
            StartCoroutine(carCooldown());
        }

        return check;
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

    Transform findTransformChild(string transformName)
    {
        Transform t = null;
        foreach (Transform tl in transform)
        {
            if (tl.name == transformName) t = tl;
        }

        return t;
    }
    bool CheckActiveCars()
    {
        if (firstCar)
        {
            return true;
        }
        if (secondCar)
        {
            return true;
        }
        if (thirdCar)
        {
            return true;
        }
        if (fourthCar)
        {
            return true;
        }
        if (fifthCar)
        {
            return true;
        }
        return false;
    }

    IEnumerator raiseVolume(float limit)
    {
        yield return new WaitForEndOfFrame();
        if (AudioListener.volume < limit)
        {
            AudioListener.volume += 0.01f;
            StartCoroutine(raiseVolume(limit));
        }
    }

    IEnumerator spawnKiller(KillerSpawnSetup kss)
    {
        yield return new WaitForSeconds(kss.update);

        Transform t = kss.spawnPointHolder.GetChild(Random.Range(0, kss.spawnPointHolder.childCount));
        /*
        RaycastHit rc;
        Physics.Linecast(t.position, player.position, out rc);

        Vector3 ang = player.position - transform.position;
        float dis = Vector3.Angle(ang, player.forward) * Time.deltaTime * 5;
        */
        float dis = Vector3.Dot(player.forward, t.position - player.position);
        //Debug.Log(dis);

        if (!currentCar)
        {
            if (dis < 0)
            {
                float getDistanceFromPlayer = Vector3.Distance(player.position, t.position);
                //print(getDistanceFromPlayer);
                if (getDistanceFromPlayer > 10) currentCar = Instantiate(kss.killerCar, t.position, t.rotation); currentCar.SetActive(true);
            }
            StartCoroutine(spawnKiller(kss));
        }
    }

    IEnumerator activateRestartCanvas()
    {
        playerDead = true;
        yield return new WaitForSeconds(5);
        GameCanvas.Find("LostCanv").gameObject.SetActive(true);
    }

    IEnumerator carCooldown()
    {
        yield return new WaitForSeconds(1);
        carCreated = false;
    }
}
