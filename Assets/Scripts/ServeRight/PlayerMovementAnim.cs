using UnityEngine;
using System;
using System.Collections.Generic;

public class PlayerMovementAnim : MonoBehaviour {
    [SerializeField] List<Sprite> movementSprites;
    [SerializeField] SpriteRenderer currentSprite;
    int currentIndex = 0;

    private void Start() {
        ServeRightPlayerController.Instance.OnPlayerMove += ChangeAnim;
        SetSprite();
    }

    public void ChangeAnim(bool sth) {
        if (currentIndex > movementSprites.Count) {
            currentIndex = 0;
        } else {
            currentIndex ++;
        }

        SetSprite();
    }
    
    private void SetSprite() {
        this.currentSprite.sprite = movementSprites[currentIndex];
    }
}