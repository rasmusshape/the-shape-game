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
    [SerializeField] MovementType movementType = MovementType.Absolute;

    // Absolute
    [Header("Absolute Movement Settings")]
    [SerializeField] float absDeadzoneBuffer = 1f;
    [SerializeField] float moveSpeed = 20f;

    // Relative
    [Header("Relative Movement Settings")]
    [SerializeField] float relDeadzoneBuffer = 0f;
    [SerializeField] float movementSensX = 20;
    [SerializeField] float movementSensY = 40;
    [SerializeField] float jitterBuffer = 1f;

    [SerializeField] bool useReturnSpeedMultiplier = true;
    [SerializeField] float returnSpeedMultiplierX = 2;
    [SerializeField] float returnSpeedMultiplierY = 2;
    [SerializeField] float returnToCenterOffset = 0.1f;

    // Snapping
    [Header("Snapping Movement Settings")]
    [SerializeField] float xMovementDeadzone = 0.25f;
    [SerializeField] float yMovementDeadzone = 0.25f;
    [SerializeField] float xCoordOffset = 1.5f;
    [SerializeField] float yCoordOffset = 3;
    [SerializeField] float changePositionCooldownMS = 100;
    [SerializeField] bool canChangePositions = true;
    Vector2 topleftCoord;
    Vector2 topMidCoord;
    Vector2 topRightCoord;
    Vector2 leftCoord;
    Vector2 centerCoord;
    Vector2 rightCoord;
    Vector2 bottomLeftCoord;
    Vector2 bottomMidCoord;
    Vector2 bottomRightCoord;

    private void Start() 
    {
        body = GetComponent<Rigidbody2D>();

        topleftCoord = new Vector2(-xCoordOffset, yCoordOffset);
        topMidCoord = new Vector2(0, yCoordOffset);
        topRightCoord = new Vector2(xCoordOffset, yCoordOffset);

        leftCoord = new Vector2(-xCoordOffset, 0);
        centerCoord = new Vector2(0, 0);
        rightCoord = new Vector2(xCoordOffset, 0);

        bottomLeftCoord = new Vector2(-xCoordOffset, -yCoordOffset);
        bottomMidCoord = new Vector2(0, -yCoordOffset);
        bottomRightCoord = new Vector2(xCoordOffset, -yCoordOffset);

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
        switch (movementType) 
        {
            case MovementType.Relative:
                UpdateWithRelativeMovement();
                break;
            case MovementType.Snapping:
                UpdateWithSnappingMovement();
                break;
            default:
                UpdateWithAbsoluteMovement();
                break;
        }
    }

    /*
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
    */

    void UpdateWithAbsoluteMovement() 
    {
        dirX = Input.acceleration.x * moveSpeed;
        dirY = (Input.acceleration.y + verticalTiltOffset) * moveSpeed;

        if (dirY > -absDeadzoneBuffer && dirY < absDeadzoneBuffer) dirY = 0;
        if (dirX > -absDeadzoneBuffer && dirX < absDeadzoneBuffer) dirX = 0;

        body.velocity = new Vector2(dirX, dirY);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Obstacle") {
            body.velocity = new Vector2(0, 0);
        }
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

            if ((transform.localPosition.x < 0 && inputX > -returnToCenterOffset) 
            || (transform.localPosition.x > 0 && inputX < returnToCenterOffset)) 
                    dirX = inputX * movementSensX * returnSpeedMultiplierX;

            if ((transform.localPosition.y < 0 && inputY > -returnToCenterOffset) 
            || (transform.localPosition.y > 0 && inputY < returnToCenterOffset)) 
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

    void UpdateWithSnappingMovement()
    {
        // TODO use tuples for the different approved positions.
        // Possibly, depending on world x.y,  positions should be dynamic, based on screensize.

        // Cache phone accelerometer input & position
        var inputX = Input.acceleration.x;
        var inputY = Input.acceleration.y + verticalTiltOffset;
        dirY = Input.acceleration.y + verticalTiltOffset;
        var localPosition = transform.localPosition;

        // Update target positions based on tilt of phone.
        var targetPosition = new Vector2();
    
        // Top
        if (inputY > yMovementDeadzone) {
            bool canMoveToTopCorners = localPosition.Equals(topMidCoord);
            bool isCurrentlyOnSidePosition = localPosition.Equals(topleftCoord) || localPosition.Equals(topRightCoord);

            if (inputX > xMovementDeadzone && (canMoveToTopCorners || isCurrentlyOnSidePosition)) {
                targetPosition = topRightCoord;
            } else if (inputX < -xMovementDeadzone && canMoveToTopCorners) {
                targetPosition = topleftCoord;
            } else {
                targetPosition = topMidCoord;
            }
        }
        // Center
        if (inputY > -yMovementDeadzone && inputY < yMovementDeadzone ) {
            bool canMoveToCenterSides = localPosition.Equals(centerCoord);
            bool isCurrentlyOnSidePosition = localPosition.Equals(leftCoord) || localPosition.Equals(rightCoord);

            if (inputX > xMovementDeadzone && (canMoveToCenterSides || isCurrentlyOnSidePosition)) {
                targetPosition = rightCoord;
            } else if (inputX < -xMovementDeadzone && (canMoveToCenterSides || isCurrentlyOnSidePosition)) {
                targetPosition = leftCoord;
            } else {
                targetPosition = centerCoord;
            }
        }
        // Bottom
        if (inputY < -yMovementDeadzone) {
            bool canMoveToBottomCorners = localPosition.Equals(bottomMidCoord);
            bool isCurrentlyOnSidePosition = localPosition.Equals(bottomLeftCoord) || localPosition.Equals(bottomRightCoord);

            if (inputX > xMovementDeadzone && (canMoveToBottomCorners || isCurrentlyOnSidePosition)) {
                targetPosition = bottomRightCoord;
            } else if (inputX < -xMovementDeadzone && canMoveToBottomCorners) {
                targetPosition = bottomLeftCoord;
            } else {
                targetPosition = bottomMidCoord;
            }
        }

        if (!transform.localPosition.Equals(targetPosition) && canChangePositions) {
            transform.localPosition = targetPosition;
            StartCoroutine(StartPositionChangeCooldown());
        } 
        
    }

    IEnumerator StartPositionChangeCooldown() {
        canChangePositions = false;
        yield return new WaitForSecondsRealtime(changePositionCooldownMS / 1000);
        canChangePositions = true;
    }
}
