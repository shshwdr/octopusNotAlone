using DG.Tweening;
using Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HumanView : Singleton<HumanView>
{
    public Text humanText;
    // Start is called before the first frame update
    void Start()
    {
        humanCountChange();
        EventPool.OptIn("humanCountChange", humanCountChange);
    }

    void humanCountChange()
    {
        humanText.text = $"Human: { RoomsAndHumanManager.Instance.humans.Count.ToString()} / {RoomsAndHumanManager.Instance.maxTotalHumanCount}";
    }

    public void tooManyHuman()
    {
        humanText.transform.DOKill();
        humanText.transform.localScale = Vector3.one;
        humanText.color = Color.white;
        humanText.transform.DOShakeScale(0.3f);
        DOTween.To(() => humanText.color, x => humanText.color = x, Color.red, 0.3f).SetLoops(2, LoopType.Yoyo);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
