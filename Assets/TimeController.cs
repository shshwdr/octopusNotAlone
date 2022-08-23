using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    public int calculateTime = 5;
    float time;
    public Text timeText;
    int round = 0;
    // Start is called before the first frame update
    void Start()
    {
        time = calculateTime;
        
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        if (time <= 0)
        {
            time = calculateTime;
            calculate();
        }
        timeText.text = (Mathf.Floor(time)).ToString();
    }

    void calculate()
    {
        ResourceManager.Instance.changeAmount("happy", -5 - (round));
        ResourceManager.Instance.changeAmount("food", -RoomsAndHumanManager.Instance.humans.Count);
        round++;

    }
}
