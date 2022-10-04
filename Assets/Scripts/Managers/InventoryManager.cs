using UnityEngine;
using System.Collections.Generic;
using System;

public class InventoryManager : Singleton<InventoryManager>
{
    #region Public GameObjects

    [SerializeField] GameObject inventory_UI;
    [SerializeField] GameObject burgerPrefab;
    [SerializeField] GameObject beerPrefab;

    #endregion

    #region Public Variables

    [Header("maximum items in the inventory")]
    public int maxInventoryCount = 3;

    [Header("current items in the inventory")]
    public List<Item> items = new List<Item>();

    // Add item to list if max inventory has not been hit.
    private bool AddItemToInventory(ItemType type)
    {
        Item item;
        if (type == ItemType.Burger)
            item = new Item(ItemType.Burger);
        else item = new Item(ItemType.Beer);

        if (items.Count < maxInventoryCount)
        {
            items.Add(item);
            return true;
        }
        return false;
    }

    public void ClearInventory()
    {
        items.Clear();
        foreach (Transform item in inventory_UI.transform) GameObject.Destroy(item.gameObject);
    }

    #endregion

    #region Private variables

    private ShapersSpawner shapersSpawner;
    private BeerSpawner beerSpawner;
    private BurgerSpawner burgerSpawner;

    #endregion

    #region Event Listeners

    #region Burger picked-up Event

    private void OnBurgerPickedUp(bool wasAdded)
    {
        if(AddItemToInventory(ItemType.Burger)) 
            AddBurgerToUI();
    }

    private void AddBurgerToUI()
    {
        Instantiate(burgerPrefab).transform.SetParent(inventory_UI.transform);
    }

    #endregion

    #region Beer picked-up Event

    private void OnBeerPickedUp(bool wasAdded)
    {
        if (AddItemToInventory(ItemType.Beer))
            AddBeerToUI();
    }

    private void AddBeerToUI()
    {
        Instantiate(beerPrefab).transform.SetParent(inventory_UI.transform);
    }

    #endregion

    #region Item delivered Event

    public void OnItemDelivered(ItemType itemType)
    {
        RemoveItemFromInventory(itemType);
        RemoveItemFromUI(itemType);
    }

    public void RemoveItemFromUI(ItemType itemType)
    {
        int childCount = inventory_UI.transform.childCount;

        if (childCount > 0)
        {
            foreach (Transform child in inventory_UI.transform)
            {
                //GameObject prefabFound = itemPrefabs.Find(prefab => prefab.CompareTag(item.ItemType.ToString()));
                if (child.CompareTag(itemType.ToString()))
                {
                    Destroy(child.gameObject);
                    return;
                }
            }
            
        }
    }

    // Finds the first item in inventory of given type and removes it.
    public void RemoveItemFromInventory(ItemType itemType)
    {
        Item itemFound = items.Find(inventoryItem => inventoryItem.ItemType == itemType);

        if (itemFound == null) return;

        items.Remove(itemFound);
    }

    #endregion

    #endregion

    #region  Unity Monobehaviour

    protected InventoryManager() { }

    void Start() {
        shapersSpawner = ShapersSpawner.Instance;
        beerSpawner = BeerSpawner.Instance;
        burgerSpawner = BurgerSpawner.Instance;
        
        beerSpawner.OnBeerPickedUp += OnBeerPickedUp;
        burgerSpawner.OnBurgerPickedUp += OnBurgerPickedUp;
        shapersSpawner.OnItemDelivered += OnItemDelivered;
    }

    void OnApplicationQuit()
    {
        beerSpawner.OnBeerPickedUp -= OnBeerPickedUp;
        burgerSpawner.OnBurgerPickedUp -= OnBurgerPickedUp;
        shapersSpawner.OnItemDelivered -= OnItemDelivered;
    }

    #endregion

}