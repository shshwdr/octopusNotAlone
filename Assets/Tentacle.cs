using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tentacle : MonoBehaviour
{

    public Transform start;
    public Transform end;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator moveToRelease(Transform item,Vector3 targetPosition, float time)
    {
        item.position = end.position;
        item.parent = end;
        moveTo(targetPosition, time);
        yield return new WaitForSeconds(time);
        item.parent = null;
        moveTo(start.position, time);
        yield return new WaitForSeconds(time);
    }

    public IEnumerator moveToCatch(Transform target, float time)
    {
        moveTo(target.position, time);
        yield return new WaitForSeconds(time);
        target.parent = end;


        moveTo(start.position, time);
        yield return new WaitForSeconds(time);


    }
    public void moveTo(Vector3 target, float time)
    {

        end.DOMove(target, time);
    }
}
