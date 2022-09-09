using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour {
    
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