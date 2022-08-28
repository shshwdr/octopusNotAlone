using Pool;
using Sinbad;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceInfo
{
    public string type;
    public int originalValue;
    public int maxValue;
}


public class ResourceManager : Singleton<ResourceManager>
{
    public List<ResourceInfo> resourceInfos = new List<ResourceInfo>();
    Dictionary<string, float> resouceAmount = new Dictionary<string, float>();
    //Dictionary<string, float> resouceMax = new Dictionary<string, float>();
    void Awake()
    {
        resourceInfos = CsvUtil.LoadObjects<ResourceInfo>("resource");
        foreach (var resource in resourceInfos)
        {
            resouceAmount[resource.type] = 1000;
            //resouceMax[resource.type] = resource.maxValue;
        }

        EventPool.Trigger("updateResource");
    }

    public void changeAmount(string type, float value)
    {

        if (!resouceAmount.ContainsKey(type))
        {
            Debug.LogError("no key in resource " + type);
            return;
        }
        resouceAmount[type] += value;
        if(resouceAmount[type]>= getMaxValue(type))
        {
            resouceAmount[type] = getMaxValue(type);
        }
        if (resouceAmount[type] <= 0)
        {
            resouceAmount[type] = 0;

            if(type == "food")
            {
                EventManager.Instance.showEventInfo("noFood");
            }else if(type == "happy")
            {

                EventManager.Instance.showEventInfo("notHappy");
            }
        }
        EventPool.Trigger("updateResource");

    }
    public float getMaxValue(string type)
    {
        switch (type) {
            case "food":
                return RoomsAndHumanManager.Instance.getRoomByName("food").other;
            case "happy":
                return RoomsAndHumanManager.Instance.getRoomByName("critical").other;
        }
        Debug.LogError("no value for " + type);
        return 0;
    }

    public void updateMaxValue(string type)
    {
        resouceAmount[type] = Mathf.Min(resouceAmount[type], getMaxValue(type));
        EventPool.Trigger("updateResource");
    }
    public float getAmount(string type)
    {
        if (!resouceAmount.ContainsKey(type))
        {
            Debug.LogError("no key in resource " + type);
            return 0;
        }
        return resouceAmount[type];
    }

    // Update is called once per frame
    void Update()
    {

    }
}
