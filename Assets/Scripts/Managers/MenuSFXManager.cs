using System;
using UnityEngine;

public class MenuSFXManager : Singleton<MenuSFXManager>
{
    [Header("SFX")] 
    public AudioClip startGameSFX;
    public AudioClip clickButtonSFX;
    public float sfxVolume = 0.5f;

    public void PlayStartGameSFX(bool flag)
    {
        AudioSource.PlayClipAtPoint(
            startGameSFX,
            Camera.main.transform.position,
            sfxVolume
        );
    }
    
    public void PlayButtonClickSFX(bool flag)
    {
        AudioSource.PlayClipAtPoint(
            clickButtonSFX,
            Camera.main.transform.position,
            sfxVolume
        );
    }
}