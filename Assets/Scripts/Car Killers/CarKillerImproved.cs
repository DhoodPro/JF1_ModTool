using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarKillerImproved : MonoBehaviour
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
    public Transform drivingForward;
    Vector3 originalSpot;
    Health h;

    Transform collisionObj;

    bool interrupt;
    bool hitBackUp;

    public bool activateOnStart;
    public bool driftAround;
    bool front;
    public List<WheelCollider> w_colliders;

     IEnumerator Start()
    {
        drive = GetComponent<SCC_Drivetrain>();
        rigid = GetComponent<Rigidbody>();
        StartCoroutine(SearchForUser());

        float dis = Mathf.Infinity;
        int x = 0;

        if (drivingForward)
        {
            originalSpot = transform.position;
        }

        if (driftAround)
        {
            Transform t = transform.Find("Wheel Colliders");
            foreach(Transform w in t)
            {
                w_colliders.Add(w.GetComponent<WheelCollider>());
            }

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
        yield return new WaitForEndOfFrame();
        h = Health.health;

        if (activateOnStart)
        {
            go = true;
            Pursuit();
        }
    }
    IEnumerator SearchForUser()
    {
        yield return new WaitForSeconds(updateSearchForUser);

        if (enabled == false) yield break;

        if (!go)
        {
            if (targetDirection)
            {
                //Debug.Log(Vector3.Distance(targetDirection.position, transform.position));
                if (Vector3.Distance(targetDirection.position, transform.position) < SpotDistance)
                {
                    RaycastHit rc;
                    Physics.Linecast(transform.position, targetDirection.position, out rc);

                    //print(rc.transform.tag);
                    
                    if (rc.transform.tag == "Player") { go = true; Pursuit(); }
                }

                if ((Vector3.Distance(targetDirection.position, transform.position) > LostDistance) && drivingForward == null)
                {
                    Destroy(gameObject);
                    yield break;
                }
            }
        }
        else
        {
            RaycastHit rc;
            Physics.Linecast(transform.position, targetDirection.position, out rc);

            collisionObj = rc.transform;

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
        if (hitBackUp)
        {
            backup();
        }
        if (interrupt) return;

        if (!go)
        {

            /*
            if (nextCheckpoint)
            {
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
            }*/

            if (drivingForward)
            {
                followTarget(drivingForward, 1, 60);
                if (Vector3.Distance(transform.position, drivingForward.position) < 15)
                {
                    transform.position = originalSpot;
                    /*
                     * -- slide mechanic --
                     * 
                    Vector3 localEul = transform.localEulerAngles;
                    localEul.y += 90;
                    transform.localEulerAngles = localEul;

                    */
                }
            }

        }
        else
        {
            if (collisionObj)
            {
                if (collisionObj.gameObject.layer != 3) followTarget(targetDirection, 1, 200);
                else followTarget(targetDirection, 0, 200);
            }
            else
            {
                followTarget(targetDirection, 1, 200);
            }
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
        yield return new WaitForSeconds(5);
        getOutOfCol = false;
        //Debug.Log("colliding");
    }
    IEnumerator resetCar()
    {
        hitBackUp = true;
        interrupt = true;
        yield return new WaitForSeconds(2);
        hitBackUp = false;
        interrupt = false;
        drive.direction = 1;
    }

    void Pursuit()
    {
        GameManager.GM.InitiateChase(true, carNumber);
        h.addCar(transform);
        GetComponent<SCC_Audio>().enabled = true;
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
        transform.Find("lights").gameObject.SetActive(false);
        drive.inputGas = 0;
        drive.inputBrake = 0;
        StopAllCoroutines();
        //GameManager.GM.InitiateChase(false);
        GameManager.GM.LostPlayer(carNumber);

        Destroy(gameObject);
    }

    void followTarget(Transform target, float gas, float speedLimit)
    {
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

        if (driftAround)
        {
            if (f < 0)
            {
                // player is behind vehicle
                if (drive.speed > 15)
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
                    front = true;//Debug.Log("behind");
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
                    front = false; //Debug.Log("front");
                }
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

    private void OnCollisionEnter(Collision collision)
    {
        if (interrupt) return;
        if (!go) return;
        string tagCheck = collision.collider.tag;

        //if (tagCheck == "RigidSetup") return;

        switch (tagCheck)
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

        //Vector3 forw = transform.TransformDirection(Vector3.forward);
        //float f = Vector3.Dot(forw, collision.contacts[0].point - transform.position);

        //if (f < 0) collisionCounter += 1;

        if (collisionCounter > 100)
        { getOutOfCol = true; StartCoroutine(resetCar()); }
    }
    private void OnCollisionExit(Collision collision)
    {
        collisionCounter = 0;
    }
}
