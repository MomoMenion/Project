using UnityEngine;
using System.Collections;

public class PlayerTopColliderScript : MonoBehaviour
{
    GameObject playerObject;
    PlayerControllerScript playerControllerScript;

    // Use this for initialization
    void Start()
    {
        #region Check for PlayerObject and set to variable
        if (GameObject.FindGameObjectWithTag("PlayerObject"))
        {
            playerObject = GameObject.FindGameObjectWithTag("PlayerObject");
            // Check for CameraScipt and set to variable
            if (playerObject.GetComponent<PlayerControllerScript>())
                playerControllerScript = playerObject.GetComponent<PlayerControllerScript>();
            else
                Debug.LogError("PlayerTopCollisionScript needs PlayerControllerScript in PlayerObject");
        }
        else
            Debug.LogError("PlayerTopCollisionScript needs Gameobject with \"PlayerObject\" tag");
        #endregion
    }

    // Triggers
    void OnTriggerEnter(Collider colliderObject)
    {
        // Attetion with deleted gameobjects when trigger
        if (colliderObject.tag != "PlayerObject" && 
            colliderObject.tag != "GetCoinTrigger" &&
            colliderObject.tag != "CameraFrontTrigger")
        {
            playerControllerScript.PlayerTopTrigger = true;
            Debug.Log("Enter " + colliderObject.tag);
        }
    }
    void OnTriggerExit(Collider colliderObject)
    {
        if (colliderObject.tag != "PlayerObject")
        {
            playerControllerScript.PlayerTopTrigger = false;
            Debug.Log("Exit " + colliderObject.tag);
        }
    }
}
