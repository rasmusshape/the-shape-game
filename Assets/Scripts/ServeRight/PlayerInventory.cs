using System.Collections;
using System.Collections.Generic;

public class PlayerInventory {
    
    private int maxInventoryCount;
    private List<Item> items;

    public PlayerInventory(int maxInventoryCount) {
        this.maxInventoryCount = maxInventoryCount;
    }

    // Add item to list if max inventory has not been hit.
    public void AddItem(Item item) {
        if (items.Count < maxInventoryCount) {
            items.Add(item);
        }
    }

    // Finds the first item in inventory of given type and removes it.
    public void RemoveItem(Item item) {
        var itemFound = items.Find(inventoryItem => inventoryItem.ItemType == item.ItemType);

        if (itemFound == null) return;

        items.Remove(itemFound);
    }

    // Not needed?
    public Item GetItem(Item item) {
        return items.Find(item => item.Equals(item));
    }

    public int CurrentInventoryCount { get; set; }


}