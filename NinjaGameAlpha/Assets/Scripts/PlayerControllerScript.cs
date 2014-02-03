using UnityEngine;
using System.Collections;

// Public enums
public enum MovementPhase
{
    Jump,
    Slide,
    Shoot
}
public enum AnimationPhase
{
    Run,
    Jump,
    Idle,
    Slide,
    Fall,
    Rush,
    RushDown,
    Die
}

public class PlayerControllerScript : MonoBehaviour
{
    #region Variables
    // Public unity setter
    public bool permanentMovement = true;
    public float runPower = 10, jumpPower = 10, superJumpPower = 15, rushPower = 5,
                 startRushTime = 0.2f, maxRushTime = 0.5f, maxSlideTime = 1,
                 maxTouchBufferTime = 0.3f,
                 gravity = 30;
    public GameObject characterMesh;
    public Rigidbody missle;
    public Transform missleStartPosition;

    // CharacterController variables
    CharacterController characterController;
    public bool PlayerTopTrigger { private get; set; }
    Vector3 movement = Vector3.zero;
    Vector3 startPosition, lastCheckpointPosition;

    // Camera variables
    GameObject cameraObject;
    CameraScipt cameraScript;
    //bool outOfCamera; TODO

    // Movement variables
    bool jump, slide, standUp, standUpAfterObject, rush, rushDone;
    float jumpBufferTime, slideBufferTime, slideStartTime, rushStartTime;
    #endregion

    // Use this for initialization
    void Start()
    {
        // Set startposition
        startPosition = transform.position;
        lastCheckpointPosition = startPosition;

        #region Check for CharacterController and set to variable
        if (GetComponent<CharacterController>())
            characterController = GetComponent<CharacterController>();
        else
            Debug.LogError("PlayerControllerScript needs CharacterController in Playerobject");
        #endregion

        #region Check for CameraObject and set to variable
        if (GameObject.FindGameObjectWithTag("CameraObject"))
        {
            cameraObject = GameObject.FindGameObjectWithTag("CameraObject");
            // Check for CameraScipt and set to variable
            if (cameraObject.GetComponent<CameraScipt>())
                cameraScript = cameraObject.GetComponent<CameraScipt>();
            else
                Debug.LogError("PlayerControllerScript needs CameraScript in CameraObject");
        }
        else
            Debug.LogError("PlayerControllerScript needs Gameobject with \"CameraObject\" tag");
        #endregion

        // Set RunPower to MainCamera
        cameraScript.MovementSpeed = runPower;

        SetToStartPosition();
    }

    // Update is called once per frame
    void Update()
    {
        #region Gravity
        // Touch Ground or ceiling
        if (characterController.isGrounded ||
            characterController.collisionFlags == CollisionFlags.Above)
            movement.y = -0.1f;
        // Fall down
        else
            movement.y -= gravity * Time.deltaTime;
        #endregion

        #region Character Movement
        // Set all movements
        DoMovementPhase();
        // Move player by moving character controller
        characterController.Move(movement * Time.deltaTime);
        // Correct z position
        if (transform.position.z != 0)
            characterController.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        #endregion
    }

    void OnTriggerEnter(Collider colliderObject)
    {
        switch (colliderObject.tag)
        {
            #region Get coins, ect. trigger
            case "GetCoinTrigger":
                Destroy(colliderObject.gameObject);
                break;
            #endregion
            #region Set to position trigger
            case "CameraEndTrigger":
                SetToLastCheckpointPosition();
                break;
            case "LevelEndTrigger":
                SetToLastCheckpointPosition();
                /* TODO 
                 * LOAD NEXT LEVEL
                 */
                break;
            case "FallDownTrigger":
                SetToLastCheckpointPosition();
                break;

            // TODO
            // Trigger front and top
            //// Stop rush
            //movement.x = runPower - 1;
            //rushDone = false;
            //// Stop jump
            //movement.y = 0;
            //outOfCamera = true;
            #endregion
        }
    }

    #region Set to position methodes
    // Set Player object and camera object to last checkpoint position
    void SetToLastCheckpointPosition()
    {
        ResetAllMovements();

        // Set positions
        transform.position = lastCheckpointPosition;
        cameraScript.CameraToPosition(lastCheckpointPosition);
    }
    // Set Player object and camera object to start position
    void SetToStartPosition()
    {
        ResetAllMovements();

        // Set positions
        transform.position = startPosition;
        cameraScript.CameraToPosition(startPosition);
    }
    #endregion

    void ResetAllMovements()
    {
        // Stand up if sliding
        if (standUp)
            StandUp();

        // Set all movements to false
        jump = false; rush = false; rushDone = false;
        // Set all times to null
        jumpBufferTime = 0; slideBufferTime = 0; slideStartTime = 0; rushStartTime = 0;

        // Set movespeed to default
        if (permanentMovement)
            movement.x = runPower;
        /* TODO
           * BEREINIGEN BEI FERTIGSTELLUNG!!!!
           */
    }

    void DoMovementPhase()
    {
        #region Run
        if (!standUp && characterController.isGrounded)
            // Set animation
            SetAnimationPhase(AnimationPhase.Run);
        #endregion

        // TODO
        // Near camera position
        //if (transform.position.x > cameraObject.transform.position.x)
        //    movement.x -= 0.1f * Time.deltaTime;
        //else if (transform.position.x < cameraObject.transform.position.x)
        //    movement.x += 0.1f * Time.deltaTime;

        #region Slide
        if (slide)
            if (!standUp && Time.time - slideBufferTime <= maxTouchBufferTime)
            {
                // Set new standup time
                slideStartTime = Time.time;

                // Fast down to ground
                if (!characterController.isGrounded)
                {
                    movement.y = -jumpPower;
                    // Set animation
                    SetAnimationPhase(AnimationPhase.RushDown);
                }
                else
                    // Set animation
                    SetAnimationPhase(AnimationPhase.Slide);

                // Set collisionbox
                characterController.height /= 2;
                characterController.center -= new Vector3(0, characterController.center.y / 2, 0);

                // Stop waiting
                slide = false;
                // Start waiting
                standUp = true;
            }
            // End cower after buffertime ends
            else if (Time.time >= slideBufferTime + maxTouchBufferTime)
                // Stop waiting
                slide = false;
        #endregion

        #region Stand up
        if (standUp)
            // Stand up after time
            if (!PlayerTopTrigger && Time.time >= slideStartTime + maxSlideTime)
            {
                // Set collisionbox
                StandUp();
                standUpAfterObject = false;
            }
            // Allow stand up after object 
            else if (!standUpAfterObject && PlayerTopTrigger)
            {
                // Set under object
                standUpAfterObject = true;
            }
            // Stand up after object
            else if (standUpAfterObject && !PlayerTopTrigger)
            {
                // Set collisionbox
                StandUp();
                standUpAfterObject = false;
            }
        // TODO
        //else if(standUp && characterController.isGrounded && 
        //        !characterMesh.animation.IsPlaying("DownJump") &&
        //        !characterMesh.animation.IsPlaying("Slide"))
        //    // Set animation
        //    SetAnimationPhase(AnimationPhase.Slide);
        #endregion

        #region Jump
        if (jump)
            // High jump
            if (standUp && Time.time - jumpBufferTime <= maxTouchBufferTime &&
                characterController.isGrounded && !PlayerTopTrigger)
            {
                // Set collisionbox
                StandUp();

                // Set movement
                movement.y = superJumpPower;

                // Set animation
                SetAnimationPhase(AnimationPhase.Jump);

                // Set new standup time
                rushStartTime = Time.time;

                // Stop waiting
                jump = false;
                // Start waiting
                rush = true;
            }
            // Normal jump
            else if (Time.time - jumpBufferTime <= maxTouchBufferTime
                && characterController.isGrounded && !PlayerTopTrigger)
            {
                // Set movement
                movement.y = jumpPower;

                // Set animation
                SetAnimationPhase(AnimationPhase.Jump);

                // Stop waiting
                jump = false;
            }
            // End jump after buffertime ends
            else if (Time.time >= jumpBufferTime + maxTouchBufferTime)
                // Stop waiting
                jump = false;
        #endregion

        #region Rush
        if (rush && !rushDone && Time.time >= rushStartTime + startRushTime)
        {
            // Set movement
            movement.x += rushPower;

            // Set animation
            SetAnimationPhase(AnimationPhase.Rush);

            // Stop waiting
            rush = false;
            // Start waiting
            rushDone = true;
        }
        else if (rushDone && Time.time >= rushStartTime + startRushTime + maxRushTime)
        {
            // Set movement
            movement.x -= rushPower;

            // Stop waiting
            rushDone = false;
        }
        #endregion
    }
    void StandUp()
    {
        // Set collisionbox
        characterController.center += new Vector3(0, characterController.center.y, 0);
        characterController.height *= 2;

        // Stop waiting
        standUp = false;
    }

    void SetAnimationPhase(AnimationPhase animationPhase)
    {
        switch (animationPhase)
        {
            case AnimationPhase.Run:
                characterMesh.animation.Play("WalkCycle");
                break;
            case AnimationPhase.Jump:
                characterMesh.animation.Play("Jump");
                break;
            case AnimationPhase.Slide:
                characterMesh.animation.Play("Slide");
                break;
            case AnimationPhase.Fall:
                characterMesh.animation.Play("Fall");
                break;
            case AnimationPhase.Rush:
                characterMesh.animation.Play("Rush");
                break;
            case AnimationPhase.RushDown:
                characterMesh.animation.Play("DownJump");
                break;
            case AnimationPhase.Die:
                characterMesh.animation.Play("Die");
                break;
            case AnimationPhase.Idle:
            default:
                characterMesh.animation.Play("Idle");
                break;
        }
    }

    // Methode called by InputLogigScript to set movemten in dependence to input
    public void SetMovementPhase(MovementPhase movementPhase)
    {
        switch (movementPhase)
        {
            case MovementPhase.Jump:
                // Set new buffer time and start waiting to true
                jumpBufferTime = Time.time;
                jump = true;
                break;
            case MovementPhase.Slide:
                // Set new buffer time and start waiting to true
                slideBufferTime = Time.time;
                slide = true;
                break;
            case MovementPhase.Shoot:
                Rigidbody missleInstance;
                missleInstance = Instantiate(missle, missleStartPosition.position, missleStartPosition.rotation) as Rigidbody;
                missleInstance.AddForce(missleStartPosition.right * 2000);
                break;
        }
    }
}
