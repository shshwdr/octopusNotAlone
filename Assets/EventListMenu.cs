using Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventListMenu : MonoBehaviour
{
    public GameObject explainMenu;
    public Text explainText;
    // Start is called before the first frame update
    void Start()
    {
        updateBuff();
        EventPool.OptIn("updateBuff", updateBuff);
    }
    void updateBuff()
    {
        int i = 0;
        var cells = GetComponentsInChildren<EventCell>(true);
        for (; i < BuffManager.Instance.activatedBuff.Count;i++)
        {
            cells[i].init(BuffManager.Instance.activatedBuff[i]);
            cells[i].gameObject.SetActive(true);
        }
        for(;i< cells.Length; i++)
        {
            cells[i].gameObject.SetActive(false);

        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
