using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Collections;

public class BurgerSpawner: Singleton<BurgerSpawner>
{
    #region Public GameObjects

    // [Header("")]

    #endregion

    #region Public Variables

    [Header("Time to spawn a burger")]
    public float timerInterval = 0.2f; 
   
    [Header("Maximum burgers spawned")]
    public int maxBurgers = 3;

    #endregion

    #region Private Variables

    private ServeRightPlayerController playerController;
    private int burgersCounter;
    
    #endregion

    #region Events
    public event Action<bool> OnBurgerPickedUp;
    
    public event Action<bool> OnBurgerSpawned;
    IEnumerator SpawnBurger()
    {
        while(true) 
         { 
             if(burgersCounter < maxBurgers) {
                burgersCounter++;
                OnBurgerSpawned(true);
             }
             yield return new WaitForSeconds(timerInterval);
         }
    }

    #endregion

    #region Event Listeners
        
        #region Burger booth hit event

        /// <summary>
        /// Listens when a burger gets picked-up
        /// Decrease burger counter
        /// Remove sprite Item
        /// </summary>
        public void OnBurgerBoothHit(bool difff)
        {
            Debug.Log(difff);
            if(burgersCounter > 0) {
                burgersCounter --;
                OnBurgerPickedUp(true);
            }
        }

        #endregion

    #endregion

    #region  Unity Monobehaviour 

    protected BurgerSpawner() { }

    private void Start()
    {
        playerController = ServeRightPlayerController.Instance;
        playerController.OnBurgerBoothHit += OnBurgerBoothHit;

        StartCoroutine(SpawnBurger());
    }

    void OnApplicationQuit()
    {
         playerController.OnBurgerBoothHit -= OnBurgerBoothHit;
    }

    #endregion
}