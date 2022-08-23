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
        var positions = restArea.GetComponent<RoomArea>().humanPositions;
        var position = positions[Random.Range(0, positions.Count)];
        var human = Instantiate(Resources.Load<GameObject>("human"), position.position, Quaternion.identity);
        restArea.addHuman(human.GetComponent<Human>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addHuman(Human human)
    {
        humans.Add(human);
    }
    public void removeHuman(Human human)
    {
        humans.Remove(human);
    }
}
