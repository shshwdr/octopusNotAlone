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
    void Awake()
    {
        resourceInfos = CsvUtil.LoadObjects<ResourceInfo>("resource");
        foreach (var resource in resourceInfos)
        {
            resouceAmount[resource.type] = resource.originalValue;
        }

        EventPool.Trigger("updateResource");
    }

    public void changeAmount(string type, int value)
    {

        if (!resouceAmount.ContainsKey(type))
        {
            Debug.LogError("no key in resource " + type);
            return;
        }
        resouceAmount[type] += value;
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
