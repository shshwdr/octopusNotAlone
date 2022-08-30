using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : Singleton<TimeController>
{

    float time;
    public Text timeText;
    int round = 0;
    // Start is called before the first frame update
    void Start()
    {
        time = GameManager.Instance.calculateTime;
        
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        if (time <= 0)
        {
            time = GameManager.Instance.calculateTime;
            calculate();
        }
        timeText.text = $"Round: {round}\nLeft Time:{(Mathf.Floor(time)).ToString()}"; ;
    }

    public int decreaseHappyNextRound { get { return (GameManager.Instance.happyDecreaseBasePerRound + Mathf.FloorToInt(round / GameManager.Instance.happyIncreaseRound) * GameManager.Instance.happyIncreasePerTime); } }
    public int decreaseFoodNextRound { get { float res = 0;
            foreach (var human in RoomsAndHumanManager.Instance.humans)
            {
                res += human.foodConsumeAmount();
            }
            return (int)res;
        } }

    void calculate()
    {
        ResourceManager.Instance.changeAmount("happy", -decreaseHappyNextRound);
        ResourceManager.Instance.changeAmount("food", -decreaseFoodNextRound);
        round++;

        if (round >= 30)
        {
            EventManager.Instance.createEventTrigger("happyEnd");
        }

    }
}
