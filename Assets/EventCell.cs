using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCell : MonoBehaviour
{
    public BuffInfo eventInfo;
    EventListMenu listMenu;
    public void init(string eventString)
    {
        if (!BuffManager.Instance.buffDict.ContainsKey(eventString))
        {
            Debug.LogError("no event string " + eventString);
            return;
        }
        eventInfo = BuffManager.Instance.buffDict[eventString];
        listMenu = GetComponentInParent<EventListMenu>();
    }

    public void onHover()
    {
        listMenu.explainMenu.SetActive(true);
        listMenu.explainText.text = eventInfo.explain;
    }

    public void onLeave()
    {
        listMenu.explainMenu.SetActive(false);

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
