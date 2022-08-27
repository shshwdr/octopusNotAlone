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

    public override bool canAddHuman()
    {
        return true;
    }
    public override void updateHumanAmountText()
    {

        room.humanAmount.text = $"{humans.Count}";
    }
}
