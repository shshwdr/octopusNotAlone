using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PopupType { upAndDown, jumpRandomly }

public class PopupTextManager : Singleton<PopupTextManager>
{
    [SerializeField] GameObject animatedCoinPrefab;
    [SerializeField] int maxCoins;
    Queue<GameObject> coinsQueue = new Queue<GameObject>();
    [SerializeField] Vector3 jumpVector;
    [SerializeField] float jumpPower;
    // Start is called before the first frame update
    void Awake()
    {


        //prepare pool
        PrepareCoins();
    }
    void PrepareCoins()
    {
        GameObject coin;
        for (int i = 0; i < maxCoins; i++)
        {
            coin = Instantiate(animatedCoinPrefab);
            coin.transform.parent = transform;
            coin.SetActive(false);
            coinsQueue.Enqueue(coin);
        }
    }
    public void ShowPopupString(Vector3 collectedCoinPosition, string text, float scale = 3f, float time = 2f, PopupType popupType = PopupType.upAndDown)
    {

        MoveSource(collectedCoinPosition, text, scale, time, popupType, () =>
        {
            //PlantsManager.Instance.currentResource[pair.Key] += 1;
            //BeeManager.Instance.updateGenerateTime();
            //PestManager.Instance.updateGenerateTime();
            //TutorialManager.Instance.collectResource(pair.Key);
        });


    }

    public void ShowPopupNumber(Vector3 collectedCoinPosition, int text, float damageScale, float scale = 1f, float time = 1f, PopupType popupType = PopupType.jumpRandomly)
    {

        if (coinsQueue.Count > 0)
        {
            //extract a coin from the pool
            GameObject coin = coinsQueue.Dequeue();
            coin.transform.position = collectedCoinPosition;
            coin.GetComponentInChildren<DamageNumber>().init(text.ToString(), damageScale);
            coin.SetActive(true);
            //coin.transform.localScale = new Vector3(scale, scale, scale);
            //move coin to the collected coin pos
            //coin.transform.position = start + new Vector3(Random.Range(-spread, spread), 0f, 0f);


            var vector = new Vector3(0, jumpVector.y, 0);
            if (popupType == PopupType.jumpRandomly)
            {
                vector = new Vector3(jumpVector.x * UnityEngine.Random.Range(-1f, 1f), jumpVector.y, 0);
            }
            //coin.GetComponentInChildren<DamageNumber>().numberText.transform.DOScale(new Vector3(scale, scale, scale), 1);
            coin.GetComponentInChildren<DamageNumber>().numberText.transform.DOLocalJump(vector, jumpPower, 1, time).OnComplete(() =>
            {
                coin.SetActive(false);
                coinsQueue.Enqueue(coin);
            }); ;

            //animate coin to target position
            //float duration = Random.Range(minAnimDuration, maxAnimDuration);
            //coin.transform.DOMove(end, duration)
            //.SetEase(easeType)
            //.OnComplete(() =>
            //{
            //    //executes whenever coin reach target position
            //    coin.SetActive(false);
            //    coinsQueue.Enqueue(coin);
            //    action();
            //});
        }
        //MoveSource(collectedCoinPosition, text.ToString(), scale, time, popupType, () =>
        //{
        //    //PlantsManager.Instance.currentResource[pair.Key] += 1;
        //    //BeeManager.Instance.updateGenerateTime();
        //    //PestManager.Instance.updateGenerateTime();
        //    //TutorialManager.Instance.collectResource(pair.Key);
        //});
    }

    void MoveSource(Vector3 start, string value, float scale, float time, PopupType popupType, Action action)
    {
        //check if there's coins in the pool
        if (coinsQueue.Count > 0)
        {
            //extract a coin from the pool
            GameObject coin = coinsQueue.Dequeue();
            coin.transform.position = start;
            coin.GetComponentInChildren<DamageNumber>().init(value);
            coin.SetActive(true);
            //coin.transform.localScale = new Vector3(scale, scale, scale);
            //move coin to the collected coin pos
            //coin.transform.position = start + new Vector3(Random.Range(-spread, spread), 0f, 0f);


            var vector = new Vector3(0, jumpVector.y, 0);
            if (popupType == PopupType.jumpRandomly)
            {
                vector = new Vector3(jumpVector.x * UnityEngine.Random.Range(0, 1), jumpVector.y, 0);
            }
            //coin.GetComponentInChildren<DamageNumber>().numberText.transform.DOScale(new Vector3(scale, scale, scale), 1);
            coin.GetComponentInChildren<DamageNumber>().numberText.transform.DOLocalJump(vector, jumpPower, 1, time).OnComplete(() =>
            {
                coin.SetActive(false);
                coinsQueue.Enqueue(coin);
            }); ;

            //animate coin to target position
            //float duration = Random.Range(minAnimDuration, maxAnimDuration);
            //coin.transform.DOMove(end, duration)
            //.SetEase(easeType)
            //.OnComplete(() =>
            //{
            //    //executes whenever coin reach target position
            //    coin.SetActive(false);
            //    coinsQueue.Enqueue(coin);
            //    action();
            //});
        }

    }
}
