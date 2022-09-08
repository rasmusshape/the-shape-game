using UnityEngine;

public class Item {

    private Sprite sprite;
    private ItemType itemType;

    public Item(Sprite sprite, ItemType itemType) {
        this.sprite = sprite;
        this.itemType = itemType;
    }

    public Sprite Sprite { get; set; }
    public ItemType ItemType { get; set; }
}

public enum ItemType {
    Burger,
    Beer,
    Trash
}