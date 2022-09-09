using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UI_Pickups : MonoBehaviour {

    BeerSpawner beerSpawner;
    BurgerSpawner burgerSpawner;

    [SerializeField] GameObject beerImages;
    [SerializeField] GameObject beerPrefab;
    [SerializeField] GameObject burgerImages;
    [SerializeField] GameObject burgerPrefab;

    private void Start() {
        beerSpawner = BeerSpawner.Instance;
        beerSpawner.OnBeerSpawned += AddBeerToUI;
        beerSpawner.OnBeerPickedUp += RemoveBeerFromUI;

        burgerSpawner = BurgerSpawner.Instance;
        burgerSpawner.OnBurgerSpawned += AddBurgerToUI;
        burgerSpawner.OnBurgerPickedUp += RemoveBurgerFromUI;
    }

    public void AddBeerToUI(bool wasAdded) {
        Instantiate(beerPrefab).transform.SetParent(beerImages.transform);
    }

    public void RemoveBeerFromUI(bool wasRemoved) {
        int childCount = beerImages.transform.childCount;

        if (childCount > 0) {
            Destroy(beerImages.transform.GetChild(childCount-1).gameObject);
        }
    }

    public void AddBurgerToUI(bool wasAdded) {
        Instantiate(burgerPrefab).transform.SetParent(burgerImages.transform);
    }

    public void RemoveBurgerFromUI(bool wasRemoved) {
        int childCount = burgerImages.transform.childCount;

        if (childCount > 0) {
            Destroy(burgerImages.transform.GetChild(childCount-1).gameObject);
        }
    }
}