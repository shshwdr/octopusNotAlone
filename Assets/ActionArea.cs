using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class ActionArea : AreaBase
{

    // Start is called before the first frame update
    void Start()
    { 
    }


    void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonUp(0))
            {
                Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);
                if (hit && hit.collider.gameObject == gameObject)
                {
                    StartCoroutine(addMinion());
                }
            }
            else if (Input.GetMouseButtonUp(1))
            {
                Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);
                if (hit && hit.collider.gameObject == gameObject)
                {
                    StartCoroutine(removeMinion());
                }
            }
        }
    }


    public  IEnumerator removeMinion()
    {
        var restRoom = RoomsAndHumanManager.Instance.restArea;
        var selected = getMinionFromRoom();
        if (selected)
        {
            selected.stopWorking();
            yield return StartCoroutine(room.catchHuman(selected.transform, catchTime));
            var selectPosition = restRoom.room.capturePosition();
            yield return StartCoroutine(restRoom.room.releaseHuman(selected.transform, selectPosition, catchTime));
            selected.transform.parent = restRoom.transform;
            restRoom.addHuman(selected);
        }
        yield return null;
    }


    public IEnumerator addMinion()
    {
        var restRoom = RoomsAndHumanManager.Instance.restArea;
        var selected = restRoom.getMinionFromRoom();
        if (selected)
        {

            selected.stopWorking();
            yield return StartCoroutine(restRoom.room.catchHuman(selected.transform, catchTime));

            var selectPosition = room.capturePosition();

            yield return StartCoroutine(room.releaseHuman(selected.transform, selectPosition, catchTime));
            selected.transform.parent = transform;
            addHuman(selected);


        }
        yield return null;
    }


}
