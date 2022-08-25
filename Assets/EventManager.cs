using Sinbad;
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
    public string option1Effect;
    public string option1ResultText;


    public string option2Text;
    public string option2Effect;
    public string option2ResultText;


    public string option3Text;
    public string option3Effect;
    public string option3ResultText;
}
public class EventManager : MonoBehaviour
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

    public HashSet<EventInfo> visitedEventInfo = new HashSet<EventInfo>();
    // Start is called before the first frame update
    void Start()
    {

        eventInfos = CsvUtil.LoadObjects<EventInfo>("events");
        foreach(var info in eventInfos)
        {
            if (info.prerequisite.Count == 0 || info.prerequisite[0].Length == 0)
            {
                unlockedEventInfos.Add(info);
            }
            else
            {
                lockedEventInfos.Add(info);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        eventIntervalTimer += Time.deltaTime;
        if (eventIntervalTimer >= eventIntervalTime)
        {
            if (eventCheckTimer == 0)
            {
                //can have event now
                if (Random.Range(0f, 1f) <= eventCheckPossibility)
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
            var selectInfo = unlockedEventInfos[Random.Range(0, unlockedEventInfos.Count)];
            Debug.Log(selectInfo);
            visitedEventInfo.Add(selectInfo);


            List<EventButtonInfo> eventButtons = new List<EventButtonInfo>();
            eventButtons.Add( new EventButtonInfo( selectInfo.option1Text, null));
            if (selectInfo.option2Effect.Length > 0)
            {
                eventButtons.Add(new EventButtonInfo(selectInfo.option2Text, null));

            }

            if (selectInfo.option3Effect.Length > 0)
            {
                eventButtons.Add(new EventButtonInfo(selectInfo.option3Text, null));

            }

            GameObject.FindObjectOfType<EventMenu>(true).Init(selectInfo.text, eventButtons);

            GameObject.FindObjectOfType<EventMenu>(true).showView();
        }
    }
}
