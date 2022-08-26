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

    public Renderer spriteRender;


    public bool hasPincer;
    public bool hasTentacle;
    public bool hasShortLegs;
    public bool noHelmet; 
    public Spine.AnimationState spineAnimationState;

    [Header("Transitions")]
    [SpineAnimation]
    public string idleAnimationName;
    [Header("Transitions")]
    [SpineAnimation]
    public string workAnimationName;

    public Image energyFillImage;

    SkeletonAnimation skeletonAnimation;
    public AreaBase currentArea;
    string workType = "rest";
    // Start is called before the first frame update
    void Awake()
    {
        energy = maxEnergy;
        energyFillImage.fillAmount = energy / maxEnergy;
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
    public void startWorking(AreaBase area)
    {
        currentArea = area;
        string workType = currentArea.room.workType;
        this.workType = workType;
        updateAnimation();
        //switch (workType)
        //{
        //    case "critical":
        //        break;
        //    case "rest":
        //        break;

        //}

    }

    void updateAnimation()
    {
        if(workType == "rest")
        {

            spineAnimationState.SetAnimation(0, idleAnimationName, true);
        }
        else
        {

            spineAnimationState.SetAnimation(0, workAnimationName, true);
        }
    }
}
