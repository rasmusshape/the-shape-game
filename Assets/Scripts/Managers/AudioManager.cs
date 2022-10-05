using UnityEngine;
using System;

public class AudioManager : Singleton<AudioManager>
{
    #region Music

    [Header("Music")]
    public AudioClip menuTheme;
    public AudioClip hecticArcadeTheme;

    [Header("SFX")] 
    public AudioClip startGameSFX;
    public AudioClip clickButtonSFX;
    public float sfxVolume = 0.5f;
    

    #endregion

    private AudioSource audioSource;
    // Background music for menus
    // SFX for menus
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
        DontDestroyOnLoad(gameObject);
        FindObjectOfType<UI_MainMenu>().OnMenuButtonClick += PlayButtonClickSFX;
    }

    private void Start()
    {
        FindObjectOfType<SceneTransition>().OnSceneChange += PlayStartGameSFX;
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