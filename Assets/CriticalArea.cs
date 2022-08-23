using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticalArea : MonoBehaviour
{
    public float generateTime = 1f;
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
        timer += Time.deltaTime;
        if (timer >= generateTime)
        {
            timer = 0;
            ResourceManager.Instance.changeAmount("happy", actionArea.humans.Count);
        }
    }
}
