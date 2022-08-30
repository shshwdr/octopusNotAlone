using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : Singleton<SFXManager>
{
    public AudioClip grab;
    public AudioClip release;
    public AudioClip die;
    public AudioClip upgrade;
    public AudioClip eventShow;
    public AudioClip fail;


    AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void playeventShowClip()
    {
        audioSource.PlayOneShot(eventShow);
    }
    public void playupgradeClip()
    {
        audioSource.PlayOneShot(upgrade);
    }
    public void playfailClip()
    {
        //audioSource.PlayOneShot(fail);
    }
    public void playgrabClip()
    {
        audioSource.PlayOneShot(grab);
    }
    public void playreleaseClip()
    {
        audioSource.PlayOneShot(release);
    }
    public void playdieClip()
    {
        audioSource.PlayOneShot(die);
    }
}
