using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RestArea : AreaBase
{


    private void Start()
    {
        foreach(var human in GetComponentsInChildren<Human>())
        {
            humans.Add(human);
        }
    }
}
