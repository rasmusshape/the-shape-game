using UnityEngine;

public class Item {
    public ItemType ItemType;

    public Item(ItemType type) {
        this.ItemType = type;
    }

}

public enum ItemType {
    Burger,
    Beer,
    Trash
}