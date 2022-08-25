using Doozy.Engine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventButtonInfo
{
    public string text;
    public Action action;

    public EventButtonInfo(string t, Action a)
    {
        text = t;
        action = a;
    }
}
public class EventMenu : BaseView
{

    public Text text;
    public List<Button> reactButtons;
    public override void showView()
    {
        base.showView();
        GetComponent<UIView>().Show();
    }

    public override void hideView()
    {
        base.hideView();

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
    public void Init(string t, List<EventButtonInfo> buttonInfo)
    {
        text.text = t;

        clearButton();

        for(int i = 0; i < buttonInfo.Count; i++)
        {
            reactButtons[i].gameObject.SetActive(true);
            reactButtons[i].GetComponentInChildren<Text>().text = buttonInfo[i].text;
            reactButtons[i].onClick.AddListener(() =>
            {
                buttonInfo[i].action();
                hideView();
            });
        }
    }
}
