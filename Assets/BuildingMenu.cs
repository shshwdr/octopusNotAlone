using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingMenu : Singleton<BuildingMenu>
{
    public GameObject panel;
    public Text nameText;
    public Text purposeText;
    public Text humanCountText;
    public Text otherUpgradeText;
    public Text costText;
    public void show(RoomInfo info)
    {
        panel.SetActive(true);
        nameText.text = $"{info.displayName} ({info.level + 1} / {info.maxLevel + 1})";
        purposeText.text = $"{info.desc}";
        if (info.humanCountDesc.Length > 0)
        {

            humanCountText.text = $"{info.humanCountDesc} {info.humanCount}";
            if (!info.isAtMaxLevel())
            {
                humanCountText.text += info.nextHumanCount;
            }

            costText.text = $"{info.upgradeDesc} {info.cost}";
        }
        else
        {
            humanCountText.text = "";
        }
        if (info.otherUpgradeDesc.Length > 0)
        {

            otherUpgradeText.text = $"{info.otherUpgradeDesc} {info.other} ";
            if (!info.isAtMaxLevel())
            {
                otherUpgradeText.text += info.nextOther;
            }
        }
        else
        {
            otherUpgradeText.text = "";
        }
    }
    public void hide()
    {

        panel.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
