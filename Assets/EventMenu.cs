using Doozy.Engine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventButtonInfo
{
    public string text;
    public string resultText;
    public Action action;

    public EventButtonInfo(string t, string r,Action a)
    {
        text = t;
        action = a;
        resultText = r;
    }
}
public class EventMenu : BaseView
{

    public Text text;
    public List<Button> reactButtons;

    List<EventButtonInfo> buttonInfo;
    public override void showView()
    {
        base.showView();
        GetComponent<UIView>().Show();
    }

    public override void hideView()
    {

        base.hideView();
        Destroy(gameObject);

        GetComponent<UIView>().Hide();


    }

    void clearButton()
    {
        foreach(var button in reactButtons)
        {
            button.onClick.RemoveAllListeners();
            button.gameObject.SetActive(false);
        }
    }

    static public void showEvent(string t, List<EventButtonInfo> bf, bool isTutorial = false)
    {
        var go = Instantiate(Resources.Load<GameObject>("Event"),GameObject.Find("MainCanvas").transform);
        go.GetComponent<EventMenu>().Init(t, bf, isTutorial);

        //go.GetComponent<EventMenu>().showView();
    }

    public void Init(string t, List<EventButtonInfo> bf,bool isTutorial)
    {
        text.text = t;
        buttonInfo = bf;

        clearButton();

        for(int i = 0; i < buttonInfo.Count; i++)
        {
            reactButtons[i].gameObject.SetActive(true);
            reactButtons[i].GetComponentInChildren<Text>().text = buttonInfo[i].text;
            var info = buttonInfo[i];
            reactButtons[i].onClick.AddListener(() =>
            {

                if (info.resultText.Length>0)
                {
                    if (isTutorial)
                    {
                        setupTutorialNextLine(info.resultText);
                    }
                    else
                    {

                        text.text = info.resultText;

                        clearButton();
                        reactButtons[0].gameObject.SetActive(true);
                        reactButtons[0].GetComponentInChildren<Text>().text = "OK";
                        reactButtons[0].onClick.AddListener(() =>
                        {
                            hideView();
                        });
                    }
                }
                else
                {

                    hideView();
                }

                if (info.action != null)
                {

                    info.action();
                }

            });
        }
    }

    public void destroy()
    {
        Destroy(gameObject);
    }

    void setupTutorialNextLine(string resultText)
    {
        var nextLine = TutorialManager.Instance.getNextLine(resultText);
        if (nextLine != null)
        {

            text.text = TutorialManager.Instance.getTutorialLine(nextLine);

            clearButton();
            reactButtons[0].gameObject.SetActive(true);
            reactButtons[0].GetComponentInChildren<Text>().text = "OK";
            reactButtons[0].onClick.AddListener(() =>
            {
                setupTutorialNextLine(nextLine);
            });
        }
        else
        {

            hideView();
        }
    }

}
