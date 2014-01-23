using UnityEngine;
using System.Collections;

public enum OldMovementPhase
{
    Jump,
    Slip,
    Shoot
}

public class OldPlayerControllerScript : MonoBehaviour
{
    // Public unity setter
    public bool permanentMovement = true, debugCollisionDetection = false;
    public float runPower = 10, jumpPower = 10, maxSlipTime = 1, slipSmoothTime = 0.3f, gravity = 9.81f;

    CharacterController characterController;
    Vector3 movement = Vector3.zero;
    Vector3 startPosition;
    GameObject mainCamera;
    CameraScipt cameraScript;

    bool sliped = false;
    float slipStartTime;

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
        // Permanent movement
        if (permanentMovement)
            movement.x = runPower;

        // Move player by moving character controller
        characterController.Move(movement * Time.deltaTime);

        if (debugCollisionDetection)
            Debug.Log(characterController.velocity);

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

        #region Slip
        if (sliped && Time.time > slipStartTime + maxSlipTime)
        {
            transform.localScale = new Vector3(1, 1, 1);
            //transform.position += new Vector3(0, 0.5f, 0);
            characterController.height *= 2;
            sliped = false;
        }
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
        //Debug.Log("stay collider");
    }

    // Set Player object and camera object to start position
    void SetToStartposition()
    {
        transform.position = startPosition;
        cameraScript.CameraToStartposition();
    }

    // Methode called by InputLogigScript to set movemten in dependence to input
    public void SetMovementPhase(OldMovementPhase movementPhase)
    {
        switch (movementPhase)
        {
            case OldMovementPhase.Jump:
                if (characterController.isGrounded)
                    movement.y = jumpPower;
                break;
            case OldMovementPhase.Slip:
                if (!sliped)
                {
                    // Fast down to ground
                    if (!characterController.isGrounded)
                        movement.y -= jumpPower;
                    // Slip
                    transform.localScale = new Vector3(1, 0.5f, 1);
                    characterController.height *= 0.5f;
                    slipStartTime = Time.time;
                    sliped = true;
                }
                break;
            case OldMovementPhase.Shoot:
                break;
        }
    }
}
