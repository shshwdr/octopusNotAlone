using Pool;
using Sinbad;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffInfo {
    public string name;
    public string explain;
    public string affect;
    public float multiplier;

}


public class BuffManager : Singleton<BuffManager>
{

    public List<BuffInfo> buffInfos = new List<BuffInfo>();
    public Dictionary<string, BuffInfo> buffDict = new Dictionary<string, BuffInfo>();

    public List<string> activatedBuff = new List<string>();
    public Dictionary<string, float> buffEffects = new Dictionary<string, float>();

    // Start is called before the first frame update
    void Start()
    {
        buffInfos = CsvUtil.LoadObjects<BuffInfo>("buff");
        foreach(var info in buffInfos)
        {
            buffDict[info.name] = info;

            if (!buffEffects.ContainsKey(info.affect))
            {
                buffEffects[info.affect] = 1;
            }
        }

    }


    public float getBuffEffects(string effect)
    {
        if (!buffEffects.ContainsKey(effect))
        {
            Debug.Log("no effect " + effect);
            return 1;
        }
        return buffEffects[effect];
    }

    public void activateBuff(string str)
    {
        if (activatedBuff.Contains(str))
        {
            Debug.LogError("duplicate buff " + str);
            return;
        }
        activatedBuff.Add(str);
        var buffInfo = buffDict[str];

        buffEffects[buffInfo.affect] *= buffInfo.multiplier;
        EventPool.Trigger("updateBuff");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
