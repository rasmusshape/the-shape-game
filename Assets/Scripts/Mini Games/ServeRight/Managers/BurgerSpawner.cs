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
   
    [Header("Maximum burgers spawned")]
    public int maxBurgers = 3;

    #endregion

    #region Private Variables

    private ServeRightPlayerController playerController;
    private InventoryManager inventoryManager;
    private int burgersCounter;

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
        if(AbleToPickUp()) {
            burgersCounter --;
            RemoveBurgerFromUI();
            OnBurgerPickedUp(true);
        }
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

        StartCoroutine(SpawnBurger());
    }

    void OnApplicationQuit()
    {
         playerController.OnBurgerBoothHit -= OnBurgerBoothHit;
    }

    #endregion

}