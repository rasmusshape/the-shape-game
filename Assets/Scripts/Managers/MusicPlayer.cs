using UnityEngine;
using System;

public class MusicPlayer : Singleton<MusicPlayer>
{
    #region Music

    [Header("Music")]
    public AudioClip menuTheme;
    public AudioClip hecticArcadeTheme;
    
    #endregion

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        // Persistent singleton
        MusicPlayer[] players = FindObjectsOfType<MusicPlayer>();
        if (players.Length > 1) Destroy(players[1].gameObject);
        
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
        
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