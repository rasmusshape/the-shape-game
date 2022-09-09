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

    [Header("Item Sprites")]
    [SerializeField] Sprite burgerSprite;
    [SerializeField] Sprite beerSprite;
    [SerializeField] Sprite trashSprite;

    void Awake() {
        // Subscribe to relevant events
        energySlider.maxValue = maxPlayerEnergy;
        currentEnergy = maxPlayerEnergy;
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