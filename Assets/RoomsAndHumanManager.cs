using Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomsAndHumanManager : Singleton<RoomsAndHumanManager>
{
    public RestArea restArea;
    public List<Human> humans;
    // Start is called before the first frame update
    void Start()
    {
        restArea = GameObject.FindObjectOfType<RestArea>();

        addNewHuman();
        addNewHuman();
        addNewHuman();
        addNewHuman();
    }

    public void addNewHuman()
    {
        var position = restArea.GetComponent<RoomArea>() .capturePosition();
        var human = Instantiate(Resources.Load<GameObject>("human"), position, Quaternion.identity);
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
