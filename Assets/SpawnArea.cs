using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnArea : MonoBehaviour
{
    float timer;
    ActionArea actionArea;
    // Start is called before the first frame update
    void Start()
    {
        actionArea = GetComponent<ActionArea>();
    }

    // Update is called once per frame
    void Update()
    {

        if (actionArea.humans.Count == 1)
        {
            GetComponent<RoomArea>().showWarningText("Require 2 human to breed");
        }
        else
        {
            GetComponent<RoomArea>().hideWarningText();
        }

        if (actionArea.humans.Count > 1)
        {

            timer += Time.deltaTime * (actionArea.humans.Count);
            if (timer >= GameManager.Instance.childGenerateTime)
            {
                timer = 0;
                RoomsAndHumanManager.Instance.addNewHuman();


                PopupTextManager.Instance.ShowPopupNumber(transform.position, 1, 1);
                //ResourceManager.Instance.changeAmount("happy", actionArea.humans.Count);
            }
        }
    }
}
