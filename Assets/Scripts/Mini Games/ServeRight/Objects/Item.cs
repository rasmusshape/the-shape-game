using System;
using UnityEngine;

[Serializable]
public class Item {
    public ItemType ItemType;

    public Item(ItemType type) {
        this.ItemType = type;
    }

}

[Serializable]
public enum ItemType {
    Burger,
    Beer,
    Trash
}