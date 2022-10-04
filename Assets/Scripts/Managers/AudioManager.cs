using UnityEngine;
using System;

public class AudioManager : Singleton<AudioManager>
{
    // Background music for menus
    // SFX for menus
    private void Awake()
    {
        GetComponent<AudioSource>().Play();
        DontDestroyOnLoad(gameObject);
    }
    
}