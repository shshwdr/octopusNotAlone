using Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        updateBuff();
        EventPool.OptIn("updateEnding", updateBuff);
    }
    void updateBuff()
    {
        int i = 0;
        var cells = GetComponentsInChildren<DeathCell>(true);
        for (; i < EndingManager.Instance.activatedEnd.Count; i++)
        {
            cells[i].GetComponentInChildren<Text>().text = EndingManager.Instance.activatedEnd[i].explain;
            //cells[i].gameObject.SetActive(true);
        }
        for (; i < cells.Length; i++)
        {
            //cells[i].gameObject.SetActive(false);
            cells[i].GetComponentInChildren<Text>().text = "???";
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
