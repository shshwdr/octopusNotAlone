using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Human : MonoBehaviour
{
    public float maxEnergy = 20;
    public float energyReduceTime = 1;
    public float energyReduceAmount = 0.5f;
    public float energyIncreaseAmount = 1f;
    float energyTimer = 0;
    public float energy;


    public bool hasPincer;
    public bool hasTentacle;
    public bool hasShortLegs;
    public bool noHelmet;

    public Image energyFillImage;

    SkeletonAnimation skeletonAnimation;

    string workType = "rest";
    // Start is called before the first frame update
    void Awake()
    {
        energy = maxEnergy;
        energyFillImage.fillAmount = energy / maxEnergy;
        skeletonAnimation = GetComponentInChildren<SkeletonAnimation>();
    }

    public void init()
    {
        var skeleton = skeletonAnimation.Skeleton;
        var skeletonData = skeleton.Data;
        var mixAndMatchSkin = new Skin("base_spacesuit");
        mixAndMatchSkin.AddSkin(skeletonData.FindSkin("base_spacesuit"));

        if (hasPincer)
        {
            mixAndMatchSkin.AddSkin(skeletonData.FindSkin("hand_left_pincer"));
        }
        if (hasTentacle)
        {
            mixAndMatchSkin.AddSkin(skeletonData.FindSkin("hand_right_tentacle"));
        }
        if (hasShortLegs)
        {
            mixAndMatchSkin.AddSkin(skeletonData.FindSkin("legs_dinosaur"));
        }
        if (noHelmet)
        {
            mixAndMatchSkin.AddSkin(skeletonData.FindSkin("head_nohelmet"));
        }


        skeleton.SetSkin(mixAndMatchSkin);
        skeleton.SetSlotsToSetupPose();

    }

    // Update is called once per frame
    void Update()
    {
        //test
        if (Input.GetKeyDown(KeyCode.A))
        {
            
        }


        if (this.workType.Length != 0)
        {

            energyTimer += Time.deltaTime;
            if (energyTimer >= energyReduceTime)
            {
                energyTimer = 0;
                if (this.workType == "rest")
                {
                    energy+= energyIncreaseAmount;
                    energy = Mathf.Min(energy, maxEnergy);
                }
                else
                {
                    energy-= energyReduceAmount;
                    if (energy <= 0)
                    {
                        die();
                    }
                }
                energyFillImage.fillAmount = energy / maxEnergy;
            }
        }

    }

    void die()
    {
        var parentRoom = GetComponentInParent<AreaBase>();
        if (parentRoom)
        {
            parentRoom.removeHuman(this);
            RoomsAndHumanManager.Instance.removeHuman(this);
        }
        else
        {
            Debug.LogError("no room?");
            RoomsAndHumanManager.Instance.removeHuman(this);
        }
        Destroy(gameObject);
    }

    public void stopWorking()
    {
        this.workType = "";
        energyTimer = 0;
    }
    public void startWorking(string workType)
    {
        this.workType = workType;
        //switch (workType)
        //{
        //    case "critical":
        //        break;
        //    case "rest":
        //        break;

        //}

    }
}
