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
    
    [Header("Pickup interval when hitting booth")] 
    public float pickupIntervalInSeconds = 0.5f;

    #endregion

    #region Private Variables

    private ServeRightPlayerController playerController;
    private InventoryManager inventoryManager;
    private int beersCounter;
    [SerializeField] private bool isHittingBeerBooth;
    
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
        isHittingBeerBooth = true;
        StartCoroutine(PickupBeer());
    }
    
    private IEnumerator PickupBeer()
    {
        while (isHittingBeerBooth)
        {
            if (AbleToPickUp()) {
                beersCounter --;
                RemoveBeerFromUI();
                OnBeerPickedUp(true);
            }

            yield return new WaitForSeconds(pickupIntervalInSeconds);
        }
    }
    
    private void OnBeerBoothExit(bool exit)
    {
        isHittingBeerBooth = false;
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
        playerController.OnBeerBoothExit += OnBeerBoothExit;

        StartCoroutine(SpawnBeer());
    }

    void OnApplicationQuit()
    {
         playerController.OnBeerBoothHit -= OnBeerBoothHit;
    }

    #endregion

}