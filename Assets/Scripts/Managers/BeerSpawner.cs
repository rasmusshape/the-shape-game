using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Collections;

public class BeerSpawner: Singleton<BeerSpawner>
{
    #region Public GameObjects

    // [Header("")]

    #endregion

    #region Public Variables

    [Header("Time to spawn a beer")]
    public float timerInterval = 0.2f; 
   
    [Header("Maximum beers spawned")]
    public int maxBeers = 3;

    #endregion

    #region Private Variables

    private ServeRightPlayerController playerController;
    private int beersCounter;
    
    #endregion

    #region Events
    public event Action<bool> OnBeerPickedUp;
    
    public event Action<bool> OnBeerSpawned;
    IEnumerator SpawnBeer()
    {
        while(true) 
         { 
             if(beersCounter < maxBeers) {
                beersCounter++;
                OnBeerSpawned(true);
             }
             yield return new WaitForSeconds(timerInterval);
         }
    }

    #endregion

    #region Event Listeners
        
        #region Beer booth hit event

        /// <summary>
        /// Listens when a beer gets picked-up
        /// Decrease beer counter
        /// Remove sprite Item
        /// </summary>
        public void OnBeerBoothHit(bool difff)
        {
            Debug.Log(difff);
            if(beersCounter > 0) {
                beersCounter --;
                OnBeerPickedUp(true);
            }
        }

        #endregion

    #endregion

    #region  Unity Monobehaviour 

    protected BeerSpawner() { }

    private void Start()
    {
        playerController = ServeRightPlayerController.Instance;
        playerController.OnBeerBoothHit += OnBeerBoothHit;

        StartCoroutine(SpawnBeer());
    }

    void OnApplicationQuit()
    {
         playerController.OnBeerBoothHit -= OnBeerBoothHit;
    }

    #endregion
}