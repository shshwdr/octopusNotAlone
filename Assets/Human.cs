using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Human : MonoBehaviour
{
    float energyTimer = 0;
    public float energy;

    public Renderer spriteRender;


    public bool hasPincer;
    public bool hasTentacle;
    public bool hasShortLegs;
    public bool noHelmet;
    public Spine.AnimationState spineAnimationState;

    [Header("Transitions")]
    [SpineAnimation]
    public string idleAnimationName;
    [SpineAnimation]
    public string happyAnimationName;
    [SpineAnimation]
    public string foodAnimationName;
    [SpineAnimation]
    public string catchAnimationName;
    [SpineAnimation]
    public string spawnAnimationName;
    [SpineAnimation]
    public string dieAnimationName;
    [SpineAnimation]
    public string fallAnimationName;
    [Header("Transitions")]
    [SpineAnimation]
    public string workAnimationName;

    public Image energyFillImage;

    SkeletonAnimation skeletonAnimation;
    public AreaBase currentArea;
    string workType = "rest";

    public bool isDead = false;
    // Start is called before the first frame update

    public bool isType(string type)
    {
        switch (type)
        {
            case "shortLeg":
                return hasShortLegs;
            case "tentacleArm":
                return hasTentacle;
            case "pincerArm": 
                return hasPincer;
            case "breathWithoutHelmet":
                return noHelmet;


        }
        return false;
    }

    public List<string> types()
    {
        List<string> res = new List<string>();
        if (hasShortLegs)
        {
            res.Add("shortLeg");
        }
        if (hasTentacle)
        {
            res.Add("tentacleArm");
        }
        if (hasPincer)
        {
            res.Add("pincerArm");
        }
        if (noHelmet)
        {
            res.Add("breathWithoutHelmet");
        }
        return res;
    }
    void Awake()
    {
        energy = 1000;
        energyFillImage.fillAmount = 1;
        skeletonAnimation = GetComponentInChildren<SkeletonAnimation>();
        spineAnimationState = skeletonAnimation.AnimationState;
        // var material = Instantiate<Material>(spriteRender.material);
        //var material = new Material(Shader.Find("Spine/Skeleton Fill"));
        //spriteRender.material = material;
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

    public float getMaxEnergy()
    {
                return RoomsAndHumanManager.Instance.getRoomByName("rest").other;
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
            if (energyTimer >= GameManager.Instance.humanEnergyReduceTime)
            {
                energyTimer = 0;
                if (this.workType == "rest")
                {
                    energy += recoverEnergyAmount();
                }
                else
                {
                    energy -= consumeEnergyAmount();
                    if (energy <= 0)
                    {
                        die();
                    }
                }

                energy = Mathf.Min(energy, getMaxEnergy());
                energyFillImage.fillAmount = energy / getMaxEnergy();
            }
        }

    }

    float consumeEnergyAmount()
    {
        var res = GameManager.Instance.humanEnergyReduceAmount;
        if (hasShortLegs)
        {
            res *= 1.5f;
        }
        if (noHelmet)
        {
            res *= 0.7f;
        }
        return res;
    }

    float recoverEnergyAmount()
    {
        var res = GameManager.Instance.humanEnergyIncreaseAmount;
        res *= BuffManager.Instance.getBuffEffects("recover");
        return res;
    }

    public void kill()
    {
        die();
    }

    void die()
    {
        isDead = true;
        updateAnimation("die");
        if (currentArea && currentArea.GetComponent<AreaBase>())
        {
            currentArea.GetComponent<AreaBase>().removeHuman(this);
            RoomsAndHumanManager.Instance.removeHuman(this);
        }
        else
        {
            //Debug.LogError("no room?");
            RoomsAndHumanManager.Instance.removeHuman(this);
        }

        SFXManager.Instance.playdieClip();
        Destroy(gameObject,2);
    }

    public void stopWorking()
    {
        this.workType = "";
        energyTimer = 0;

    }

    public void startCatch()
    {

        updateAnimation("catch");
    }
    public void updateAnimation()
    {
        updateAnimation(workType);
    }
    public float foodGenerateAmount()
    {
        var res = GameManager.Instance.originalFoodGenerateAmount;
        if (hasTentacle)
        {
            res *= 0.5f;
        }
        if (hasPincer)
        {
            res *= 2f;
        }
        res *= BuffManager.Instance.buffEffects["food"];
        return res;
    }

    public float happyGenerateAmount()
    {
        var res = GameManager.Instance.originalFoodGenerateAmount;
        if (hasTentacle)
        {
            res *= 2f;
        }
        if (hasShortLegs)
        {
            res *= 2f;
        }
        if (hasPincer)
        {
            res /= 2f;
        }
        if (noHelmet)
        {
            res /= 2f;
        }
        res *= BuffManager.Instance.buffEffects["happy"];
        return res;
    }

    public float foodConsumeAmount()
    {
        float res = GameManager.Instance.foodDecreaseBasePerRoundPerPerson;

        if (noHelmet)
        {
            res *= 2f;
        }
        res *= BuffManager.Instance.buffEffects["foodConsume"];
        return res;
    }

    public void startWorking(AreaBase area)
    {
        currentArea = area;
        string workType = currentArea.room.workType;
        this.workType = workType;
        updateAnimation(workType);
        //switch (workType)
        //{
        //    case "critical":
        //        break;
        //    case "rest":
        //        break;

        //}

    }

    void updateAnimation(string type)
    {
        switch (type)
        {
            case "rest":
                spineAnimationState.SetAnimation(0, idleAnimationName, true);
                break;
            case "critical":
                spineAnimationState.SetAnimation(0, happyAnimationName, true);
                break;
            case "food":
                spineAnimationState.SetAnimation(0, foodAnimationName, true);
                break;
            case "breed":
                spineAnimationState.SetAnimation(0, spawnAnimationName, true);
                break;
            case "catch":
                spineAnimationState.SetAnimation(0, catchAnimationName, false);
                break;
            case "die":
                spineAnimationState.SetAnimation(0, dieAnimationName, false);
                break;


        }
    }
}
