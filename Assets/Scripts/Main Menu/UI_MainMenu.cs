using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MainMenu : MonoBehaviour {

    [SerializeField] GameObject leaderboardCanvas;
    [SerializeField] GameObject mainMenuCanvas;

    private SceneTransition sceneTransition;
    private MenuSFXManager sfxManager;
    
    private void Start()
    {
        sceneTransition = FindObjectOfType<SceneTransition>();
        sfxManager = MenuSFXManager.Instance;
    }

    public void NewGame() {
        sfxManager.PlayButtonClickSFX(true);
        sfxManager.PlayStartGameSFX(true);
        sceneTransition.LoadScene(SceneTransition.SceneIndexType.ServeRight);
    }

    public void ToggleLeaderboard(bool state) {
        sfxManager.PlayButtonClickSFX(true);
        leaderboardCanvas.SetActive(state);
        mainMenuCanvas.SetActive(!state);
    }

}