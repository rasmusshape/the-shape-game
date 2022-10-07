using System;
using UnityEngine.UI;
using UnityEngine;
using LootLocker.Requests;
using TMPro;


public class LeaderboardController : MonoBehaviour
{
    public TMP_InputField MemberID;
    public int ID;
    private int maxScores = 9;
    public GameObject leaderboardUI;
    public GameObject scoreLinePrefab;
    public GameObject submitButton;

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
            }
        });
    }

    public void SubmitScore()
    {
        MenuSFXManager.Instance.PlayButtonClickSFX(true);
        if (MemberID.text.Length == 0) return;

        LootLockerSDKManager.SubmitScore(
            ValidateInput(MemberID.text), gameManager.playerScore, ID, (response) =>
        { });
        
        ClearScores();
        ShowScores();

        submitButton.SetActive(false);
    }

    private string ValidateInput(string input)
    {
        var diff = 6 - input.Length;

        if (diff > 0)
        {
            for (int i = diff; i > 0; i--)
            {
                input += " ";
            }
        }

        return input;
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
                GameObject scoreLine = Instantiate(scoreLinePrefab);
                TextMeshProUGUI[] textFields = scoreLine.GetComponentsInChildren<TextMeshProUGUI>();
                textFields[0].text = "Connect to the internet for leaderboards";
                Destroy(textFields[1].gameObject);
                        
                scoreLine.transform.SetParent(leaderboardUI.transform);
                
                if (submitButton != null) submitButton.SetActive(false);
                
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
