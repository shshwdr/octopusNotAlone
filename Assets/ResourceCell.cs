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
    public void init(ResourceInfo inf)
    {
        this.info = inf;
        text.text = info.type;
        fillImage.fillAmount = ResourceManager.Instance.getAmount(info.type) / info.maxValue;
        EventPool.OptIn("updateResource",updateResource);
    }

    void updateResource()
    {
        fillImage.fillAmount = ResourceManager.Instance.getAmount(info.type) / info.maxValue;
    }

}
