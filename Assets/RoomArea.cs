using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomArea : MonoBehaviour
{
    public List<Tentacle> tentacles;
    public List<Transform> humanPositions;
    public List<Vector3> usedHumanPositions;
    public List<Transform> usedTentacles;
    public string workType;
    

    public Vector3 capturePosition()
    {
        
        for(int i = 0; i < humanPositions.Count; i++)
        {
            var selected = humanPositions[i];
            if (!usedHumanPositions.Contains(selected.position))
            {
                usedHumanPositions.Add(selected.position);
                return selected.position;
            }
        }
        Debug.LogError("overlap for room " + workType);
        return humanPositions[0].position;
    }

    public void returnPosiiton(Vector3 position)
    {
        usedHumanPositions.Remove(position);
    }

    


    public IEnumerator catchHuman(Transform human, float time)
    {
        //tentacles[0].moveToCatch(human,time);

        //this target position can be used by others;
        returnPosiiton(human.position);

        //instantiate tentacle
        var tentacle = Instantiate(tentacles[0].gameObject, tentacles[0].transform.position,Quaternion.identity);

        yield return StartCoroutine(tentacle.GetComponent< Tentacle>().moveToCatch(human, time));
        human.parent = null;
        Destroy(tentacle);
    }
    public IEnumerator releaseHuman(Transform human,Vector3 targetPosition, float time)
    {
        //tentacles[0].moveToCatch(human,time);

        //this target position is been captured

        var tentacle = Instantiate(tentacles[0].gameObject, tentacles[0].transform.position, Quaternion.identity);
        yield return StartCoroutine(tentacle.GetComponent<Tentacle>().moveToRelease(human, targetPosition, time));
        human.parent = null;

        Destroy(tentacle);
    }
}
