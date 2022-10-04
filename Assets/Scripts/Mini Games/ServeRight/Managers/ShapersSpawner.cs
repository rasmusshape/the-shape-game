using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShapersSpawner : Singleton<ShapersSpawner>
{
    #region Public GameObjects

    [Header("The positions of the incative shapers")]
    public List<Shaper> inactiveShapers;

    [Header("The positions of the active shapers")]
    public List<Shaper> activeShapers;

    #endregion

    #region Public Variables

    [Header("Time passed to spawn an order")]
    public float timerInterval = 5f;

    [Header("max shapers allowed to order. Max 8")]
    public int maxShapers = 1;

    [Header("After how many difficulty lvls the number of max shapers starts increasing")]
    public int shapersDifficultyInterval = 10;

    #endregion

    #region Private Variables

    private DifficultyManager difficultyManager;
    private ServeRightPlayerController playerController;
    private OrderManager orderManager;
    private InventoryManager inventoryManager;
    private int shapersDiffficulty = 1;

    #endregion

    #region Events

    public event Action<int> OnOrderDelivered;
    public event Action<ItemType> OnItemDelivered;
    public event Action<int, Shaper> OnOrderSpawned;

    IEnumerator SpawnOrder()
    {
        while (true)
        {
            //find random inactive Shaper
            if (inactiveShapers.Count > 0 && activeShapers.Count < maxShapers)
            {
                Shaper shaperPicked = inactiveShapers[UnityEngine.Random.Range(0, inactiveShapers.Count)].GetComponent<Shaper>();
                //Add order to shaper and Activate Shaper
                shaperPicked.gameObject.SetActive(true);
                inactiveShapers.Remove(shaperPicked);
                activeShapers.Add(shaperPicked);

                shaperPicked.order = orderManager.GenerateRandomOrder(shaperPicked.gameObject);
                OnOrderSpawned(shaperPicked.order.orderId, shaperPicked);
            }
            yield return new WaitForSeconds(timerInterval);
        }
    }

    #endregion

    #region Event Listeners

    #region Difficulty Event

    public void OnDifficultyIncreased(int currentDifficulty)
    {
        if (maxShapers < 8)
        {
            if (currentDifficulty >= shapersDiffficulty * shapersDifficultyInterval)
            {
                shapersDiffficulty++;
                maxShapers++;
            }
        }
    }

    #endregion

    #region Shaper Hit Event

    public void OnShaperHit(int shaperId)
    {
        Shaper shaperPicked = findPickedShaper(shaperId);
        if (shaperPicked.order != null)
        {

            foreach (Item item in inventoryManager.items)
            {
                bool result = shaperPicked.order.RemoveItemFromOrder(item);
                if (result)
                {
                    OnItemDelivered(item.ItemType);
                    break;
                }
            }

            if (shaperPicked.order.orderItems.Count == 0)
            {
                activeShapers.Remove(shaperPicked);
                inactiveShapers.Add(shaperPicked);
                OnOrderDelivered(shaperPicked.order.points);
                ClearOrderLineVisuals(shaperPicked);
                Destroy(shaperPicked.gameObject.GetComponent<Order>());
                shaperPicked.gameObject.SetActive(false);
            }
        }
    }

    private void ClearOrderLineVisuals(Shaper shaperPicked)
    {
        foreach (Transform child in shaperPicked.orderLine.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private Shaper findPickedShaper(int pickedId)
    {
        Shaper result = null;
        List<Shaper> shapers = new List<Shaper>();
        shapers.AddRange(activeShapers);
        shapers.AddRange(inactiveShapers);

        foreach (Shaper shaper in shapers)
        {
            if (shaper.id == pickedId) result = shaper;
        }
        return result;
    }
    
    #endregion
    
    #region Order Expired Event
    
    public void OnOrderExpired(int orderId)
    {
        // find the shaper with the orderid and deactivate.
        var shaper = activeShapers.Find(aShaper => aShaper.order.orderId == orderId);
        shaper.gameObject.SetActive(false);
    }
    
    #endregion
    
    #endregion
    
    
    
    #region  Unity Monobehaviour 

    protected ShapersSpawner() { }

    private void Start()
    {
        inventoryManager = InventoryManager.Instance;
        difficultyManager = DifficultyManager.Instance;
        orderManager = OrderManager.Instance;
        playerController = ServeRightPlayerController.Instance;

        difficultyManager.OnDifficultyLvlChange += OnDifficultyIncreased;
        playerController.OnShaperHit += OnShaperHit;
        orderManager.OnOrderExpired += OnOrderExpired;

        StartCoroutine(SpawnOrder());
    }

    void OnApplicationQuit()
    {
        difficultyManager.OnDifficultyLvlChange -= OnDifficultyIncreased;
        playerController.OnShaperHit -= OnShaperHit;
    }

    #endregion
}