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

    public List<string> checks;
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

    public string image;
    public int isLocked;
    public int isRepeated;
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

    HashSet<string> prerequisiteUnlocked = new HashSet<string>();

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
            if (info.isLocked == 1)
            {
                continue;
            }
            if(info.prerequisite == null)
            {
                Debug.Log(info.name);
            }
            if (info.prerequisite==null ||info.prerequisite.Count == 0 || info.prerequisite[0].Length == 0)
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
            if (eventCheckTimer <= 0)
            {
                //can have event now
                if (eventIntervalTimer>=eventmaxIntervalTime|| UnityEngine.Random.Range(0f, 1f) <= eventCheckPossibility)
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

    bool canPassCheck(EventInfo info)
    {
        if(info.checks == null || info.checks.Count == 0 || info.checks[0].Length == 0)
        {
            return true;
        }
        
        foreach (var ch in info.checks)
        {
            if (ch.StartsWith("mutation"))
            {
                int mutation = 0;
                int.TryParse(ch.Split('_')[1], out mutation);
                if (mutation == 0)
                {
                    Debug.LogError("no "+ ch);
                }
                if (!RoomsAndHumanManager.Instance.hasMutation(mutation))
                {
                    return false;
                }
            }
            else
            {
                if (!RoomsAndHumanManager.Instance.hasType(ch))
                {
                    return false;
                }

            }

        }
        return true;
    }

    List<EventInfo> canPickEvents()
    {
        List<EventInfo> res = new List<EventInfo>();
        foreach(var info in unlockedEventInfos)
        {
            if (canPassCheck(info))
            {
                res.Add(info);
            }
        }
        return res;
    }

    void pickEventAndStart()
    {
        if (unlockedEventInfos.Count > 0)
        {
            var canpick = canPickEvents();
            if(canpick.Count == 0)
            {
                Debug.LogError("no events now");
                return;
            }
            var selectInfo = canpick[UnityEngine.Random.Range(0, canpick.Count)];


            Debug.Log(selectInfo.name);
            if(selectInfo.isRepeated != 1)
            {

                visitedEventInfo.Add(selectInfo);
                visitedEventInfoName.Add(selectInfo.name);
                unlockedEventInfos.Remove(selectInfo);
            }

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

        SFXManager.Instance.playeventShowClip();
        waitForLastEvent = true;
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
                if (!prerequisiteUnlocked.Contains(pre))
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
        eventButtons.Add(new EventButtonInfo(selectInfo.option1Text, selectInfo.option1ResultText, getAction(selectInfo.option1Effect), (selectInfo.option1Effect.Count == 2 && selectInfo.option1Effect[1] == "restart")));
        if (selectInfo.option2Text.Length > 0)
        {
            eventButtons.Add(new EventButtonInfo(selectInfo.option2Text, selectInfo.option2ResultText, getAction(selectInfo.option2Effect), (selectInfo.option1Effect.Count == 2 && selectInfo.option1Effect[1] == "restart")));

        }

        if (selectInfo.option3Text.Length > 0)
        {
            eventButtons.Add(new EventButtonInfo(selectInfo.option3Text, selectInfo.option3ResultText, getAction(selectInfo.option3Effect), (selectInfo.option1Effect.Count == 2 && selectInfo.option1Effect[1] == "restart")));

        }

        EventMenu.showEvent(selectInfo.text, eventButtons, selectInfo.image);
       // EventMenu.showEvent(selectInfo.text, eventButtons);

    }


    Action getAction(List<string> effects)
    {
        return () => {
            foreach (var eff in effects)
            {
                var effs = eff.Split('_');
                switch (effs[0])
                {
                    case "kill":
                        var killType = effs[1];
                        var killAmount = effs[2];
                        var killOpposite = effs.Length>3 &&  (effs[3] == "op");
                        RoomsAndHumanManager.Instance.killHuman(killType, int.Parse( killAmount), killOpposite);
                        break;
                    case "buff":
                    case "add":
                        BuffManager.Instance.activateBuff(eff);
                        break;
                    case "unlock":
                        prerequisiteUnlocked.Add(effs[1]);
                        break;
                    case "canBreed":
                        BreedManager.Instance.addCanBreedMutation(effs[1]);
                        break;
                    case "addHuman":
                        for(int i = 0; i < int.Parse( effs[1]); i++)
                        {
                            RoomsAndHumanManager.Instance.addNewHuman();
                        }
                        break;
                    case "restart":
                        GameManager.Instance.restartGame();
                        break;
                    case "restore":
                        ResourceManager.Instance.changeAmount("food", 100);
                        ResourceManager.Instance.changeAmount("happy", 100);
                        break;
                    case "end":

                        SFXManager.Instance.playfailClip();
                        EndingManager.Instance.activateEnd(effs[1]);
                        break;

                }
            }
        };
    }
}
