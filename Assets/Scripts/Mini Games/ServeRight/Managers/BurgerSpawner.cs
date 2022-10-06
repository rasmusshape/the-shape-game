using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Collections;

public class BurgerSpawner: Singleton<BurgerSpawner>
{
    #region Public GameObjects

    [SerializeField] GameObject burgerImages;
    [SerializeField] GameObject burgerPrefab;

    #endregion

    #region Public Variables

    [Header("Time to spawn a burger")]
    public float timerInterval = 2f; 
    public float timerReductionInterval = 0.25f;
   
    [Header("Maximum burgers spawned")]
    public int maxBurgers = 3;

    [Header("Pickup interval when hitting booth")] 
    public float pickupIntervalInSeconds = 0.5f;

    #endregion

    #region Private Variables

    private ServeRightPlayerController playerController;
    private InventoryManager inventoryManager;
    private int burgersCounter;
    [SerializeField] private bool isHittingBurgerBooth;

    #endregion

    #region Events
    public event Action<bool> OnBurgerPickedUp;
    
    //public event Action<bool> OnBurgerSpawned;    
    IEnumerator SpawnBurger()
    {
        while(true) 
        {
            if (burgersCounter < maxBurgers) {
                burgersCounter++;
                AddBurgerToUI();
                //OnBurgerSpawned(true);
            }
            yield return new WaitForSeconds(timerInterval);
        }
    }

    private void AddBurgerToUI()
    {
        Instantiate(burgerPrefab).transform.SetParent(burgerImages.transform);
    }

    #endregion

    #region Event Listeners

    #region Burger booth hit event

    /// <summary>
    /// Listens when a burger gets picked-up
    /// Decrease burger counter
    /// Remove sprite Item
    /// </summary>
    public void OnBurgerBoothHit(bool _)
    {
        isHittingBurgerBooth = true;
        StartCoroutine(PickUpBurger());
    }

    private IEnumerator PickUpBurger()
    {
        while (isHittingBurgerBooth)
        {
            if (AbleToPickUp()) {
                burgersCounter --;
                RemoveBurgerFromUI();
                OnBurgerPickedUp(true);
            }

            yield return new WaitForSeconds(pickupIntervalInSeconds);
        }
    }

    private void OnBurgerBoothExit(bool exit)
    {
        isHittingBurgerBooth = false;
    }

    private bool AbleToPickUp()
    {
        return burgersCounter > 0 && inventoryManager.items.Count < inventoryManager.maxInventoryCount;
    }

    private void RemoveBurgerFromUI()
    {
        int childCount = burgerImages.transform.childCount;

        if (childCount > 0)
        {
            Destroy(burgerImages.transform.GetChild(childCount - 1).gameObject);
        }
    }

    #endregion

    #endregion

    #region  Unity Monobehaviour 

    protected BurgerSpawner() { }

    private void Start()
    {
        inventoryManager = InventoryManager.Instance;
        playerController = ServeRightPlayerController.Instance;

        playerController.OnBurgerBoothHit += OnBurgerBoothHit;
        playerController.OnBurgerBoothExit += OnBurgerBoothExit;
        
        ShapersSpawner.Instance.OnMaxShapers += IncreaseSpawnRate;
        StartCoroutine(SpawnBurger());
    }
    
    public void IncreaseSpawnRate(bool flag)
    {
        if (timerInterval > 0.5f) timerInterval -= timerReductionInterval;
    }

    void OnApplicationQuit()
    {
         playerController.OnBurgerBoothHit -= OnBurgerBoothHit;
    }

    #endregion

}