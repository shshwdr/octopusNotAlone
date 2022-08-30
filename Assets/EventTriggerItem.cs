using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTriggerItem : Singleton<EventTriggerItem>
{
    public EventInfo info;
    public GameObject child;
    private void OnMouseDown()
    {
        EventManager.Instance.showEventInfo(info);
        EventManager.Instance.finishLastEvent();
        child.SetActive(false);
        GetComponent<Collider2D>().enabled = false;
    }
    // Start is called before the first frame update
    void Start()
    {

        DOTween.To(() => child.GetComponent<SpriteRenderer>().color, x => child.GetComponent<SpriteRenderer>().color = x, Color.red, 1f).SetLoops(-1, LoopType.Yoyo);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
