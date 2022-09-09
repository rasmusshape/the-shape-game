using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager: Singleton<SpawnerManager>
{
    #region Public GameObjects

    // [Header("The GameObject of the voltmeter")]
    // public GameObject voltmeter;

    #endregion

    #region Public Variables

    [Header("Time passed in game")]
    public float timer = 0f; 

    #endregion

    #region Private Variables

    private DifficultyManager difficultyManager;
    private int diffficulty;

    #endregion

    #region Event Listeners

    #region Difficulty Event

    /// <summary>
    /// Listens when the difficulty of the game gets increased
    /// Increase the diffficulty modifier
    /// </summary>
    public void OnDifficultyIncreased(int difff)
    {
        diffficulty = difff;
        Debug.Log(difff);
    }

    #endregion

    #endregion

    #region  Unity Monobehaviour 

    protected SpawnerManager() { }

    private void Start()
    {
        difficultyManager = DifficultyManager.Instance;
        difficultyManager.OnDifficultyLvlChange += OnDifficultyIncreased;
    }

    void OnApplicationQuit()
    {
         difficultyManager.OnDifficultyLvlChange -= OnDifficultyIncreased;
    }

    private void FixedUpdate()
    {
        timer += Time.deltaTime;
    }

    #endregion
}