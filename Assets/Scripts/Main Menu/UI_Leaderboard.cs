using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Leaderboard : MonoBehaviour
{
    [SerializeField] private Animator logoAnimator;
    [SerializeField] private TextMeshProUGUI scoreText;

    private SceneTransition sceneTransition;
    private MenuSFXManager sfxManager;
    
    private void Start()
    {
        sceneTransition = FindObjectOfType<SceneTransition>();
        sfxManager = MenuSFXManager.Instance;
        scoreText.text = GameManager.Instance.playerScore.ToString();
        
        StartCoroutine(SendToCornerOnDelay());
    }

    public void GotoMainMenu() {
        sfxManager.PlayButtonClickSFX(true);
        sceneTransition.LoadScene(SceneTransition.SceneIndexType.MainMenu);
    }
    
    IEnumerator SendToCornerOnDelay()
    {
        yield return new WaitForSeconds(1.5f);
        logoAnimator.SetTrigger("ZoomToCorner");
    }
}
