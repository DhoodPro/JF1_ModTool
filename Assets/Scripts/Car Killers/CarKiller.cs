using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarKiller : MonoBehaviour
{
    public Transform targetDirection;
    SCC_Drivetrain drive;
    Rigidbody rigid;
    public float turnSpeed = 1;
    public bool go;
    bool check;
    public AudioDistortionFilter radio;
    public float SpotDistance = 30;
    public float LostDistance = 70;
    public int updateSearchForUser = 1;
    public float steeringDirection;
    public float checkpointDistanceSwitch = 10;
    public Transform roadMap;
    Transform nextCheckpoint;
    int mapTracker;
    int collisionCounter = 0;
    bool getOutOfCol;
    bool backupBool;
    public Transform player;
    public GameObject sparkSet;
    public int carNumber = 1;
    public GameObject Fire;
    public bool activateFire;
    GameManager gm;
    Health h;

    bool interrupt;
    bool hitBackUp;
    bool front;
    bool startRay;
    bool rayHitting;

    public bool defaultRayHeight = true;
    public float rayHeight = 1;
    public float rayDistance = 25;
    public float raySpread = 8;

    List<WheelCollider> w_colliders = new List<WheelCollider>();

    IEnumerator Start()
    {
        drive = GetComponent<SCC_Drivetrain>();
        rigid = GetComponent<Rigidbody>();
        gm = GameManager.GM;
        startRay = true;
        if (defaultRayHeight) { rayHeight = 0.5f; }
        yield return new WaitForEndOfFrame();
        h = Health.health;
        StartCoroutine(SearchForUser());

        float dis = Mathf.Infinity;
        int x = 0;

        Transform wc = transform.Find("Wheel Colliders");
        foreach (Transform w in wc)
        {
            w_colliders.Add(w.GetComponent<WheelCollider>());
        }

        foreach (Transform t in roadMap)
        {
            float check = Vector3.Distance(transform.position, t.position);
            if (check < dis)
            {
                nextCheckpoint = t;
                dis = check;
                mapTracker = x;
            }
            x += 1;
        }
    }
    IEnumerator SearchForUser()
    {
        yield return new WaitForSeconds(updateSearchForUser);

        if (!go)
        {
            if (targetDirection)
            {
                //Debug.Log(Vector3.Distance(targetDirection.position, transform.position));
                if (Vector3.Distance(targetDirection.position, transform.position) < SpotDistance)
                {
                    RaycastHit rc;
                    Physics.Linecast(transform.position, targetDirection.position, out rc);

                    if (rc.transform.tag == "Player") { Pursuit(); }
                }
            }
        }
        else
        {
            if (targetDirection)
            {
                if (Vector3.Distance(targetDirection.position, transform.position) > SpotDistance * 2)
                {
                    stopChase(true);
                    yield break;
                }
            }
        }
        StartCoroutine(SearchForUser());
    }

    private void FixedUpdate()
    {
        if (targetDirection == null) return;
        if (hitBackUp)
        {
            backup();
        }
        if (interrupt) return;

        if (!go)
        {
            if (nextCheckpoint)
            {
                rayCastSet();
                followTarget(nextCheckpoint, 1, 40);

                float dis = Vector3.Distance(nextCheckpoint.position, transform.position);
                if (dis < checkpointDistanceSwitch)
                {
                    mapTracker += 1;
                    if (mapTracker >= roadMap.childCount)
                    {
                        mapTracker = 0;
                    }

                    nextCheckpoint = roadMap.GetChild(mapTracker);
                }
            }
        }
        else
        {
            rayCastSet();
            followTarget(player, 1, 80);
        }
    }
    IEnumerator resetCol()
    {
        float dis = Mathf.Infinity;
        int x = 0;
        foreach (Transform t in roadMap)
        {
            float check = Vector3.Distance(transform.position, t.position);
            if (check < dis)
            {
                nextCheckpoint = t;
                dis = check;
                mapTracker = x;
            }
            x += 1;
        }
        StartCoroutine(resetCar());
        yield return new WaitForSeconds(5);
        getOutOfCol = false;
        //Debug.Log("colliding");
    }
    IEnumerator resetCar()
    {
        hitBackUp = true;
        interrupt = true;
        yield return new WaitForSeconds(3);
        hitBackUp = false;
        interrupt = false;
        if (drive) drive.direction = 1;
    }

    void Pursuit()
    {
        if (go) return;
        
        go = true;
        gm.InitiateChase(true, 1);
        h.addCar(transform);
        if (activateFire) Fire.SetActive(true);
        /*
        Transform getLights = transform.Find("lights");
        foreach(Transform t in getLights)
        {
            Light l = t.GetComponent<Light>();
            l.intensity = 10;
            l.range = 10;
        }*/
    }
    void stopChase(bool audStop)
    {
        interrupt = true;
        /*
        if (audStop)
        {
            AudioSource[] auds = transform.GetComponentsInChildren<AudioSource>();
            foreach (AudioSource a in auds)
            {
                a.enabled = false;
            }
        }*/
        //transform.Find("lights").gameObject.SetActive(false);
        //drive.inputGas = 0;
        //drive.inputBrake = 0;
        //StopAllCoroutines();
        //GameManager.GM.InitiateChase(false);
        gm.LostPlayer(carNumber);

        Destroy(gameObject);
    }

    void followTarget(Transform target, float gas, float speedLimit)
    {
        if (rayHitting) return;
        /*
        Quaternion rot = Quaternion.LookRotation(targetDirection.position - transform.position);
        Quaternion move = Quaternion.Lerp(transform.rotation, rot, turnSpeed * Time.deltaTime);

        rigid.MoveRotation(move);

        drive.inputGas = 1;
        */

        if (drive.speed < speedLimit) drive.inputGas = gas;
        else drive.inputGas = 0;
        Vector3 ang = target.position - transform.position;
        drive.inputSteering = Vector3.Angle(ang, transform.forward) * Time.deltaTime * 5;
        if (drive.inputSteering > 1)
        {
            drive.inputSteering = 1;
        }
        else if (drive.inputSteering < -1)
        {
            drive.inputSteering = -1;
        }

        Vector3 vec = transform.TransformDirection(Vector3.right);
        Vector3 brake = transform.TransformDirection(Vector3.forward);
        Vector3 player = target.position - transform.position;

        float v = Vector3.Dot(vec, player);
        float f = Vector3.Dot(brake, player);
        //steeringDirection = v;

        drive.inputBrake = 0;
        if (drive.speed < speedLimit) drive.inputGas = gas;

        if (v < 0) drive.inputSteering *= -1;

        
            if (f < 0)
            {
                // player is behind vehicle
                if (drive.speed > 20)
                {
                    drive.inputHandbrake = 1;
                    drive.inputGas = 0;
                }
                else
                {
                    drive.inputHandbrake = 0;
                    drive.inputGas = gas;
                }


                if (front == false)
                {
                    foreach (WheelCollider wc in w_colliders)
                    {
                        WheelFrictionCurve wfc = wc.sidewaysFriction;
                        wfc.asymptoteSlip = 2;
                        wfc.stiffness = 1.5f;
                        wc.sidewaysFriction = wfc;
                    }
                    front = true;
                }
            }
            else
            {
                // player is in front of vehicle
                drive.inputHandbrake = 0;

                if (front == true)
                {
                    foreach (WheelCollider wc in w_colliders)
                    {
                        WheelFrictionCurve wfc = wc.sidewaysFriction;
                        wfc.extremumSlip = 0.3f;
                        wfc.stiffness = 1.5f;
                        wc.sidewaysFriction = wfc;
                    }
                    front = false;
                }
            }
        

        if (drive.speed > 40)
        {
            float getAngle = Vector3.Angle(ang, transform.forward);
            //Debug.Log(getAngle);
            if (getAngle > 15)
            {
                drive.inputGas = 0;
            }
        }
    }
    void backup()
    {
        drive.inputGas = 0;
        drive.direction = -1;
        drive.inputBrake = 0.5f;
        drive.inputSteering = 0;
    }

    void rayCastSet()
    {
        if (targetDirection == null) return;

        if (startRay == false || hitBackUp)
        {
            rayHitting = false;
            return;
        }

        RaycastHit rc1;
        Vector3 raycastY = new Vector3(0, rayHeight, 0);
        Physics.Linecast(transform.position, targetDirection.position, out rc1);
        float getDis = Vector3.Distance(targetDirection.position, transform.position);
        //print(rc.transform.tag);

        if (rc1.transform.tag == "Player" && getDis < 25)
        {
            rayHitting = false;
            return;
        }

        //rayDistance = 5 + (drive.speed / 2);

        Debug.DrawRay(transform.position + transform.right + raycastY, (transform.forward * rayDistance) + (transform.right * raySpread) + raycastY, Color.green);
        Debug.DrawRay(transform.position + -transform.right + raycastY, (transform.forward * rayDistance) + (-transform.right * raySpread) + raycastY, Color.green);
        //Debug.DrawRay(transform.position, (transform.forward * 15) + (transform.right * 5), Color.green);
        //Debug.DrawRay(transform.position, (transform.forward * 15) + (-transform.right * 5), Color.green);

        RaycastHit[] rcSet = new RaycastHit[2];
        Physics.Linecast((transform.position + transform.right) + raycastY, (transform.position + transform.right) + (transform.forward * rayDistance) + (transform.right * raySpread) + raycastY, out rcSet[0]);
        Physics.Linecast((transform.position + -transform.right) + raycastY, (transform.position + -transform.right) + (transform.forward * rayDistance) + (-transform.right * raySpread) + raycastY, out rcSet[1]);

        bool hit = false;
        int bothHit = 0;
        int x = -1;
        foreach (RaycastHit rc in rcSet)
        {
            if (rc.transform)
            {
                string tagCheck = rc.transform.tag;
                //print(rc.transform);

                if (tagCheck != "terrain" && tagCheck != "Player")
                {
                    bothHit += 1;
                    hit = true;

                    Vector3 vec = transform.TransformDirection(Vector3.right);
                    Vector3 vec2 = transform.TransformDirection(Vector3.forward);
                    Vector3 coll = rc.point;
                    Vector3 player = targetDirection.position - transform.position;

                    float v = Vector3.Dot(vec, coll);
                    float f = Vector3.Dot(vec2, player);

                    drive.inputSteering = (x * (drive.speed / 20));
                    drive.inputSteering = Mathf.Clamp(drive.inputSteering, -1, 1);
                    //if (f < 0)
                    //{
                    //    drive.inputGas = 0;
                    //    drive.direction = -1;
                    //    drive.inputBrake = 0.5f;
                    //}
                    //else
                    //{
                    drive.inputGas = 1f;
                    drive.inputBrake = 0f;
                    drive.direction = 1;
                    //}

                    //float sp = 0.2f;
                    //if (drive.speed > 0)
                    //{
                    //    sp = 1f / drive.speed;
                    //}
                    //drive.inputGas = sp;
                }
            }
            x += 2;
        }
        if (bothHit == 2)
        {
            hit = false;
        }

        rayHitting = hit;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (interrupt) return;
        if (!go) return;
        if (!enabled) return;
        string tagCheck = collision.collider.tag;
        switch(tagCheck)
        {
            case "Player":
                go = false;
                return;
            case "junkyardCar":
                
                break;
        }

        sparkSet.transform.position = collision.contacts[0].point;
        sparkSet.SetActive(true);
        StartCoroutine(resetCar());
        //stopChase(true);
    }
    private void OnCollisionStay(Collision collision)
    {
        if (getOutOfCol) return;

        collisionCounter += 1;

        if (collisionCounter > 100)
        { getOutOfCol = true; StartCoroutine(resetCol()); }
    }
    private void OnCollisionExit(Collision collision)
    {
        collisionCounter = 0;
    }
}
