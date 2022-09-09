using UnityEngine;

public class Item {
    private ItemType itemType;

    public Item(ItemType type) {
        this.itemType = type;
    }

    public ItemType ItemType { get; set; }
}

public enum ItemType {
    Burger,
    Beer,
    Trash
}