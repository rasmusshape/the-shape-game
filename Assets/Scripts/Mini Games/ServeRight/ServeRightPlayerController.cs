using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class ServeRightPlayerController : Singleton<ServeRightPlayerController>
{
    Rigidbody2D body;
    float dirX;
    float dirY;
    [SerializeField] float updateSpeed = 0.75f;
    [SerializeField] float verticalTiltOffset = 0.5f;
    [SerializeField] MovementType movementType = MovementType.Absolute;

    // Event
    public event Action<bool> OnPlayerMove;

    // Snapping
    [Header("Snapping Movement Settings")]
    [SerializeField] float xMovementDeadzone = 0.25f;
    [SerializeField] float yMovementDeadzone = 0.25f;
    [SerializeField] float booths_xOffset = 1.5f;
    [SerializeField] float trashcans_xOffset = 1f;
    [SerializeField] float aisles_xOffset = 1.75f;
    [SerializeField] float yAnchorsDistance = 1f;
    [SerializeField] float changePositionCooldownMS = 100;
    [SerializeField] bool canChangePositions = true;

    // Movement anchors
    Dictionary<Vector2, xCoordType> centerCoordsDict;
    List<Vector2> centerCoordsList;
    Vector2 topCenterCoord;
    Vector2 aisleOne_CenterCoord;
    Vector2 trashCanTop_CenterCoord;
    Vector2 aisleTwo_CenterCoord;
    Vector2 aisleThree_CenterCoord;
    Vector2 trashCanBot_CenterCoord;
    Vector2 aisleFour_CenterCoord;
    Vector2 botCenterCoord;
    Vector2 defaultCoords = new Vector2(20,20);

    // Absolute
    [Header("Absolute Movement Settings")]
    [SerializeField] float absDeadzoneBuffer = 1f;
    [SerializeField] float moveSpeed = 20f;
    [SerializeField] float maxMoveSpeed = 40f;

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

    const string trashCanTag = "TrashCan";
    const string beerBoothTag = "BeerBooth";
    const string burgerBoothTag = "BurgerBooth";
    const string trashDumpTag = "TrashDump";
    const string shaperTag = "Shaper";

    public event Action<bool> OnBeerBoothHit;
    public event Action<bool> OnBeerBoothExit;
    public event Action<bool> OnBurgerBoothHit;
    public event Action<bool> OnBurgerBoothExit;
    public event Action<int> OnShaperHit;

    protected ServeRightPlayerController() { }
    private bool playerCanMove = true;
     
    private void Start() 
    {
        body = GetComponent<Rigidbody2D>();

        topCenterCoord = new Vector2(0, yAnchorsDistance * 3);
        aisleOne_CenterCoord = new Vector2(0, yAnchorsDistance * 2.2f);
        trashCanTop_CenterCoord = new Vector2(0, yAnchorsDistance * 1.5f);
        aisleTwo_CenterCoord = new Vector2(0, yAnchorsDistance * 0.8f);

        aisleThree_CenterCoord = new Vector2(0, yAnchorsDistance * -0.8f);
        trashCanBot_CenterCoord = new Vector2(0, yAnchorsDistance * -1.5f);
        aisleFour_CenterCoord = new Vector2(0, yAnchorsDistance * -2.2f);
        botCenterCoord = new Vector2(0, yAnchorsDistance * -3);

        centerCoordsDict = new Dictionary<Vector2, xCoordType> {
            { topCenterCoord, xCoordType.Booth },
            { aisleOne_CenterCoord, xCoordType.Aisle },
            { trashCanTop_CenterCoord, xCoordType.Trashcan },
            { aisleTwo_CenterCoord, xCoordType.Aisle },
            { aisleThree_CenterCoord, xCoordType.Aisle },
            { trashCanBot_CenterCoord, xCoordType.Trashcan },
            { aisleFour_CenterCoord, xCoordType.Aisle },
            { botCenterCoord, xCoordType.Booth }
        };

        centerCoordsList = new List<Vector2> {
            topCenterCoord,
            aisleOne_CenterCoord,
            trashCanTop_CenterCoord,
            aisleTwo_CenterCoord,
            aisleThree_CenterCoord,
            trashCanBot_CenterCoord,
            aisleFour_CenterCoord,
            botCenterCoord
        };

        transform.localPosition = topCenterCoord;

        GameManager.Instance.OnGameOver += OnGameOver;
    }

    private void Update()
    {
        // Limit movement to border of x + y here
        transform.position = new Vector2(
            Mathf.Clamp (transform.position.x, -2, 2), 
            Mathf.Clamp(transform.position.y, -3.75f, 3.5f) 
        );
    }

    private void FixedUpdate() 
    {
        if (playerCanMove)
        {
            switch (movementType) 
            {
                case MovementType.Relative:
                    UpdateWithRelativeMovement();
                    break;
                case MovementType.Absolute:
                    UpdateWithAbsoluteMovement();
                    break;
                default:
                    UpdateWithSnappingMovement();
                    break;
            }
            
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
        GUILayout.Label("Transform X: " + transform.localPosition.x);
        GUILayout.Label("Transform Y: " + transform.localPosition.y);
    }
    */

    void OnTriggerEnter2D(Collider2D other) {
        switch (other.gameObject.tag) {
            case trashCanTag:
                Debug.Log("TrashCan HIT");
                break;
            case trashDumpTag:
                Debug.Log("TrashDump HIT");
                break;
            case beerBoothTag:
                OnBeerBoothHit(true);
                Debug.Log("BeerBooth HIT");
                break;
            case burgerBoothTag:
                OnBurgerBoothHit(true);
                Debug.Log("BurgerBooth HIT");
                break;
            case shaperTag:
                OnShaperHit(other.gameObject.GetComponent<Shaper>().id);
                Debug.Log("Shaper HIT");
                break;
        }
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        switch (other.gameObject.tag) {
            case beerBoothTag:
                OnBeerBoothExit(true);
                Debug.Log("BeerBooth Exit");
                break;
            case burgerBoothTag:
                OnBurgerBoothExit(true);
                Debug.Log("BurgerBooth Exit");
                break;
        }
    }


    void UpdateWithSnappingMovement()
    {
        // Cache phone accelerometer input & position
        var inputX = Input.acceleration.x;
        var inputY = Input.acceleration.y + verticalTiltOffset;

        Vector2 currentPosition = transform.localPosition;

        // Update target positions based on tilt of phone.
        Vector2 targetPosition = defaultCoords;
    
        // Moving up.
        if (inputY > yMovementDeadzone) {
            //Debug.Log("Attempting to move up");
            // if x != 0 we're in a side position and cannot move up -> do nothing.
            if (currentPosition.x != 0) return;

            // If we're at the top, we cannot move further up -> do nothing.
            if (currentPosition.y >= topCenterCoord.y) return;

            var positionIndex = centerCoordsList.FindIndex(coord => Mathf.Approximately(coord.y, currentPosition.y));
            Debug.Log("Position Index: " + positionIndex);

            targetPosition = new Vector2(currentPosition.x, centerCoordsList[positionIndex - 1].y);

            UpdatePosition(targetPosition);
        }

        // mMving down
        if (inputY < -yMovementDeadzone) {

            // if x != 0 we're in a side position and cannot move up -> do nothing.
            if (currentPosition.x != 0) return;

            
            // If we're at the bottom, we cannot move further down -> do nothing.
            if (currentPosition.y <= botCenterCoord.y) return;
            
            
            var positionIndex = centerCoordsList.FindIndex(coord => Mathf.Approximately(coord.y, currentPosition.y));
            Debug.Log("Current Y: " + currentPosition.y);
            Debug.Log("BotCoord Y: " + botCenterCoord.y);
            Debug.Log("Position Index: " + positionIndex);

            targetPosition = new Vector2(currentPosition.x, centerCoordsList[positionIndex + 1].y);
            

            UpdatePosition(targetPosition);
        }

        // Moving right ->
        if (inputX > xMovementDeadzone) {

            // If we're in a position to the right -> do nothing
            if (currentPosition.x > 0) return;

            
            Vector2 dictLookup = defaultCoords;

            foreach (Vector2 coord in centerCoordsDict.Keys) {
                if (coord.Equals(currentPosition)) {
                    dictLookup = coord;
                }
            }

            // If true, means we're not in the middle -> We need to move to middle
            // If we found a coord, means we're in the middle. Use offsets to move to the right
            if (dictLookup.Equals(defaultCoords)) {
                targetPosition = new Vector2(0, currentPosition.y);
            } else {
                xCoordType currentRowType = centerCoordsDict[dictLookup];

                switch (currentRowType) 
                {
                    case xCoordType.Booth:
                        targetPosition = new Vector2(currentPosition.x + booths_xOffset, currentPosition.y);
                        break;
                    case xCoordType.Trashcan:
                        targetPosition = new Vector2(currentPosition.x + trashcans_xOffset, currentPosition.y);
                        break;
                    default:
                        targetPosition = new Vector2(currentPosition.x + aisles_xOffset, currentPosition.y);
                        break;
                }
            }

            UpdatePosition(targetPosition);
                
        } 

        // Moving left <-
        if (inputX < -xMovementDeadzone) {
            // If we're in a position to the left -> do nothing
            if (currentPosition.x < 0) return;

            // If were at the bottom, we cant move left.
            if (currentPosition.Equals(botCenterCoord)) return;

            Vector2 dictLookup = defaultCoords;

            foreach (Vector2 coord in centerCoordsDict.Keys) {
                if (coord.Equals(currentPosition)) {
                    dictLookup = coord;
                }
            }

            // If true, means we're not in the middle -> We need to move to middle
            // If we found a coord, means we're in the middle. Use offsets to move to the left
            if (dictLookup.Equals(defaultCoords)) {
                targetPosition = new Vector2(0, currentPosition.y);
            } else {
                xCoordType currentRowType = centerCoordsDict[dictLookup];

                switch (currentRowType) 
                {
                    case xCoordType.Booth:
                        targetPosition = new Vector2(currentPosition.x - booths_xOffset, currentPosition.y);
                        break;
                    case xCoordType.Trashcan:
                        targetPosition = new Vector2(currentPosition.x - trashcans_xOffset, currentPosition.y);
                        break;
                    default:
                        targetPosition = new Vector2(currentPosition.x - aisles_xOffset, currentPosition.y);
                        break;
                }
            }

            UpdatePosition(targetPosition);
        } 
        
    }

    IEnumerator StartPositionChangeCooldown() {
        canChangePositions = false;
        yield return new WaitForSecondsRealtime(changePositionCooldownMS / 1000);
        canChangePositions = true;
    }

    void UpdatePosition(Vector2 targetPosition) {
        if (!transform.localPosition.Equals(targetPosition) && canChangePositions) {
            transform.localPosition = targetPosition;
            StartCoroutine(StartPositionChangeCooldown());
        }
    }

    void UpdateWithAbsoluteMovement() 
    {
        dirX = Math.Clamp(Input.acceleration.x * moveSpeed, -maxMoveSpeed, maxMoveSpeed);
        dirY = Math.Clamp((Input.acceleration.y + verticalTiltOffset) * moveSpeed, -maxMoveSpeed, maxMoveSpeed);

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

    public void OnGameOver(bool flag)
    {
        playerCanMove = false;
    }

    private enum xCoordType {
        Booth,
        Aisle,
        Trashcan
    }
}
