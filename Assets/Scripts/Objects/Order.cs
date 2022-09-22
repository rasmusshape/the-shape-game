using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order: MonoBehaviour {

    #region Public Variables

    public List<Item> orderItems = new List<Item>();
    public int points = 0;
    public float timer = 0f;
    public int orderId = 0;

    public void AddItemToOrder(Item item)
    {
        orderItems.Add(item);
        points += orderManager.pointsPerItem;
    }

    public bool RemoveItemFromOrder(Item item)
    {
        Item itemFound = orderItems.Find(orderItem => orderItem.ItemType == item.ItemType);

        if (itemFound == null) return false;

        orderItems.Remove(itemFound);
        return true;
    }

    #endregion

    #region Private Variables

    private OrderManager orderManager;
    private ShapersSpawner shapersSpawner;

    #endregion

    #region Event Listeners

    #region Order Spawned Event

    public void OnOrderSpawned(int id)
    {
        if (orderId == id)
        {
            timer = orderItems.Count * orderManager.timeToDeliverItem;
            StartCoroutine(Countdown());
        }
    }

    private IEnumerator Countdown()
    {
        float totalTime = 0;
        while (totalTime < timer)
        {
            //countdownImage.fillAmount = totalTime / timer;
            totalTime += Time.deltaTime;
            yield return null;
        }
        orderManager.FireOrderExpiredEvent(orderId);
    }

    #endregion

    #endregion

    #region  Unity Monobehaviour 

    private void Awake()
    {
        orderManager = OrderManager.Instance;
        shapersSpawner = ShapersSpawner.Instance;

        shapersSpawner.OnOrderSpawned += OnOrderSpawned;
    }

    void OnApplicationQuit()
    {
        shapersSpawner.OnOrderSpawned -= OnOrderSpawned;
    }

    #endregion

}