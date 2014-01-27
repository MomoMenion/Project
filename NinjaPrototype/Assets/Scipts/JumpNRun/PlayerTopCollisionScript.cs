using UnityEngine;
using System.Collections;

public class PlayerTopCollisionScript : MonoBehaviour
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

    void OnTriggerStay(Collider colliderObject)
    {
        playerControllerScript.TopCollision = true;
    }
    void OnTriggerExit(Collider colliderObject)
    {
        playerControllerScript.TopCollision = false;
    }
}
