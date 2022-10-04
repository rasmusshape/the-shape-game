using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Collections;

public class BeerSpawner: Singleton<BeerSpawner>
{
    #region Public GameObjects

    [SerializeField] GameObject beerImages;
    [SerializeField] GameObject beerPrefab;

    #endregion

    #region Public Variables

    [Header("Time to spawn a beer")]
    public float timerInterval = 2f; 
   
    [Header("Maximum beers spawned")]
    public int maxBeers = 3;

    #endregion

    #region Private Variables

    private ServeRightPlayerController playerController;
    private InventoryManager inventoryManager;
    private int beersCounter;
    
    #endregion

    #region Events
    public event Action<bool> OnBeerPickedUp;
    
    //public event Action<bool> OnBeerSpawned;
    IEnumerator SpawnBeer()
    {
        while(true) 
         { 
             if(beersCounter < maxBeers) {
                beersCounter++;
                AddBeerToUI();
                //OnBeerSpawned(true);
             }
             yield return new WaitForSeconds(timerInterval);
         }
    }

    private void AddBeerToUI()
    {
        Instantiate(beerPrefab).transform.SetParent(beerImages.transform);
    }

    #endregion

    #region Event Listeners

    #region Beer booth hit event

    /// <summary>
    /// Listens when a beer gets picked-up
    /// Decrease beer counter
    /// Remove sprite Item
    /// </summary>
    public void OnBeerBoothHit(bool isHit)
    {
        if(AbleToPickUp()) {
            beersCounter --;
            RemoveBeerFromUI();
            OnBeerPickedUp(true);
        }
    }

    private bool AbleToPickUp()
    {
        return beersCounter > 0 && inventoryManager.items.Count < inventoryManager.maxInventoryCount;
    }

    private void RemoveBeerFromUI()
    {
        int childCount = beerImages.transform.childCount;

        if (childCount > 0)
        {
            Destroy(beerImages.transform.GetChild(childCount - 1).gameObject);
        }
    }

    #endregion

    #endregion

    #region  Unity Monobehaviour 

    protected BeerSpawner() { }

    private void Start()
    {
        inventoryManager = InventoryManager.Instance;
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