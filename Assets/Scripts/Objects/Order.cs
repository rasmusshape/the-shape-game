using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order: MonoBehaviour {

    #region Public Variables

    public List<Item> orderItems = new List<Item>();
    public List<GameObject> itemPrefabs = new List<GameObject>();
    public int points = 0;
    public float timer = 0f;
    public int orderId = 0;

    public void AddItemToOrder(Item item, GameObject prefabItem, Transform orderLine)
    {
        orderItems.Add(item);
        itemPrefabs.Add(prefabItem);
        Instantiate(prefabItem, orderLine, false);
        points += orderManager.pointsPerItem;
    }

    public bool RemoveItemFromOrder(Item item)
    {
        Item itemFound = orderItems.Find(orderItem => orderItem.ItemType == item.ItemType);
        GameObject prefabFound = itemPrefabs.Find(prefab => prefab.CompareTag(item.ItemType.ToString()));

        if (itemFound == null || prefabFound == null) return false;

        orderItems.Remove(itemFound);
        Destroy(prefabFound);

        return true;
    }

    #endregion

    #region Private Variables

    private OrderManager orderManager;
    private ShapersSpawner shapersSpawner;

    #endregion

    #region Event Listeners

    #region Order Spawned Event

    public void OnOrderSpawned(int id, Shaper shaper)
    {
        if (orderId == id)
        {
            timer = orderItems.Count * orderManager.timeToDeliverItem;
            StartCoroutine(Countdown(shaper.bubbleSprite));
        }
    }

    private IEnumerator Countdown(SpriteRenderer bubble)
    {
        float totalTime = 0;
        while (totalTime < timer)
        {
            totalTime += Time.deltaTime;
            bubble.color = UnityEngine.Color.Lerp(orderManager.startBubbleColor, orderManager.expiredBubbleColor, totalTime / timer);
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