using System.Collections.Generic;
using UnityEngine;

public class Order {
    List<Item> orderItems = new List<Item>();

    public void AddItemToOrder(Item item) {
        orderItems.Add(item);
    }

    public void RemoveItemFromOrder(Item item) {
        Item itemFound = orderItems.Find(orderItem => orderItem.ItemType == item.ItemType);

        if (itemFound == null) return;

        orderItems.Remove(itemFound);
    }

}