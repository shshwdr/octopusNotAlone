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

                var pickP1 = actionArea.humans[Random.Range(0, actionArea.humans.Count)];
                var tempHumans = new List<Human>(actionArea.humans);
                tempHumans.Remove(pickP1);

                var pickP2 = tempHumans[Random.Range(0, tempHumans.Count)];

                RoomsAndHumanManager.Instance.addNewHuman(pickP1,pickP2);


                PopupTextManager.Instance.ShowPopupNumber(transform.position, 1, 1);
                //ResourceManager.Instance.changeAmount("happy", actionArea.humans.Count);
            }
        }
    }
}
