using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : Singleton<TutorialManager>
{
    public bool shoulShowTutorial = true;
    Dictionary<string, string> tutorialString = new Dictionary<string, string>()
    {
        {"first_0","I'm living on this planet alone for decades." },
        {"first_00","One day I accidentally caught some minion who called themselves human. So I start to raise them on my planet, hope they can make me not alone." },
       // {"first_000","" },
        {"first_000","Try drag one to the right place to let them perform and make you happy." },


        {"afterHappy_0","Look at it. What a cute way to kick its little feet. Hahaha." },
        {"afterHappy_00","Well although they are cute, I'd prefer them to be able to survive themselves. I don't have all day to prepare food for them." },
        //{"afterHappy_000","" },
        {"afterHappy_000","Try drag one to the left place to let them make food for themselves." },


        {"afterFood_0","Nice! I can check on the top left for the current status of my happiness and food left. Don't let it go too low." },
        {"afterFood_00","Now I think they just need to breed more tiny human next. It is kind of silly that they need two to breed child." },
        {"afterFood_000","Try drag two to the upper place to let them breed more." },

        {"afterBreed_0","That's everything I can do to those tiny guys. Watch out when they spawn too many, I can only have certain number of them on my planet now, so those who are exceed the number will be killed." },
        {"afterBreed_00","If they make me happy enough, I can upgrade to have more of them. But if not, I can also drag some to the bottom to kill them manually." },
        {"afterBreed_000","Anyway they are just... pets? I have them for fun, not for making myself busy." },


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

        EventMenu.showEvent(tutorialString[name], eventButtons,null, true);
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
