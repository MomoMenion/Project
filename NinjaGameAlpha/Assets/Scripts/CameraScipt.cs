using UnityEngine;
using System.Collections;

public class CameraScipt : MonoBehaviour
{
    // public unity setter
    public bool permanentMovement = true;
    public float XOffsetToPlayer = 2f;

    public float MovementSpeed { private get; set; }

    // Update is called once per frame
    void Update()
    {
        // Permanent movement
        if (permanentMovement)
            transform.Translate(Vector3.right * MovementSpeed * Time.deltaTime);
        /* TODO
           * BEREINIGEN BEI FERTIGSTELLUNG!!!!
           */
    }

    // Set camera to position
    public void CameraToPosition(Vector3 position) { transform.position = position - Vector3.right * XOffsetToPlayer; }
    /* TODO
           * BEREINIGEN BEI FERTIGSTELLUNG!!!!
           */
}
