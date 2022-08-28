using Pool;
using Sinbad;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomInfo {
    public string type;
    public List<float> upgradeCost;
    public List<float> upgradeHumanCount;
    public List<float> otherValue;
    public string displayName;
    public string desc;
    public string humanCountDesc;
    public string otherUpgradeDesc;
    public string upgradeDesc;

    public int maxLevel { get { return upgradeCost.Count; } }


    public int level = 0;

    public float nextCost
    {
        get
        {
            if (isAtMaxLevel())
            {
                return 0;
            }
            return upgradeCost[level + 1];
        }
    }

    public float cost { get { if (level >= upgradeCost.Count)
            {
                Debug.LogError("out of index in upgrade cost");
                return -1;
            }
            return upgradeCost[level]; } }

    public float nextHumanCount
    {
        get
        {
            if (isAtMaxLevel())
            {
                return 0;
            }
            return upgradeHumanCount[level + 1];
        }
    }
    public float humanCount
    {
        get
        {
            if (level >= upgradeHumanCount.Count)
            {
                Debug.LogError("out of index in humanCount");
                return -1;
            }
            return upgradeHumanCount[level];
        }
    }

    public float nextOther
    {
        get
        {
            if (isAtMaxLevel())
            {
                return 0;
            }
            return otherValue[level + 1];
        }
    }
    public float other
    {
        get
        {
            if (level >= otherValue.Count)
            {
                Debug.LogError("out of index in otherValue");
                return -1;
            }
            return otherValue[level];
        }
    }


    public bool isAtMaxLevel()
    {
        return level >= maxLevel;
    }

    public void upgrade()
    {
        if (isAtMaxLevel())
        {
            Debug.LogError("cant upgrade at level "+level);
            return;
        }
        level++;
    }
}


public class RoomsAndHumanManager : Singleton<RoomsAndHumanManager>
{
    public RestArea restArea;
    public List<Human> humans;

    public RoomArea[] allRooms;






    public List<RoomInfo> roomInfos = new List<RoomInfo>();
    public Dictionary<string, RoomInfo> roomInfoDict = new Dictionary<string, RoomInfo>();

    public RoomInfo getRoomByName(string name)
    {
        if (!roomInfoDict.ContainsKey(name))
        {
            Debug.LogError(" no room " + name);
        }
        return roomInfoDict[name];
    }
    private void Awake()
    {

        roomInfos = CsvUtil.LoadObjects<RoomInfo>("room");
        foreach (var info in roomInfos)
        {
            roomInfoDict[info.type] = info;
        }
    }
    // Start is called before the first frame update
    void Start()
    {



        allRooms = GameObject.FindObjectsOfType<RoomArea>();
        restArea = GameObject.FindObjectOfType<RestArea>();

        addNewHuman();
        addNewHuman();
        addNewHuman();
        addNewHuman();


    }

    public int maxTotalHumanCount { get { return (int)RoomsAndHumanManager.Instance.getRoomByName("rest").humanCount; } }
    bool canAddHuman()
    {
        return maxTotalHumanCount > humans.Count;
    }
    public void addNewHuman(Human p1 = null, Human p2 = null)
    {
        var position = restArea.GetComponent<RoomArea>() .capturePosition();
        var human = Instantiate(Resources.Load<GameObject>("human"), position, Quaternion.identity);
        BreedManager.Instance.breed(p1, p2, human.GetComponent<Human>());

        if (canAddHuman())
        {

            restArea.addHuman(human.GetComponent<Human>());
            addHuman(human.GetComponent<Human>());
        }
        else
        {
            // kill directly
            HumanView.Instance.tooManyHuman();
            human.GetComponent<Human>().kill();

        }
    }

    public bool hasType(string str)
    {
        foreach(var human in humans)
        {
            if (human.isType(str))
            {
                return true;
            }
        }
        return false;
    }

    public void killHuman(string type, int amount, bool isOpposite)
    {

        foreach (var human in humans)
        {
            if (type == "any" || (!isOpposite&&human.isType(type)) || (isOpposite && !human.isType(type)))
            {
                human.kill();
                amount -= 1;
                if(amount == 0)
                {
                    return;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addHuman(Human human)
    {
        humans.Add(human);
        EventPool.Trigger("humanCountChange");

    }
    public void removeHuman(Human human)
    {
        humans.Remove(human);
        EventPool.Trigger("humanCountChange");
        if (humans.Count <= 0)
        {

            EventManager.Instance.showEventInfo("noHuman");
        }
    }
}
