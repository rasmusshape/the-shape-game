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

    [Header("Item Sprites")]
    [SerializeField] Sprite burgerSprite;
    [SerializeField] Sprite beerSprite;
    [SerializeField] Sprite trashSprite;

    void Awake() {
        // Subscribe to relevant events
        energySlider.maxValue = maxPlayerEnergy;
        currentEnergy = maxPlayerEnergy;
    }

    protected GameManager() { }

    private void Start()
    {
        spawnManager = SpawnManager.Instance;
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