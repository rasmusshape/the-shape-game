using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MainMenu : MonoBehaviour {

    [SerializeField] GameObject leaderboardCanvas;
    [SerializeField] GameObject mainMenuCanvas;

    private SceneTransition sceneTransition;

    public event Action<bool> OnMenuButtonClick;

    private void Start()
    {
        sceneTransition = FindObjectOfType<SceneTransition>();
    }

    public void NewGame() {
        OnMenuButtonClick.Invoke(true);
        sceneTransition.LoadScene(SceneTransition.SceneIndexType.ServeRight);
    }

    public void ToggleLeaderboard(bool state) {
        OnMenuButtonClick.Invoke(true);
        leaderboardCanvas.SetActive(state);
        mainMenuCanvas.SetActive(!state);
    }

}