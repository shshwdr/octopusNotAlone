using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomArea : MonoBehaviour
{
    public List<Tentacle> tentacles;
    public List<Transform> humanPositions;
    // Start is called before the first frame update
    void Start()
    {
        
    }


    public IEnumerator catchHuman(Transform human, float time)
    {
        //tentacles[0].moveToCatch(human,time);

        yield return StartCoroutine(tentacles[0].moveToCatch(human, time));
    }
    public IEnumerator releaseHuman(Transform human,Vector3 targetPosition, float time)
    {
        //tentacles[0].moveToCatch(human,time);

        yield return StartCoroutine(tentacles[0].moveToRelease(human, targetPosition, time));
    }
}
