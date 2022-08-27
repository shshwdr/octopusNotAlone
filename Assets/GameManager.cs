using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public int calculateTime = 20;
    public int happyDecreaseBasePerRound = 20;
    public int happyIncreaseRound = 5;
    public int happyIncreasePerTime = 20;
    public int foodDecreaseBasePerRoundPerPerson = 1;



    public float foodGenerateTime = 1f;
    public float originalFoodGenerateAmount = 1f;

    public float criticalGenerateTime = 1f;
    public float originalCriticalGenerateAmount = 1f;


    public float childGenerateTime = 1f;

    public float humanOriginalMaxEnergy = 20;
    public float humanEnergyReduceTime = 1;
    public float humanEnergyReduceAmount = 0.5f;
    public float humanEnergyIncreaseAmount = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
