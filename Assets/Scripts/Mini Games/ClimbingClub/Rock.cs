using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {

        if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began))
        {
            /*
            Vector3 pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);

            if (hit != null && hit.collider != null) {
                Debug.Log ("I'm hitting "+ hit.collider.name);
                Destroy(gameObject);
            }
            */

            
            var wp = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
             var touchPosition = new Vector2(wp.x, wp.y);
 
             if (this.GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPosition)){
                 Debug.Log("HIT!");
                 Destroy(gameObject);
             }
             
            
        }

    }

    public void Interact() {
        Debug.Log("Interacted with rock!");
        transform.localScale.Set(2, 2, 2);
    }

}
