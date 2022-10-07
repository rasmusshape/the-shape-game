using System;
using UnityEngine.UI;
using UnityEngine;
using LootLocker.Requests;
using TMPro;


public class LeaderboardController : MonoBehaviour
{
    public TMP_InputField MemberID;
    public int ID;
    public int maxScores = 9;
    public GameObject leaderboardUI;
    public GameObject scoreLinePrefab;
    
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
        
        LootLockerSDKManager.StartSession("ShapeTheGame", (response) =>
        {
            if (response.success)
            {
                ShowScores();
            }
            else
            {
                Debug.Log("Failed to connect to LootLocker: " + response.Error);
                
                GameObject scoreLine = Instantiate(scoreLinePrefab);
                TextMeshProUGUI[] textFields = scoreLine.GetComponentsInChildren<TextMeshProUGUI>();
                textFields[0].text = "Connect to the internet for leaderboards";
                Destroy(textFields[1].gameObject);
                        
                scoreLine.transform.SetParent(leaderboardUI.transform);
            }
        });
    }

    public void SubmitScore()
    {
        LootLockerSDKManager.SubmitScore(
                MemberID.text, gameManager.playerScore, ID, (response) =>
            {
                ClearScores();
                ShowScores();
            });
    }
    

    public void ShowScores()
    {
        LootLockerSDKManager.GetScoreList(ID, maxScores, (response) =>
        {
            if (response.success)
            {
                LootLockerLeaderboardMember[] data = response.items;

                int amountToShow = Mathf.Clamp(data.Length, 0, maxScores);
                
                for (int i = 0; i < amountToShow; i++)
                {
                    GameObject scoreLine = Instantiate(scoreLinePrefab);
                    TextMeshProUGUI[] textFields = scoreLine.GetComponentsInChildren<TextMeshProUGUI>();
                    textFields[0].text = data[i].member_id;
                    textFields[1].text = data[i].score.ToString();
                        
                    scoreLine.transform.SetParent(leaderboardUI.transform);
                }
            }
            else
            {
                Debug.Log("Failed to fetch high-scores: " + response.Error);
            }
        });
    }

    public void ClearScores()
    {
        foreach (Transform child in leaderboardUI.transform)
        {
            Destroy(child.gameObject);
        }

    }
}
