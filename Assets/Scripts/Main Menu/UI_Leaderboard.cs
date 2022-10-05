using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Leaderboard : MonoBehaviour
{
    [SerializeField] private Animator logoAnimator;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SendToCornerOnDelay());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SendToCornerOnDelay()
    {
        yield return new WaitForSeconds(1);
        logoAnimator.Play("ZoomToCorner");
    }
}
