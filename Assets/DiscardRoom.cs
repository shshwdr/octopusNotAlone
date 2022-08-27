using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscardRoom : AreaBase
{

    public override bool canAddHuman()
    {
        return true;
    }

    public override void addHuman(Human human)
    {
        human.kill();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
