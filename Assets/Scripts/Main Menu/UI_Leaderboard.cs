using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Leaderboard : MonoBehaviour
{
    [SerializeField] private Animator logoAnimator;

    private SceneTransition sceneTransition;
    private MenuSFXManager sfxManager;
    
    private void Start()
    {
        sceneTransition = FindObjectOfType<SceneTransition>();
        sfxManager = MenuSFXManager.Instance;
    }

    public void GotoMainMenu() {
        sfxManager.PlayButtonClickSFX(true);
        sceneTransition.LoadScene(SceneTransition.SceneIndexType.MainMenu);
    }
    
    IEnumerator SendToCornerOnDelay()
    {
        yield return new WaitForSeconds(1);
        logoAnimator.Play("ZoomToCorner");
    }
}
