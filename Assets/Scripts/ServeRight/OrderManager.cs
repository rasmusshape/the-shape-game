using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class OrderManager : Singleton<OrderManager> {
    
    int currentMinItemsPrOrder = 1;
    int currentMaxItemsPrOrder = 1;
    int maxItemsPrOrder = 5;

    List<Item> items;

 protected OrderManager() { }

    private void Start() {
        items = new List<Item> {
            new Item(ItemType.Burger),
            new Item(ItemType.Beer)
        };
    }

    public Order GetOrder(GameObject shaper) {
        int itemsForOrder = Random.Range(currentMinItemsPrOrder, currentMaxItemsPrOrder);

        Order newOrder = shaper.AddComponent(typeof(Order)) as Order;

        for (int i = 0; i < itemsForOrder; i++) {
            newOrder.AddItemToOrder(GetRandomItem());
        }

        return newOrder;
    }

    public Item GetRandomItem() {
        return items[Random.Range(0, items.Count)];
    }
}