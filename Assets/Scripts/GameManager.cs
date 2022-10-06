using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    public int playerScore;
    void Awake()
    {
        // Persistent singleton
        GameManager[] managers = FindObjectsOfType<GameManager>();
        if (managers.Length > 1) Destroy(managers[1].gameObject);

        DontDestroyOnLoad(gameObject);
    }
    
    public void NewGame()
    {
        playerScore = 0;
    }
}
