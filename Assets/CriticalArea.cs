using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticalArea : MonoBehaviour
{
    float timer; 
    ActionArea actionArea;
    float amount = 0;
    // Start is called before the first frame update
    void Start()
    {
        actionArea = GetComponent<ActionArea>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= GameManager.Instance.criticalGenerateTime)
        {
            timer = 0;
            foreach (var human in actionArea.humans)
            {
                amount += human.foodGenerateAmount();
            }
            int intAmount = Mathf.FloorToInt(amount);
            if (intAmount > 0)
            {
                amount -= intAmount;
                ResourceManager.Instance.changeAmount("happy", intAmount);

                PopupTextManager.Instance.ShowPopupNumber(transform.position, intAmount, intAmount);

            }
        }
    }
}
