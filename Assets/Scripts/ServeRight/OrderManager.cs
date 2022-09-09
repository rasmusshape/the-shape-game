using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class OrderManager : MonoBehaviour {
    
    int currentMinItemsPrOrder = 1;
    int currentMaxItemsPrOrder = 1;
    int maxItemsPrOrder = 5;

    List<Item> items;

    GameManager gameManager;

    private void Start() {
        gameManager = FindObjectOfType<GameManager>();

        items = new List<Item> {
            new Item(ItemType.Burger),
            new Item(ItemType.Beer)
        };
    }

    public Order GetOrder() {
        int itemsForOrder = Random.Range(currentMinItemsPrOrder, currentMaxItemsPrOrder);

        Order newOrder = new Order();

        for (int i = 0; i < itemsForOrder; i++) {
            newOrder.AddItemToOrder(GetRandomItem());
        }

        return newOrder;
    }

    public Item GetRandomItem() {
        return items[Random.Range(0, items.Count)];
    }
}