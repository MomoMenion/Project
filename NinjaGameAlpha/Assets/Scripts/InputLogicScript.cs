using UnityEngine;
using System.Collections;

public class InputLogicScript : MonoBehaviour
{
    // Public unity setter
    public bool touchInput = true, keyInput = false;
    public float minSwipDist = 10.0f, maxTipeTime = 0.1f;

    // Touch variables
    Vector2[] touchStartPos = new Vector2[2];
    float[] touchStartTime = new float[2];

    // Player controller variables
    PlayerControllerScript playerControllerScript;

    void Start()
    {
        // Check for PlayerControllerScript and set to variable
        if (GetComponent<PlayerControllerScript>())
            playerControllerScript = GetComponent<PlayerControllerScript>();
        else
            Debug.LogError("TouchLogicScript needs PlayerControllerScript in Playerobject");
    }

    void Update()
    {
        // Check for all chosen inputs
        if (touchInput)
            CheckTouchInput();
        if (keyInput)
            CheckKeyInput(); 
        /* TODO
         * BEREINIGEN BEI FERTIGSTELLUNG!!!!
         */
    }

    void CheckTouchInput()
    {
        if (Input.touchCount > 0)
            for (int currentTouch = 0; currentTouch < Input.touchCount; currentTouch++)
            {
                Touch touch = Input.GetTouch(currentTouch);
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        touchStartPos[currentTouch] = touch.position;
                        touchStartTime[currentTouch] = Time.time;
                        break;
                    case TouchPhase.Moved:
                        // Swipe up
                        if (touch.position.y >= touchStartPos[currentTouch].y + minSwipDist)
                        {
                            touchStartPos[currentTouch] = touch.position;
                            InputUp();
                        }
                        // Swipe down
                        else if (touch.position.y <= touchStartPos[currentTouch].y - minSwipDist)
                        {
                            touchStartPos[currentTouch] = touch.position;
                            InputDown();
                        }
                        break;
                    case TouchPhase.Canceled:
                    case TouchPhase.Ended:
                        // Tipe short
                        if (Time.time - touchStartTime[currentTouch] <= maxTipeTime)
                            TipeShort();
                        break;
                }
            }
    }

    void CheckKeyInput()
    {
        if (Input.anyKey)
            // Key right
            if (Input.GetKey(KeyCode.RightArrow))
                InputRight();
            // Key left
            else if (Input.GetKey(KeyCode.LeftArrow))
                InputLeft();
            /* TODO
            * BEREINIGEN BEI FERTIGSTELLUNG!!!!
            */
            // Key up
            else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
                InputUp();
            // Key down
            else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
                InputDown();
            else if (Input.GetKeyDown(KeyCode.Space))
                TipeShort();
    }

    // All input methodes
    void InputRight() { }
    void InputLeft() { }
    void InputUp() { playerControllerScript.SetMovementPhase(MovementPhase.Jump); }
    void InputDown() { playerControllerScript.SetMovementPhase(MovementPhase.Slide); }
    // Touch input methodes
    void TipeShort() { playerControllerScript.SetMovementPhase(MovementPhase.Shoot); }
}