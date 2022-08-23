using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Human : MonoBehaviour
{
    public float maxEnergy = 20;
    public float energyReduceTime = 1;
    float energyTimer = 0;
    public float energy;

    public Image energyFillImage;

    string workType = "rest";
    // Start is called before the first frame update
    void Start()
    {
        energy = maxEnergy;
        energyFillImage.fillAmount = energy / maxEnergy;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.workType.Length != 0)
        {

            energyTimer += Time.deltaTime;
            if (energyTimer >= energyReduceTime)
            {
                energyTimer = 0;
                if (this.workType == "rest")
                {
                    energy++;
                    energy = Mathf.Min(energy, maxEnergy);
                }
                else
                {
                    energy--;
                    if (energy <= 0)
                    {
                        die();
                    }
                }
                energyFillImage.fillAmount = energy / maxEnergy;
            }
        }

    }

    void die()
    {
        Destroy(gameObject);
    }

    public void stopWorking()
    {
        this.workType = "";
        energyTimer = 0;
    }
    public void startWorking(string workType)
    {
        this.workType = workType;
        //switch (workType)
        //{
        //    case "critical":
        //        break;
        //    case "rest":
        //        break;

        //}

    }
}
