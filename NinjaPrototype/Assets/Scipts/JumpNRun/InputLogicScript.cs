using UnityEngine;
using System.Collections;

public class InputLogicScript : MonoBehaviour
{
    // Public unity setter
    public bool touchInput = true, keyInput = false;
    public float minSwipDist = 10.0f, maxTipeTime = 0.1f;

    // Touch variables
    Vector2 touchStartPos;
    float touchStartTime;
    bool touched;

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
    }

    void CheckTouchInput()
    {
        if (Input.touchCount > 0)
            foreach (Touch touch in Input.touches)
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        touchStartPos = touch.position;
                        touchStartTime = Time.time;
                        break;
                    case TouchPhase.Moved:
                        if (!touched)
                            //// Swipe right
                            //if (touch.position.x >= touchStartPos.x + minSwipDist)
                            //{
                            //    InputRight();
                            //    touched = true;
                            //}
                            //// Swipe left
                            //else if (touch.position.x <= touchStartPos.x - minSwipDist)
                            //{
                            //    InputLeft();
                            //    touched = true;
                            //}
                            // Swipe up
                            if (touch.position.y >= touchStartPos.y + minSwipDist)
                            {
                                InputUp();
                                touched = true;
                            }
                            // Swipe down
                            else if (touch.position.y <= touchStartPos.y - minSwipDist)
                            {
                                InputDown();
                                touched = true;
                            }
                        break;
                    //case TouchPhase.Stationary:
                    //    // Tipe long
                    //    if (!touched && Time.time - touchStartTime >= maxTipeTime)
                    //    {
                    //        TipeLong();
                    //        touched = true;
                    //    }
                    //    break;
                    case TouchPhase.Canceled:
                    case TouchPhase.Ended:
                        // Tipe short
                        if (!touched && Time.time - touchStartTime <= maxTipeTime)
                            TipeShort();
                        touched = false;
                        break;
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
            // Key up
            else if (Input.GetKeyDown(KeyCode.UpArrow) || (Input.GetKeyDown(KeyCode.Space)))
                InputUp();
            // Key down
            else if (Input.GetKeyDown(KeyCode.DownArrow))
                InputDown();
    }

    // All input methodes
    void InputRight() { }
    void InputLeft() { }
    void InputUp() { playerControllerScript.SetMovementPhase(MovementPhase.Jump); }
    void InputDown() { playerControllerScript.SetMovementPhase(MovementPhase.Cower); }
    // Touch input methodes
    void TipeShort() { }
    void TipeLong() { }
}