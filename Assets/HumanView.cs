using Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HumanView : MonoBehaviour
{
    public Text humanText;
    // Start is called before the first frame update
    void Start()
    {
        humanCountChange();
        EventPool.OptIn("humanCountChange", humanCountChange);
    }

    void humanCountChange()
    {
        humanText.text = "Human: "+ RoomsAndHumanManager.Instance.humans.Count.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
