using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RestArea : AreaBase
{

    public override IOrderedEnumerable<Human> sort()
    {
        return humans.OrderBy(human => -human.energy);
    }

    private void Start()
    {
        foreach(var human in GetComponentsInChildren<Human>())
        {
            addHuman(human);
        }
    }
}
