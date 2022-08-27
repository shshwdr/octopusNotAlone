using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : Singleton<TutorialManager>
{
    public bool shoulShowTutorial = true;
    Dictionary<string, string> tutorialString = new Dictionary<string, string>()
    {
        {"first_0","You are living on this planet alone for decades." },
        {"first_00","One day you accidentally caught some minion who called themselves human." },
        {"first_000","You start to raise them on  your planet, those little things really can make you happy." },
        {"first_0000","Try drag one to the right place to let them perform and make you happy." },


    };

    Dictionary<string, bool> isTutorialFinished = new Dictionary<string, bool>();

    public void showTutorial(string name)
    {
        if (!shoulShowTutorial)
        {
            return;
        }

        if (isTutorialFinished.ContainsKey(name))
        {
            return;
        }
        if (!tutorialString.ContainsKey(name))
        {
            Debug.LogError("no " + name);
            return;
        }


        List<EventButtonInfo> eventButtons = new List<EventButtonInfo>();
        eventButtons.Add(new EventButtonInfo("OK", name, ()=> {
            isTutorialFinished[name] = true;

            //if (tutorialString.ContainsKey(name + "0"))
            //{
            //    showTutorial(name + "0");
            //}
           // StartCoroutine(show(name));
        }
        ));

        EventMenu.showEvent(tutorialString[name], eventButtons,true);
    }
    void Start()
    {
        showTutorial("first_0");
    }


    public string getNextLine(string lastLine)
    {
        if (tutorialString.ContainsKey(lastLine + "0"))
        {
            return lastLine + "0";
        }
        return null;
    }

    public string getTutorialLine(string line)
    {

        return tutorialString[line];
    }

    IEnumerator show(string name)
    {
        yield return new WaitForSeconds(0.3f);
        if (tutorialString.ContainsKey(name + "0"))
        {
            showTutorial(name + "0");
        }
    }
}
