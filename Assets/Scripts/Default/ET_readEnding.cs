using System.Collections;
using UnityEngine;

public class ET_readEnding : MonoBehaviour
{
    public ET_collision[] collisions;
    public GoToSceneOnTrigger GTS;
    public int prefSave = 7;
    bool ready;
    int count;

    private IEnumerator Start()
    {
        foreach(ET_collision e in collisions)
        {
            yield return new WaitForEndOfFrame();
            StartCoroutine(checkET(e));
        }
        while(!ready)
        {
            yield return new WaitForSeconds(1);
            if (count == 4) ready = true;
        }

        GTS.levelSave = prefSave;
    }
    IEnumerator checkET(ET_collision at)
    {
        yield return new WaitForSeconds(1);
        if (at.Collided)
        {
            count++;
            yield break;
        }
        StartCoroutine(checkET(at));
    }
}
