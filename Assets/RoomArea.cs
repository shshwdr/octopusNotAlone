using DG.Tweening;
using Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomArea : MonoBehaviour
{
    public List<Tentacle> tentacles;
    public List<Transform> humanPositions;
    public List<Vector3> usedHumanPositions;
    public List<Transform> usedTentacles;
    public string workType;

    public Text humanAmount;
    public Text warningText;
    public Button upgradeButton;

    public void warnHumanCount()
    {
        humanAmount.transform.DOKill();
        humanAmount.transform.localScale = Vector3.one;
        humanAmount.color = Color.white;
        humanAmount.transform.DOShakeScale(0.3f);
        DOTween.To(() => humanAmount.color, x => humanAmount.color = x, Color.red, 0.3f).SetLoops(2,LoopType.Yoyo);

        
    }

    public void showWarningText(string text)
    {
        if (warningText.gameObject.active)
        {
            return;
        }
        warningText.gameObject.SetActive(true);
        warningText.text = text;
        warningText.color = Color.white;
        DOTween.To(() => warningText.color, x => warningText.color = x, Color.red, 0.3f).SetLoops(-1, LoopType.Yoyo);
    }
    public void hideWarningText()
    {
        warningText.transform.DOKill();
        warningText.gameObject.SetActive(false);
    }

    public Vector3 capturePosition()
    {
        
        for(int i = 0; i < humanPositions.Count; i++)
        {
            var selected = humanPositions[i];
            if (!usedHumanPositions.Contains(selected.position))
            {
                usedHumanPositions.Add(selected.position);
                return selected.position;
            }
        }
        Debug.LogError("overlap for room " + workType);
        return humanPositions[0].position;
    }

    public void returnPosiiton(Vector3 position)
    {
        usedHumanPositions.Remove(position);
    }

    bool canUpgrade()
    {
        var roomInfo = RoomsAndHumanManager.Instance.getRoomByName(workType);
        if (!roomInfo.isAtMaxLevel() && hasEnoughResourceToUpgrade(roomInfo))
        {
            return true;
        }
        return false;
    }

    bool hasEnoughResourceToUpgrade(RoomInfo roomInfo)
    {

        return ResourceManager.Instance.getAmount("happy") > roomInfo.cost;
    }

    public void upgrade()
    {
        if (!canUpgrade())
        {
            return;
        }
        var roomInfo = RoomsAndHumanManager.Instance.getRoomByName(workType);
        ResourceManager.Instance.changeAmount("happy", -roomInfo.cost);
        RoomsAndHumanManager.Instance.getRoomByName(workType).upgrade();
        EventPool.Trigger("upgradeRoom");
        switch (workType)
        {
            case "critical":
                ResourceManager.Instance.updateMaxValue("happy");
                break;
            case "food":
                ResourceManager.Instance.updateMaxValue("food");
                break;


        }
    }

    void updateUpgradeButton()
    {
        if (canUpgrade())
        {
            upgradeButton.gameObject.SetActive(true);
        }
        else
        {

            upgradeButton.gameObject.SetActive(false);
        }
    }


    public IEnumerator catchHuman(Transform human, float time)
    {
        //tentacles[0].moveToCatch(human,time);

        //this target position can be used by others;
        returnPosiiton(human.position);

        //instantiate tentacle
        var tentacle = Instantiate(tentacles[0].gameObject, tentacles[0].transform.position,Quaternion.identity);

        yield return StartCoroutine(tentacle.GetComponent< Tentacle>().moveToCatch(human, time));
        human.parent = null;
        Destroy(tentacle);
    }
    public IEnumerator releaseHuman(Transform human,Vector3 targetPosition, float time)
    {
        //tentacles[0].moveToCatch(human,time);

        //this target position is been captured

        var tentacle = Instantiate(tentacles[0].gameObject, tentacles[0].transform.position, Quaternion.identity);
        yield return StartCoroutine(tentacle.GetComponent<Tentacle>().moveToRelease(human, targetPosition, time));
        human.parent = null;

        Destroy(tentacle);
    }

    private void Start()
    {
        updateUpgradeButton();
        upgradeButton.onClick.AddListener(()=>{
            upgrade();
        });
        EventPool.OptIn("updateResource", updateUpgradeButton);
        ResourceManager.Instance.updateMaxValue("happy");
        ResourceManager.Instance.updateMaxValue("food");
    }
}
