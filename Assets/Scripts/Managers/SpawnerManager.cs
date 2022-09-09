using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnerManager: Singleton<SpawnerManager>
{
    #region Public GameObjects

    [Header("The positions of the incative shapers")]
    public List<Shaper> inactiveShapers;

    [Header("The positions of the active shapers")]
    public List<Shaper> activeShapers;

    #endregion

    #region Public Variables

    [Header("Time passed in game")]
    public float timer = 0f; 

    [Header("Time passed to spawn an order")]
    public float timerInterval = 5f;

    [Header("max shapers allowed to order. Max 8")]
    public int maxShapers = 1;

    #endregion

    #region Private Variables

    private DifficultyManager difficultyManager;
    private ServeRightPlayerController playerController;
    private OrderManager orderManager;
    private int diffficulty;

    #endregion

    #region Events

    public event Action<Order> OnOrderDelivered;

    #endregion

    #region Event Listeners

    #region Difficulty Event

    public void OnDifficultyIncreased(int difff)
    {
        diffficulty = difff;
        if(maxShapers < 8) maxShapers ++;
        Debug.Log(difff);
    }

    #endregion

    #region Shaper Hit Event

    public void OnShaperHit(int shaperId)
    {
        Shaper shaperPicked = findPickedShaper(shaperId);
        if(shaperPicked.order != null) {
            inactiveShapers.Remove(shaperPicked);
            activeShapers.Add(shaperPicked);
            OnOrderDelivered(shaperPicked.order);
            shaperPicked.gameObject.SetActive(false);
        }
        Debug.Log(shaperId);
    }

    private Shaper findPickedShaper(int pickedId) {
        Shaper result = null;
        List<Shaper> shapers = new List<Shaper>();
        shapers.AddRange(activeShapers);
        shapers.AddRange(inactiveShapers);

        foreach (Shaper item in shapers)
        {
            if(item.id == pickedId) result = item;
        }
        return result;
    }

    #endregion

    #endregion

    #region  Unity Monobehaviour 

    protected SpawnerManager() { }

    private void Start()
    {
        difficultyManager = DifficultyManager.Instance;
        orderManager = OrderManager.Instance;
        playerController = ServeRightPlayerController.Instance;

        difficultyManager.OnDifficultyLvlChange += OnDifficultyIncreased;
        playerController.OnShaperHit += OnShaperHit;

        StartCoroutine(SpawnOrder());
    }

    IEnumerator SpawnOrder()
    {
        while(true) 
         { 
            Debug.Log("eeeeee");
             //find random inactive Shaperg
             if(inactiveShapers.Count > 0 && activeShapers.Count < maxShapers) {
                Shaper shaperPicked = inactiveShapers[UnityEngine.Random.Range(0, inactiveShapers.Count)].GetComponent<Shaper>();
                shaperPicked.order = orderManager.GetOrder(shaperPicked.gameObject);
                //Add order to shaper and Activate Shaper
                shaperPicked.gameObject.SetActive(true);
                inactiveShapers.Remove(shaperPicked);
                activeShapers.Add(shaperPicked);
             }
             yield return new WaitForSeconds(timerInterval);
         }
    }

    void OnApplicationQuit()
    {
         difficultyManager.OnDifficultyLvlChange -= OnDifficultyIncreased;
         playerController.OnShaperHit -= OnShaperHit;
    }

    private void FixedUpdate()
    {
        timer += Time.deltaTime;
    }

    #endregion
}