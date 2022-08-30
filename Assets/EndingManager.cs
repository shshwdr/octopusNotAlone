using Pool;
using Sinbad;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndInfo
{
    public string name;
    public string explain;

}

public class EndingManager : Singleton<EndingManager>
{
    public List<EndInfo> endInfos = new List<EndInfo>();
    public Dictionary<string, EndInfo> endDict = new Dictionary<string, EndInfo>();

    public List<EndInfo> activatedEnd= new List<EndInfo>();

    // Start is called before the first frame update
    void Start()
    {
        endInfos = CsvUtil.LoadObjects<EndInfo>("endings");
        foreach (var info in endInfos)
        {
            endDict[info.name] = info;

        }

    }

    public void activateEnd(string str)
    {
        var endInfo = endDict[str];
        activatedEnd.Add(endInfo);

        EventPool.Trigger("updateEnding");
    }
}
