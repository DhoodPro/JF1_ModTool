using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeEffect : MonoBehaviour
{
    public GameObject SmokePrefab;
    MeshRenderer smokeRenderer;
    Vector3 newPos;
    Vector3 newEulerAngles;
    Vector3 newScale;

    public Color startColor = Color.grey;
    public Color endColor = Color.grey;

    public int speedOfEffect = 1;
    public float randomizer = 1;
    public float ScaleMultiplier = 1;
    public float RotationMuliplier = 1;
    public float PositionMultiplier = 1;
    public int timer = 3;

    bool startColorEffect = false;

    private void Start()
    {
        SmokePrefab = Instantiate(SmokePrefab, transform.position, transform.rotation);
        smokeRenderer = SmokePrefab.GetComponent<MeshRenderer>();

        smokeRenderer.material.color = startColor;

        float randX = Random.Range(-randomizer, randomizer);
        float randY = Random.Range(-randomizer, randomizer);
        float randZ = Random.Range(-randomizer, randomizer);

        newPos = new Vector3(transform.position.x + randX*PositionMultiplier,
            transform.position.y + randY * PositionMultiplier, transform.position.z + randZ * PositionMultiplier);

        newEulerAngles = new Vector3(transform.localEulerAngles.x - randX*RotationMuliplier,
            transform.localEulerAngles.y - randY * RotationMuliplier, transform.localEulerAngles.z - randZ * RotationMuliplier);

        newScale = new Vector3(transform.localScale.x + ScaleMultiplier,
            transform.localScale.y + ScaleMultiplier, transform.localScale.z + ScaleMultiplier);

        SmokePrefab.transform.parent = transform;
        
        StartCoroutine(destroyPrefab());
    }

    private void Update()
    {
        SmokePrefab.transform.position = Vector3.Lerp(SmokePrefab.transform.position, newPos, speedOfEffect * Time.deltaTime);
        SmokePrefab.transform.localEulerAngles = Vector3.Lerp(SmokePrefab.transform.localEulerAngles,
            newEulerAngles, speedOfEffect * Time.deltaTime);
        SmokePrefab.transform.localScale = Vector3.Lerp(SmokePrefab.transform.localScale, newScale, speedOfEffect * Time.deltaTime);

        if (startColorEffect) smokeRenderer.material.color = Color.Lerp(smokeRenderer.material.color, endColor, (speedOfEffect*2) * Time.deltaTime);
    }

    IEnumerator destroyPrefab()
    {
        yield return new WaitForSeconds(1);
        startColorEffect = true;

        yield return new WaitForSeconds(timer);
        Destroy(gameObject);
    }
}
