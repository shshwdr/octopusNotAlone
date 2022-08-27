using Pool;
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
    protected virtual void Start()
    {
        updateHumanAmountText();
        EventPool.OptIn("upgradeRoom", updateHumanAmountText);
    }

    public virtual void updateHumanAmountText()
    {

        room.humanAmount.text = $"{humans.Count} / { maxHumanCount()}";
    }

    public int maxHumanCount()
    {
        return (int)RoomsAndHumanManager.Instance.getRoomByName(room.workType).humanCount;
    }

    public virtual bool canAddHuman()
    {
        
        return maxHumanCount ()> humans.Count();
    }


    public virtual void addHuman(Human human)
    {
        humans.Add(human);
        //RoomsAndHumanManager.Instance. addHuman(human);
        human.startWorking(this);

        updateHumanAmountText();

        if(room.workType == "critical")
        {
            TutorialManager.Instance.showTutorial("afterHappy_0");
        }
        else if (room.workType == "food")
        {

            TutorialManager.Instance.showTutorial("afterFood_0");
        }
        else if (room.workType == "breed" && humans.Count>1)
        {

            TutorialManager.Instance.showTutorial("afterBreed_0");
        }
        
    }

    public void removeHuman(Human human)
    {
        humans.Remove(human);
        //RoomsAndHumanManager.Instance.removeHuman(human);
        updateHumanAmountText();
    }

    public virtual IOrderedEnumerable<Human> sort()
    {
        return humans.OrderBy(human => human.energy);
    }
    public Human getMinionFromRoom()
    {
        var sorted = sort();
        var selected = sorted.FirstOrDefault();
        if (selected)
        {
            removeHuman(selected);
        }
        return selected;
    }
}