using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class GameManager : Singleton<GameManager> {

    #region Public GameObjects

    [SerializeField] TextMeshProUGUI scoreText;
    [Header("Energy")] [SerializeField] Slider energySlider;

    #endregion

    #region Public Variables

    [SerializeField] int playerScore;
    [SerializeField] int currentEnergy;
    [SerializeField] int maxPlayerEnergy;
    [SerializeField] int energyPointsGained = 1;
    [SerializeField] int energyPointsReduced = -3;

    #endregion

    #region Private variables

    private ShapersSpawner shapersSpawner;
    private OrderManager orderManager;

    #endregion

    #region Events

    private event Action<bool> OnGameOver;

    void UpdatePlayerEnergy(int points)
    {
        //TODO: Reduce energy based on order's points

        if (points > 0 && currentEnergy == maxPlayerEnergy) return;

        currentEnergy += points;

        energySlider.value = currentEnergy;   
        

        //TODO: Check for game over and trigger event OnGameOver(true)
        if (currentEnergy == 0) OnGameOver(true);
    }

    #endregion

    #region Event Listeners

    #region Order Delivered Event

    public void OnOrderDelivered(int points)
    {
        AddToScore(points);
        UpdatePlayerEnergy(energyPointsGained);
    }

    // Called when score is changed.
    void AddToScore(int amount)
    {
        scoreText.text = (playerScore += amount).ToString();
    }

    #endregion

    #region Order Expired Event

    public void OnOrderExpired(int points)
    {
        UpdatePlayerEnergy(energyPointsReduced);
    }

    #endregion

    #endregion

    #region  Unity Monobehaviour 

    protected GameManager() { }

    void Awake() {
        energySlider.maxValue = maxPlayerEnergy;
        currentEnergy = maxPlayerEnergy;
        playerScore = 0;
        scoreText.text = playerScore.ToString();
    }

    private void Start()
    {
        shapersSpawner = ShapersSpawner.Instance;
        orderManager = OrderManager.Instance;

        shapersSpawner.OnOrderDelivered += OnOrderDelivered;
        orderManager.OnOrderExpired += OnOrderExpired;
        
        AudioManager.Instance.ChangeMusic(AudioManager.MusicType.Hectic);
    }

    void OnApplicationQuit()
    {
        shapersSpawner.OnOrderDelivered -= OnOrderDelivered;
        orderManager.OnOrderExpired -= OnOrderExpired;
    }

    #endregion

}