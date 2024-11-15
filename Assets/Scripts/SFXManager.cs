using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : Singleton<SFXManager>
{
    AudioSource sfxSource;
    protected override void Awake()
    {
        base.Awake();
        sfxSource = GetComponent<AudioSource>();
    }

    public void Play(string name)
    {
        sfxSource.PlayOneShot(Resources.Load<AudioClip>("SFX/" + name));
    }

    public void Play(AudioClip clip)
    {
        if (!clip)
        {
            return;
        }
        sfxSource.PlayOneShot(clip);
    }
    //public void Play(string )
    public void PlayCardSelection()
    {
        Play("UI_Cards_Building_Incoming_V2");
    }
    public void PlayBuffSelection()
    {
        Play("UI_Cards_Building_Incoming_V1");
    }
    
    
    public void PlayUpgradeSynergy()
    {
        Play("UI_Cards_Building_Incoming_V1");
    }
    
    public void PlayFinishMilestone()
    {
        Play("UI_Cards_Building_Incoming_V1");
    }

    public void PlayBuilding()
    {
  Play("SFX_Fantasy_Construction_01");      
    }
}
