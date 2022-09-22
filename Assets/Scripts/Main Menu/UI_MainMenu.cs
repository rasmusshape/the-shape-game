using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MainMenu : MonoBehaviour {

    [SerializeField] GameObject leaderboardCanvas;
    [SerializeField] GameObject mainMenuCanvas;
    
    public void NewGame() {
        Debug.Log("new");
        // Hardcoded scene -> ServeRight minigame
        SceneManager.LoadScene(1);
    }

    public void ToggleLeaderboard(bool state) {
        Debug.Log("Hey");
        leaderboardCanvas.SetActive(state);
        mainMenuCanvas.SetActive(!state);
    }

}