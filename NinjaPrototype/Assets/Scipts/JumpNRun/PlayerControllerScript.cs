using UnityEngine;
using System.Collections;

public enum MovementPhase
{
    Jump,
    Cower,
    Shoot
}

public class PlayerControllerScript : MonoBehaviour
{
    // Public unity setter
    public bool permanentMovement = true, debugCollisionDetection = false;
    public float runPower = 10, jumpPower = 10, maxCowerTime = 1, maxTouchBufferTime = 0.3f, gravity = 9.81f;

    // CharacterController variables
    CharacterController characterController;
    Vector3 movement = Vector3.zero;
    Vector3 startPosition;

    // Camera variables
    GameObject mainCamera;
    CameraScipt cameraScript;

    // Movement variables
    bool jump = false, cower = false, standUp = false;
    float jumpBufferTime, cowerBufferTime, cowerStartTime;

    // Use this for initialization
    void Start()
    {
        // Set startposition
        startPosition = transform.position;

        #region Check for CharacterController and set to variable
        if (GetComponent<CharacterController>())
            characterController = GetComponent<CharacterController>();
        else
            Debug.LogError("PlayerControllerScript needs CharacterController in Playerobject");
        #endregion

        #region Check for CameraObject and set to variable
        if (GameObject.FindGameObjectWithTag("CameraObject"))
        {
            mainCamera = GameObject.FindGameObjectWithTag("CameraObject");
            // Check for CameraScipt and set to variable
            if (mainCamera.GetComponent<CameraScipt>())
                cameraScript = mainCamera.GetComponent<CameraScipt>();
            else
                Debug.LogError("PlayerControllerScript needs CameraScript in CameraObject");
        }
        else
            Debug.LogError("PlayerControllerScript needs Gameobject with \"CameraObject\" tag");
        #endregion

        // Set RunPower to MainCamera
        cameraScript.MovementSpeed = runPower;
    }

    // Update is called once per frame
    void Update()
    {
        #region Character Movement
        // Set all movements
        DoMovementPhase();
        // Move player by moving character controller
        characterController.Move(movement * Time.deltaTime);
        // Correct z position
        characterController.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        #endregion

        if (debugCollisionDetection)
            Debug.Log(characterController.collisionFlags);

        #region Gravity
        // Touch Ground
        if (characterController.isGrounded)
            movement.y = -1;
        // Touch Ceiling
        else if (characterController.collisionFlags == CollisionFlags.Above)
            movement.y = 0;
        // Fall down
        else
            movement.y -= gravity * Time.deltaTime;
        #endregion
    }

    void OnTriggerEnter(Collider colliderObject)
    {
        switch (colliderObject.tag)
        {
            #region Get points, ect. trigger
            case "TriggerGetGold":
                Destroy(colliderObject.gameObject);
                Debug.Log("You get gold!");
                break;
            #endregion
            #region Set to startposition trigger
            case "TriggerOutOfCamera":
                SetToStartposition();
                Debug.Log("You are out of camera und die!");
                break;
            case "TriggerFinish":
                SetToStartposition();
                Debug.Log("You have finished!");
                break;
            case "TriggerFallDown":
                SetToStartposition();
                Debug.Log("You fall down and die!");
                break;
            #endregion
        }
    }
    void OnTriggerStay(Collider colliderObject)
    {

    }

    // Set Player object and camera object to start position
    void SetToStartposition()
    {
        transform.position = startPosition;
        cameraScript.CameraToStartposition();
    }

    void DoMovementPhase()
    {
        // Permanent movement
        if (permanentMovement)
            movement.x = runPower;

        // Jump
        if (jump && Time.time - jumpBufferTime <= maxTouchBufferTime
            && characterController.isGrounded)
        {
            movement.y = jumpPower;
            jump = false;
        }
        // End jump after buffertime ends
        else if (jump && Time.time >= jumpBufferTime + maxTouchBufferTime)
            jump = false;

        // Cower
        if (cower && Time.time - cowerBufferTime <= maxTouchBufferTime)
        {
            // Fast down to ground
            if (!characterController.isGrounded)
                movement.y -= jumpPower;
            // Cower
            characterController.transform.localScale = new Vector3(1, 0.5f, 1);
            // Set new standup time if not allready set for currend standup
            if (!standUp)
                cowerStartTime = Time.time;
            cower = false;
            standUp = true;
        }
        // End cower after buffertime ends
        else if (cower && Time.time >= cowerBufferTime + maxTouchBufferTime)
            cower = false;

        // Stand up
        if (standUp && Time.time > cowerStartTime + maxCowerTime)
        {
            characterController.transform.localScale = new Vector3(1, 1, 1);
            standUp = false;
        }
    }

    // Methode called by InputLogigScript to set movemten in dependence to input
    public void SetMovementPhase(MovementPhase movementPhase)
    {
        switch (movementPhase)
        {
            case MovementPhase.Jump:
                jumpBufferTime = Time.time;
                jump = true;
                break;
            case MovementPhase.Cower:
                cowerBufferTime = Time.time;
                cower = true;
                break;
            case MovementPhase.Shoot:
                break;
        }
    }
}
