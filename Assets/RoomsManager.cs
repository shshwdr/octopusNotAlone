using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomsManager : Singleton<RoomsManager>
{
    public RestArea restArea;
    // Start is called before the first frame update
    void Start()
    {
        restArea = GameObject.FindObjectOfType<RestArea>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
