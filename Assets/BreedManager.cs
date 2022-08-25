using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreedManager : Singleton<BreedManager>
{

    public List<string> nextMustBreed = new List<string>();

    public List<string> canBreedMutations = new List<string>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addNextMustBreed(string b)
    {
        nextMustBreed.Add(b);
        Debug.Log("next must breed " + b);
    }

    public void addCanBreedMutation(string b)
    {
        canBreedMutations.Add(b);
        Debug.Log("can breed later " + b);
    }

    public void breed(Human h1, Human h2, Human child)
    {
        if (nextMustBreed.Count >0)
        {
            var next = nextMustBreed[0];
            nextMustBreed.RemoveAt(0);
            switch (next)
            {
                case "shortLeg":
                    child.hasShortLegs = true;
                    break;
                case "pincerArm":
                    child.hasPincer = true;
                    break;
                case "tentacleArm":
                    child.hasTentacle = true;
                    break;
                case "breathWithoutHelmet":
                    child.noHelmet = true;
                    break;


            }
            EventManager.Instance.createEventTrigger(next);

        }
        child.init();
    }
}
