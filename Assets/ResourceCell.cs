using DG.Tweening;
using Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceCell : MonoBehaviour
{
    public Text text;
    public Image fillImage;
    ResourceInfo info;
    public Text valueText;
    public void init(ResourceInfo inf)
    {
        this.info = inf;
        text.text = info.type;
        fillImage.fillAmount = ResourceManager.Instance.getAmount(info.type) / info.maxValue;
        updateResource();
        EventPool.OptIn("updateResource", updateResource);
        EventPool.OptIn("humanCountChange", updateResource);
        
    }

    void updateResource()
    {
        var value = ResourceManager.Instance.getAmount(info.type);
        var maxValue = ResourceManager.Instance.getMaxValue(info.type);
        fillImage.fillAmount = value / maxValue;
        float decreaseValue = 0;
        if(info.type == "happy")
        {
            decreaseValue = TimeController.Instance.decreaseHappyNextRound;
        }
        else
        {

            decreaseValue = TimeController.Instance.decreaseFoodNextRound;
        }
        valueText.text = $"{value} ({value - decreaseValue}) / {maxValue}";
        if(value - decreaseValue < 0)
        {
            startWarning();
        }
        else
        {
            stopWarning();
        }
    }
    bool isWarning = false;
    void startWarning()
    {
        if (isWarning) return;
        isWarning = true;
        valueText.gameObject.SetActive(true);
        valueText.color = Color.white;
        DOTween.To(() => valueText.color, x => valueText.color = x, Color.red, 0.3f).SetLoops(-1, LoopType.Yoyo);
    }
    void stopWarning()
    {
        isWarning = false;
        valueText.color = Color.white;
    }

}
