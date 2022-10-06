using UnityEngine;
using System;

public class ServeRightSFXManager : MonoBehaviour
{
    #region SFX Files
    [Header("SFX audio clips")]
    public AudioClip itemPickedUpSFX;
    public AudioClip itemDeliveredSFX;
    public AudioClip orderCompletedSFX;
    public AudioClip orderExpiredSFX;
    public AudioClip playShaperSpawnedSFX;
    public AudioClip gameOverSFX;

    [Header("Audio Settings")] 
    public float sfxVolume = 1;
    #endregion

    #region Private Variables
    private OrderManager orderManager;
    private BeerSpawner beerSpawner;
    private BurgerSpawner burgerSpawner;
    private ShapersSpawner shaperSpawner;
    #endregion
    
    public bool isAudioEnabled = true;
    
    private void Start()
    {
        orderManager = OrderManager.Instance;
        beerSpawner = BeerSpawner.Instance;
        burgerSpawner = BurgerSpawner.Instance;
        shaperSpawner = ShapersSpawner.Instance;

        orderManager.OnOrderExpired += PlayOrderExpiredSFX;
        beerSpawner.OnBeerPickedUp += PlayItemPickedUpSFX;
        burgerSpawner.OnBurgerPickedUp += PlayItemPickedUpSFX;
        shaperSpawner.OnItemDelivered += PlayItemDeliveredSFX;
        shaperSpawner.OnOrderDelivered += PlayOrderCompletedSFX;
        shaperSpawner.OnOrderSpawned += PlayShaperSpawnedSFX;
        ServeRightGameManager.Instance.OnGameOver += PlayGameOverSFX;

    }

    #region SFX functions
    private void PlayItemPickedUpSFX(bool wasAdded)
    {
        PlaySFX(itemPickedUpSFX);
    }

    private void PlayOrderExpiredSFX(int orderId)
    {
        PlaySFX(orderExpiredSFX);
    }
    
    private void PlayItemDeliveredSFX(ItemType type)
    {
        PlaySFX(itemDeliveredSFX);
    }
    
    private void PlayOrderCompletedSFX(int orderId)
    {
        PlaySFX(orderCompletedSFX);
    }
    
    private void PlayShaperSpawnedSFX(int orderId, Shaper shaper)
    {
        PlaySFX(playShaperSpawnedSFX);
    }
    
    private void PlayGameOverSFX(bool flag)
    {
        PlaySFX(gameOverSFX);
    }
    #endregion

    #region Supporting functions

    public void ToggleAudio()
    {
        isAudioEnabled = !isAudioEnabled;
    }
    
    private void PlaySFX(AudioClip sfxClip)
    {
        if (isAudioEnabled)
        {
            AudioSource.PlayClipAtPoint(
                sfxClip,
                Camera.main.transform.position,
                sfxVolume
            );
        }
    }

    #endregion
     
    
    
}