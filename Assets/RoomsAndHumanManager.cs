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

}


public class RoomsAndHumanManager : Singleton<RoomsAndHumanManager>
{
    public RestArea restArea;
    public List<Human> humans;

    public RoomArea[] allRooms;


    public List<RoomInfo> roomInfos = new List<RoomInfo>();
    public Dictionary<string, RoomInfo> roomInfoDict = new Dictionary<string, RoomInfo>();

    // Start is called before the first frame update
    void Start()
    {
        allRooms = GameObject.FindObjectsOfType<RoomArea>();
        restArea = GameObject.FindObjectOfType<RestArea>();

        addNewHuman();
        addNewHuman();
        addNewHuman();
        addNewHuman();


        roomInfos = CsvUtil.LoadObjects<RoomInfo>("buff");
        foreach (var info in roomInfos)
        {
            roomInfoDict[info.type] = info;
        }
    }

    public void addNewHuman()
    {
        var position = restArea.GetComponent<RoomArea>() .capturePosition();
        var human = Instantiate(Resources.Load<GameObject>("human"), position, Quaternion.identity);
        BreedManager.Instance.breed(human.GetComponent<Human>(), human.GetComponent<Human>(), human.GetComponent<Human>());
        restArea.addHuman(human.GetComponent<Human>());
        addHuman(human.GetComponent<Human>());
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
    }
}
