using System.Collections;
using TMPro;
using UnityEngine;

public class UI_Leaderboard : MonoBehaviour
{
    [SerializeField] private Animator logoAnimator;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject leaderboardUI;
    [SerializeField] private GameObject inputGroupUI;
    [SerializeField] private GameObject submitButton;
    [SerializeField] TMP_InputField MemberID;

    private SceneTransition sceneTransition;
    private MenuSFXManager sfxManager;
    
    private void Start()
    {
        sceneTransition = FindObjectOfType<SceneTransition>();
        sfxManager = MenuSFXManager.Instance;
        scoreText.text = GameManager.Instance.playerScore.ToString();
    }

    public void SubmitScore()
    {
        MenuSFXManager.Instance.PlayButtonClickSFX(true);
        if (MemberID.text.Length <= 0) return;
        
        FindObjectOfType<LeaderboardController>().SubmitScore();

            logoAnimator.SetTrigger("ZoomToCorner");
            inputGroupUI.SetActive(false);
            leaderboardUI.SetActive(true);
            submitButton.SetActive(false);
        
    }

    public void GotoMainMenu() {
        sfxManager.PlayButtonClickSFX(true);
        sceneTransition.LoadScene(SceneTransition.SceneIndexType.MainMenu);
    }
    
}
