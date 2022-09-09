using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class GameManager : Singleton<GameManager> {

    private SpawnerManager spawnManager;
    private BeerSpawner beerSpawner;
    private BurgerSpawner burgerSpawner;
    public event Action<bool> OnGameOver;
    
    [SerializeField] TextMeshProUGUI scoreText;
    int playerScore;

    [Header("Energy")]
    [SerializeField] Slider energySlider;
    [SerializeField] int currentEnergy;
    [SerializeField] int maxPlayerEnergy;

    [Header("Inventory")]
    [SerializeField] PlayerInventory inventory;

    [Header("Item Sprites")]
    [SerializeField] Sprite burgerSprite;
    [SerializeField] Sprite beerSprite;
    [SerializeField] Sprite trashSprite;

    void Awake() {
        // Subscribe to relevant events
        energySlider.maxValue = maxPlayerEnergy;
        currentEnergy = maxPlayerEnergy;
        inventory = new PlayerInventory(3);
    }

    protected GameManager() { }

    private void Start()
    {
        spawnManager = SpawnerManager.Instance;
        burgerSpawner = BurgerSpawner.Instance;
        beerSpawner = BeerSpawner.Instance;

        spawnManager.OnOrderDelivered += OnOrderDelivered;
        burgerSpawner.OnBurgerPickedUp += OnBurgerPickedUp;
        beerSpawner.OnBeerPickedUp += OnBeerPickedUp;
    }

    void OnApplicationQuit()
    {
        spawnManager.OnOrderDelivered -= OnOrderDelivered;
        burgerSpawner.OnBurgerPickedUp -= OnBurgerPickedUp;
        beerSpawner.OnBeerPickedUp -= OnBeerPickedUp;
    }

    public void OnOrderDelivered(Order orderDelivered)
    {
        Debug.Log(orderDelivered);
    }

    public void OnBurgerPickedUp(bool difff)
    {
        Debug.Log(difff);
    }

    public void OnBeerPickedUp(bool difff)
    {
        Debug.Log(difff);
    }

    // Called when score is changed.
    void AddToScore(int amount) {
        scoreText.text = (playerScore += amount).ToString();
    }

    void UpdatePlayerEnergy() {
        if (currentEnergy == maxPlayerEnergy) {
            energySlider.value = maxPlayerEnergy + 1;
        } else {
            energySlider.value = currentEnergy;
        }
    }

    public void DropAll() {
        Debug.Log("Dropped everything!");
        inventory.ClearInventory();
        // Update UI
    }

    public Sprite GetSprite(ItemType type) {
        switch (type) {
            case ItemType.Burger:
                return burgerSprite;
            case ItemType.Beer:
                return burgerSprite;
            case ItemType.Trash:
                return burgerSprite;
        }
        return null;
    }

}