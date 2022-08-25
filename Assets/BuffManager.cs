using Pool;
using Sinbad;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffInfo {
    public string name;
    public string explain;

}


public class BuffManager : Singleton<BuffManager>
{

    public List<BuffInfo> buffInfos = new List<BuffInfo>();
    public Dictionary<string, BuffInfo> buffDict = new Dictionary<string, BuffInfo>();

    public List<string> activatedBuff = new List<string>(); 

    // Start is called before the first frame update
    void Start()
    {
        buffInfos = CsvUtil.LoadObjects<BuffInfo>("buff");
        foreach(var info in buffInfos)
        {
            buffDict[info.name] = info;
        }

    }

    public void activateBuff(string str)
    {
        if (activatedBuff.Contains(str))
        {
            Debug.LogError("duplicate buff " + str);
            return;
        }
        activatedBuff.Add(str);
        EventPool.Trigger("updateBuff");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
