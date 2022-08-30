using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscardRoom : AreaBase
{
    public Transform foodSpawnTrans;
    public Transform energySpawnTrans;
    public override bool canAddHuman()
    {
        return true;
    }

    public override void addHuman(Human human)
    {

        TutorialManager.Instance.showTutorial("afterDiscard_0");
        human.kill();

        ResourceManager.Instance.changeAmount("food", GameManager.Instance.discardFood);
        ResourceManager.Instance.changeAmount("happy", GameManager.Instance.discardEnergy);

        PopupTextManager.Instance.ShowPopupNumber(foodSpawnTrans.position, (int)GameManager.Instance.discardFood, GameManager.Instance.discardFood);
        PopupTextManager.Instance.ShowPopupNumber(energySpawnTrans.position, (int)GameManager.Instance.discardEnergy, GameManager.Instance.discardEnergy);

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
