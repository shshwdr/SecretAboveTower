using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    AudioSource sfxSource;
    public void Play(string name)
    {
        sfxSource.PlayOneShot(Resources.Load<AudioClip>("SFX/" + name));
    }
    //public void Play(string )
}
