using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Collections;

public class DifficultyManager: Singleton<DifficultyManager>
{
    #region Events

    public event Action<int> OnDifficultyLvlChange;

    IEnumerator IncreaseDifficulty()
    {
        while(true) 
         { 
             difficultyLvl += difficultyInterval;
             OnDifficultyLvlChange(difficultyLvl);
             yield return new WaitForSeconds(timerInterval);
         }
    }

    #endregion

    #region Public Variables

    [Header("Difficulty lvl")]
    [ReadOnly]
    public int difficultyLvl = 1; 

    [Header("Time needed to change difficulty level")]
    public float timerInterval = 0.3f; 

    [Header("Increment of difficulty level")]
    public int difficultyInterval = 1; 

    #endregion

    #region  Unity Monobehaviour 

    protected DifficultyManager() { }

    private void Start()
    {   
        StartCoroutine(IncreaseDifficulty());
    }

    #endregion
}