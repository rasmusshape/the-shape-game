using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class OrderManager : Singleton<OrderManager> {

    #region Public Variables

    [Header("Maximum item an order can have")]
    public int maxItemsPrOrder = 3;

    [Header("Time to deliver an Item of an Order")]
    public float timeToDeliverItem = 1.0f;

    [Header("Points per order's item")]
    public int pointsPerItem = 10;

    [Header("After how many difficulty lvls the number of max orders starts increasing")]
    public int orderDifficultyInterval = 10;

    #endregion

    #region Private variables

    private int currentMinItemsPrOrder = 1;
    private int currentMaxItemsPrOrder = 1;
    private int orderDifficulty = 1;
    private DifficultyManager difficultyManager;
    private List<Item> items;
    private int ordersPointer = 0;

    #endregion

    #region Events

    public event Action<int> OnOrderExpired;

    public void FireOrderExpiredEvent(int orderId)
    {
        OnOrderExpired(orderId);
    }

    #endregion

    #region Difficulty Event Listener

    public void OnDifficultyIncreased(int currentDifficulty)
    {
        if(currentMaxItemsPrOrder < maxItemsPrOrder)
        {
            if(currentDifficulty >= orderDifficulty * orderDifficultyInterval)
            {
                orderDifficulty++;
                currentMaxItemsPrOrder++;
            }
        }
    }

    #endregion

    #region  Unity Monobehaviour 

    protected OrderManager() { }

    private void Awake() {
        items = new List<Item> {
            new Item(ItemType.Burger),
            new Item(ItemType.Beer)
        };

        difficultyManager = DifficultyManager.Instance;
        difficultyManager.OnDifficultyLvlChange += OnDifficultyIncreased;
    }

    void OnApplicationQuit()
    {
        difficultyManager.OnDifficultyLvlChange -= OnDifficultyIncreased;
    }

    #endregion

    public Order GenerateRandomOrder(GameObject shaper) {
        int itemsForOrder = UnityEngine.Random.Range(currentMinItemsPrOrder, currentMaxItemsPrOrder);

        Order newOrder = shaper.AddComponent(typeof(Order)) as Order;

        for (int i = 0; i < itemsForOrder; i++) newOrder.AddItemToOrder(GetRandomItem());

        ordersPointer++;
        newOrder.orderId = ordersPointer;

        return newOrder;
    }

    private Item GetRandomItem() {
        return items[UnityEngine.Random.Range(0, items.Count)];
    }
}