using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class AreaBase:MonoBehaviour
{

    public float catchTime = 1;
    public List<Human> humans;

    public RoomArea room;
    private void Awake()
    {

        room = GetComponent<RoomArea>();
    }
    // Update is called once per frame

    public void addHuman(Human human)
    {
        humans.Add(human);
        RoomsAndHumanManager.Instance. addHuman(human);
        human.startWorking(room.workType);
    }
    public Human getMinionFromRoom()
    {
        var sorted = humans.OrderBy(human => human.energy);
        var selected = sorted.FirstOrDefault();
        if (selected)
        {

            humans.Remove(selected);
            RoomsAndHumanManager.Instance.removeHuman(selected);
        }
        return selected;
    }
}