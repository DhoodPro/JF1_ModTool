
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPC : MonoBehaviour
{
    public class CameraSend
    {
        public Transform cam;
        public Transform lerpPoint;
    }

    float vmotion;
    float hmotion;
    //public float accel = 0.2f;
    public float speed = 8;
    public float stamina = 100;
    [HideInInspector] public float _stamina;
    public float sensitivity = 2;
    public float jumpPower = 5;
    Rigidbody rg;
    float mult = 1;
    public float SprintMultiplier = 3;
    bool console;
    Transform characterCam;
    public float limitOfVerticalLook = 35;

    Vector3 lowRot;
    Vector3 highRot;
    float lookBackVar;
    float verticalLimit;
    bool tired;
    bool moving;

    public AudioSource walkSound;
    public bool sprintOn;
    
    public GameObject flashLight;
    bool buttonPress;

    GUI_StaminaRead gsr;
    Timer getTM;
    pauseMenu pm;

	void Start ()
    {
        ControllerManager.invertY = (PlayerPrefs.GetInt("invertY", 0) > 0) ? true : false;
        ControllerManager.invertX = (PlayerPrefs.GetInt("invertX", 0) > 0) ? true : false;
        rg = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        sensitivity = GameManager.sensitivity;
        //if (SystemInfo.deviceType == DeviceType.Console)
        //{
        //    sensitivity /= 3;
        //}
        _stamina = stamina;
        StartCoroutine(findStaminaRead());

        characterCam = transform.GetChild(0);
        lowRot = Vector3.Lerp(characterCam.localEulerAngles, -characterCam.right * limitOfVerticalLook, 1);
        highRot = Vector3.Lerp(characterCam.localEulerAngles, characterCam.right * limitOfVerticalLook, 1);

        if (!rg) Destroy(this);
	}
    private void OnEnable()
    {
        flashLight.SetActive(true);
        if (sensitivity == 0)
        {
            verticalLimit = 0;
        }
        if (getTM == null)
        {
            getTM = Timer.tm;
        }
        if (pm == null)
        {
            pm = pauseMenu.pm;
        }
        if (getTM)
        {
            getTM.triggerTimer();
        }
        if (pm)
        {
            pm.startPauseFunction();
            pm.fading = false;
        }
    }
    private void OnDisable()
    {
        flashLight.SetActive(false);
        if (getTM)
        {
            getTM.triggerTimer();
        }

        if (pm)
        {
            pm.fading = true;
            pm.enablePauseFunction(false);
        }
    }
    void FixedUpdate ()
    {
        movement();
        cameraMovement();
    }

    void movement()
    {
        sensitivity = GameManager.sensitivity;
        float hor = Input.GetAxis("Horizontal");

        if (hor == 0)
        {
            if (!console) hor = Input.GetAxis("HorizontalMouse");
        }
        if (ControllerManager.invertX) { hor *= -1; }
        //print(ControllerManager.invertX);

        float hor2 = Input.GetAxis("Horizontal2");

        if (hor2 == 0)
        {
            hor2 = Input.GetAxis("joystick_horizontal");
        }

        float ver = Input.GetAxis("Vertical");

        if (ver != 0)
        {
            vmotion = ver * speed;
            footsteps();
        }
        else
        {
            //motion = Mathf.MoveTowards(motion, 0, 5 * Time.deltaTime);
            //if (motion == 0)
            //{
                vmotion = 0;
            //}
        }
        if (hor2 != 0)
        {
            hmotion = hor2 * speed;
            footsteps();
        }
        else
        {
            //motion = Mathf.MoveTowards(motion, 0, 5 * Time.deltaTime);
            //if (motion == 0)
            //{
            hmotion = 0;
            //}
        }

        float sens = sensitivity;

        Vector3 vec1 = transform.forward * (vmotion * mult);
        Vector3 vec2 = transform.right * (hmotion * mult);
        Vector3 vec = vec1 + vec2;
        rg.velocity = new Vector3(vec.x, rg.velocity.y, vec.z);

        Vector3 rot = (transform.up * hor * sens);
        //Debug.Log(rot);
        //Quaternion id = Quaternion.Euler(rot);
        //rg.MoveRotation(rg.rotation * id);
        transform.Rotate(rot);

        if (Input.GetButton("Jump"))
        {
            if (!Grounded()) return;

            rg.velocity += transform.up * jumpPower;
            
        }

        if (Input.GetButton("Fire3") && _stamina > 0 && !tired)
        {
            mult = SprintMultiplier;
            _stamina += -0.3f;
            if (_stamina <= 0)
            {
                StartCoroutine(StaminaDrained());
            }
            if (!sprintOn)
            {
                sprintOn = true;
                if (gsr) gsr.RunFunction(this);
            }
        }
        else
        {
            mult = 1;
            if (_stamina < stamina)
            {
                _stamina += 0.1f;
                if (_stamina > stamina / 4) _stamina += 0.1f;
                if (_stamina > stamina / 2) _stamina += 0.1f;
            }
            if (sprintOn)
            {
                sprintOn = false;
            }
        }
    }
    void cameraMovement()
    {
        float vert = Input.GetAxis("Vertical2")* -5;
        
        if (vert == 0)
        {
            vert = Input.GetAxis("Mouse Y");
        }
        if (ControllerManager.invertY) { vert *= -1; }
        verticalLimit += vert * Time.deltaTime * sensitivity;
        verticalLimit = Mathf.Clamp(verticalLimit, -0.5f, 0.5f);

        if (Input.GetButton("LookBack"))
        {
            lookBackVar = Mathf.MoveTowards(lookBackVar, 150, 10);
        }
        else
        {
            lookBackVar = Mathf.MoveTowards(lookBackVar, 0, 10);
        }

        Vector3 target = Vector3.Lerp(lowRot, highRot, 0.5f + (verticalLimit));
        target.y = lookBackVar;

        characterCam.localEulerAngles = target;
    }
    bool Grounded()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, -transform.up, out hit, 1.05f);

        if (hit.collider == null)
        {
            return false;
        }

        if (hit.collider.isTrigger == false)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void footsteps()
    {
        if (!moving && Grounded() && speed != 0)
        {
            StartCoroutine(walkingSounds());
        }
    }
    IEnumerator walkingSounds()
    {
        moving = true;
        //print("walking");
        float getM = mult;
        if (getM > 1) getM = 1.5f;
        walkSound.pitch = Random.Range(0.4f, 0.5f);
        walkSound.pitch *= mult;
        walkSound.Play();
        float sc = (walkSound.clip.length + 0.06f) / (getM);
        //print(sc);
        yield return new WaitForSeconds(sc);
        moving = false;
    }

    public CameraSend getCam()
    {
        CameraSend cs = new CameraSend();
        cs.cam = characterCam;
        cs.lerpPoint = transform.Find("lerpPoint");
        return cs;
    }

    IEnumerator StaminaDrained()
    {
        tired = true;
        yield return new WaitForSeconds(3);
        tired = false;
    }
    IEnumerator findStaminaRead()
    {
        yield return new WaitForEndOfFrame();
        gsr = GUI_StaminaRead.GSR;
    }

    IEnumerator resetButtonPress()
    {
        buttonPress = true;
        yield return new WaitForSeconds(0.2f);
        buttonPress = false;
    }
}
