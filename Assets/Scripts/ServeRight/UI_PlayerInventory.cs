using UnityEngine;
using System.Collections.Generic;

public class UI_PlayerInventory : MonoBehaviour {


    GameManager gameManager;

    [SerializeField] GameObject inventory_UI;
    [SerializeField] GameObject burgerPrefab;
    [SerializeField] GameObject beerPrefab;
    

    private void Start() {
        //gameManager = GameManager.Instance;
        // event add
    }
    
    public void AddBeerToUI(bool wasAdded) {
        Instantiate(beerPrefab).transform.SetParent(inventory_UI.transform);
    }

    public void RemoveItemOfType(ItemType itemType) {
        int childCount = inventory_UI.transform.childCount;

        if (childCount > 0) {
            Item[] activeItemsInUI = transform.GetComponentsInChildren<Item>();

            foreach (Transform child in transform) {
                if (child.GetComponent<Item>().ItemType == itemType) {
                    Destroy(child.gameObject);
                    return;
                }
            }
        }
    }

}