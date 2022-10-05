using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    private Animator crossfadeAnimator;
    private float animationSpeed = 1.5f;
    private Dictionary<SceneIndexType, int> indexToType;

    public event Action<bool> OnSceneChange;

    private void Start()
    {
        crossfadeAnimator = GetComponentInChildren<Animator>();
        indexToType = new Dictionary<SceneIndexType, int>
        {
            {SceneIndexType.MainMenu, 0},
            {SceneIndexType.GameOver, 1},
            {SceneIndexType.ServeRight, 2},
        };
    }

    public void LoadScene(SceneIndexType scene)
    {
        StartCoroutine(AnimateSceneTransition(indexToType[scene]));
    }

    IEnumerator AnimateSceneTransition(int buildIndex)
    {
        crossfadeAnimator.SetTrigger("Start");

        if (OnSceneChange != null) OnSceneChange.Invoke(true);

        yield return new WaitForSeconds(animationSpeed);

        SceneManager.LoadScene(buildIndex);
    }

    public enum SceneIndexType
    {
        MainMenu,
        GameOver,
        ServeRight,
    }
}
