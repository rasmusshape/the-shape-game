using UnityEngine;
using System;

public class AudioManager : Singleton<AudioManager>
{
    #region Music

    public AudioClip menuTheme;
    public AudioClip hecticArcadeTheme;

    #endregion

    private AudioSource audioSource;
    // Background music for menus
    // SFX for menus
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
        DontDestroyOnLoad(gameObject);
    }

    public void ChangeMusic(MusicType musicType)
    {
        audioSource.Stop();
        
        switch (musicType)
        {
            case MusicType.Hectic:
                audioSource.clip = hecticArcadeTheme;
                break;
            default:
                audioSource.clip = menuTheme;
                break;
        }
        
        audioSource.Play();
    }

    public enum MusicType
    {
        Menu,
        Hectic
    }
}