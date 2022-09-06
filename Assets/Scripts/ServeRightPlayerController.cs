using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServeRightPlayerController : MonoBehaviour
{
    Rigidbody2D body;
    float dirX;
    float dirY;
    [SerializeField] float updateSpeed = 0.75f;
    [SerializeField] float verticalTiltOffset = 0.5f;
    [SerializeField] bool useAbsoluteMovement;

    // Absolute
    [Header("Absolute Movement (Back and Forth)")]
    [SerializeField] float absDeadzoneBuffer = 1f;
    [SerializeField] float moveSpeed = 20f;

    // Relative
    [Header("Relative Movement (Whip)")]
    [SerializeField] float relDeadzoneBuffer = 0f;
    [SerializeField] float movementSensX = 20;
    [SerializeField] float movementSensY = 40;
    [SerializeField] float jitterBuffer = 1f;

    [SerializeField] bool useReturnSpeedMultiplier = true;
    [SerializeField] float returnSpeedMultiplierX = 2;
    [SerializeField] float returnSpeedMultiplierY = 2;
    [SerializeField] float returnToCenterBuffer = 0.1f;



    private void Start() 
    {
        body = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Limit movement to border of x + y here
        transform.position = new Vector2(
            Mathf.Clamp (transform.position.x, -2, 2), 
            Mathf.Clamp(transform.position.y, -4.5f, 4.5f) 
        );
    }

    private void FixedUpdate() 
    {
        if (useAbsoluteMovement) {
            UpdateWithAbsoluteMovement();
        } else {
            UpdateWithRelativeMovement();
        }
    }

    protected void OnGUI()
    {
        GUI.skin.label.fontSize = Screen.width / 20;

        GUILayout.Label("Orientation: " + Screen.orientation);
        GUILayout.Label("iphone width: " + Screen.width + " : " + GUI.skin.label.fontSize);
        GUILayout.Label("Input X: " + Input.acceleration.x);
        GUILayout.Label("Input Y: " + Input.acceleration.y);
        GUILayout.Label("Target position X: " + dirX);
        GUILayout.Label("Target position Y: " + dirY);
        GUILayout.Label("Transform X: " + transform.localPosition.x);
        GUILayout.Label("Transform Y: " + transform.localPosition.y);
        GUILayout.Label("Absolute difference: " + (transform.localPosition.x - dirX));
    }

    void UpdateWithAbsoluteMovement() 
    {
        dirX = Input.acceleration.x * moveSpeed;
        dirY = (Input.acceleration.y + verticalTiltOffset) * moveSpeed;

        if (dirY > -absDeadzoneBuffer && dirY < absDeadzoneBuffer) dirY = 0;
        if (dirX > -absDeadzoneBuffer && dirX < absDeadzoneBuffer) dirX = 0;

        body.velocity = new Vector2(dirX, dirY);
    }

    void UpdateWithRelativeMovement()
    {
        // Cache phone accelerometer input
        var inputX = Input.acceleration.x;
        var inputY = Input.acceleration.y + verticalTiltOffset;

        // Variables for adjustable movement sensisivity
        dirX = inputX * movementSensX;
        dirY = inputY * movementSensY;

        // Adjustable accelerometer deadzone 
        if (inputX > -relDeadzoneBuffer && inputX < relDeadzoneBuffer) dirX = 0;
        if (inputY > -relDeadzoneBuffer && inputY < relDeadzoneBuffer) dirY = 0;
        
        // Add multiplier for faster move speed back to center position.
        if (useReturnSpeedMultiplier) {

            if ((transform.localPosition.x < 0 && inputX > -returnToCenterBuffer) 
            || (transform.localPosition.x > 0 && inputX < returnToCenterBuffer)) 
                    dirX = inputX * movementSensX * returnSpeedMultiplierX;

            if ((transform.localPosition.y < 0 && inputY > -returnToCenterBuffer) 
            || (transform.localPosition.y > 0 && inputY < returnToCenterBuffer)) 
                    dirY = inputY * movementSensY * returnSpeedMultiplierY;
        }
        
        // Move player object to target position using linear interpolation.
        var targetPosition = new Vector2(
            dirX,
            dirY + verticalTiltOffset
        );
        

        if ((Mathf.Abs(targetPosition.x - transform.localPosition.x) > jitterBuffer)  
        || (Mathf.Abs(targetPosition.y - transform.localPosition.y) > jitterBuffer)  ) {
            transform.localPosition = Vector2.Lerp(transform.localPosition, targetPosition, Time.deltaTime * updateSpeed);
        }
    }

}
