using Doozy.Engine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : BaseView
{
    // Start is called before the first frame update
    public override void showView()
    {
        base.showView();
        GetComponent<UIView>().Show();
    }
    public override void hideView()
    {
        GetComponent<UIView>().Hide();
        base.hideView();

    }
}
