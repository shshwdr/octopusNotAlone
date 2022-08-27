using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BreedManager : Singleton<BreedManager>
{

    public List<string> nextMustBreed = new List<string>();

    public HashSet<string> canBreedMutations = new HashSet<string>();

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
            EventManager.Instance.createEventTrigger(next);
            mutate(child, next);
        }
        else
        {
            if(!h1 || !h2)
            {
                //just generate normal human.
                generalBreed(child);
            }
            else
            {
                //get value from parent
                cloneFromParent(h1, child);
                cloneFromParent(h2, child);
                //general
                generalBreed(child);
            }

        }
        child.init();
    }

    void cloneFromParent(Human parent, Human child)
    {
        if (parent.hasPincer && Random.Range(0f, 1f) <= GameManager.Instance.mutationParentRate)
        {
            child.hasPincer = true;
        }
        if (parent.hasTentacle && Random.Range(0f, 1f) <= GameManager.Instance.mutationParentRate)
        {
            child.hasTentacle = true;
        }
        if (parent.hasShortLegs && Random.Range(0f, 1f) <= GameManager.Instance.mutationParentRate)
        {
            child.hasShortLegs = true;
        }
        if (parent.noHelmet && Random.Range(0f, 1f) <= GameManager.Instance.mutationParentRate)
        {
            child.noHelmet = true;
        }
    }

    void mutate(Human child, string next)
    {

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
    }

    void generalBreed( Human child)
    {
        bool willMutate = Random.Range(0f,1f) < GameManager.Instance.mutationGeneralRate;
        if(willMutate && canBreedMutations.Count > 0)
        {
            var mutation = canBreedMutations.ElementAt(Random.Range(0, canBreedMutations.Count));
            mutate(child, mutation);
        }
    }
}
