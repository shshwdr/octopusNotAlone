using Sinbad;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EventInfo
{
    public string name;
    public string type;
    public string text;
    public List<string> prerequisite;
    public string option1Text;
    public List<string> option1Effect;
    public string option1ResultText;


    public string option2Text;
    public List<string> option2Effect;
    public string option2ResultText;


    public string option3Text;
    public List<string> option3Effect;
    public string option3ResultText;
}
public class EventManager : Singleton<EventManager>
{

    public float eventIntervalTime = 1f;
    float eventIntervalTimer = 0f;
    public float eventCheckTime = 1f;
    float eventCheckTimer = 0f;
    public float eventCheckPossibility = 1f;
    public float eventmaxIntervalTime = 2f;
    public List<EventInfo> eventInfos = new List<EventInfo>();
    public List<EventInfo> unlockedEventInfos = new List<EventInfo>();
    public List<EventInfo> lockedEventInfos = new List<EventInfo>();
    public Dictionary<string, EventInfo> eventInfoDict = new Dictionary<string, EventInfo>();

    public HashSet<EventInfo> visitedEventInfo = new HashSet<EventInfo>();
    public HashSet<string> visitedEventInfoName = new HashSet<string>();

    bool waitForLastEvent = false;
    // Start is called before the first frame update
    void Start()
    {

        eventInfos = CsvUtil.LoadObjects<EventInfo>("events");
        foreach(var info in eventInfos)
        {
            if(info.name == "")
            {
                continue;
            }
            if (info.prerequisite.Count == 0 || info.prerequisite[0].Length == 0)
            {
                unlockedEventInfos.Add(info);
            }
            else
            {
                lockedEventInfos.Add(info);
            }
            eventInfoDict[info.name] = info;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (waitForLastEvent)
        {
            return;
        }
        eventIntervalTimer += Time.deltaTime;
        if (eventIntervalTimer >= eventIntervalTime)
        {
            if (eventCheckTimer == 0)
            {
                //can have event now
                if (UnityEngine.Random.Range(0f, 1f) <= eventCheckPossibility)
                {
                    //succeed, start an event
                    pickEventAndStart();
                    eventIntervalTimer = 0;
                }
                else
                {
                    eventCheckTimer = eventCheckTime;
                }
            }
            else
            {

                eventCheckTimer -= Time.deltaTime;
            }
        }
    }

    void pickEventAndStart()
    {
        if (unlockedEventInfos.Count > 0)
        {
            var selectInfo = unlockedEventInfos[UnityEngine.Random.Range(0, unlockedEventInfos.Count)];

            //check

            Debug.Log(selectInfo.name);
            visitedEventInfo.Add(selectInfo);
            visitedEventInfoName.Add(selectInfo.name);
            unlockedEventInfos.Remove(selectInfo);

            waitForLastEvent = true;
            if (selectInfo.type == "breed")
            {
                BreedManager.Instance.addNextMustBreed(selectInfo.name);
            }
            else
            {
                //var go = Instantiate<GameObject>(Resources.Load<GameObject>( "eventTriggerPrefab"));
                //showEventInfo(selectInfo);
                createEventTrigger(selectInfo);
            }

            unlockEvents();
        }
    }

    public void createEventTrigger(EventInfo selectInfo)
    {

        EventTriggerItem.Instance.info = selectInfo;
        EventTriggerItem.Instance.child.SetActive(true);
        EventTriggerItem.Instance.GetComponent<Collider2D>().enabled = true;
    }
    public void createEventTrigger(string selectInfoStr)
    {

        if (!eventInfoDict.ContainsKey(selectInfoStr))
        {
            Debug.LogError("no event exist " + selectInfoStr);
            return;
        }
        createEventTrigger(eventInfoDict[selectInfoStr]);
    }

    public void finishLastEvent()
    {
        waitForLastEvent = false;
    }

    void unlockEvents()
    {
        var list = new List<EventInfo>(lockedEventInfos);
        foreach(var eventInfo in list)
        {
            bool canUnlock = true;
            foreach(var pre in eventInfo.prerequisite)
            {
                if (!visitedEventInfoName.Contains(pre))
                {
                    canUnlock = false;
                    break;
                }
            }
            if (canUnlock)
            {
                Debug.Log("unlock event " + eventInfo.name);
                unlockedEventInfos.Add(eventInfo);
                lockedEventInfos.Remove(eventInfo);
            }
        }
    }

    public void showEventInfo(string selectInfoStr)
    {
        if (!eventInfoDict.ContainsKey(selectInfoStr))
        {
            Debug.LogError("no event exist " + selectInfoStr);
            return;
        }
        showEventInfo(eventInfoDict[selectInfoStr]);
    }

    public void showEventInfo(EventInfo selectInfo)
    {

        List<EventButtonInfo> eventButtons = new List<EventButtonInfo>();
        eventButtons.Add(new EventButtonInfo(selectInfo.option1Text, selectInfo.option1ResultText, getAction(selectInfo.option1Effect)));
        if (selectInfo.option2Text.Length > 0)
        {
            eventButtons.Add(new EventButtonInfo(selectInfo.option2Text, selectInfo.option2ResultText, getAction(selectInfo.option2Effect)));

        }

        if (selectInfo.option3Text.Length > 0)
        {
            eventButtons.Add(new EventButtonInfo(selectInfo.option3Text, selectInfo.option3ResultText, getAction(selectInfo.option3Effect)));

        }

        EventMenu.showEvent(selectInfo.text, eventButtons);
       // EventMenu.showEvent(selectInfo.text, eventButtons);

    }

    Action getAction(List<string> effects)
    {
        return () => {
            foreach (var eff in effects)
            {
                if (eff == "kill")
                {

                }
                else if (eff.StartsWith("buff_"))
                {
                    BuffManager.Instance.activateBuff(eff);
                }
                else if (eff.StartsWith("add_"))
                {
                    BuffManager.Instance.activateBuff(eff);
                }
            }
        };
    }
}
