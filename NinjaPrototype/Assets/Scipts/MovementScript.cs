using UnityEngine;
using System.Collections;

public class MovementScript : MonoBehaviour
{
    public bool permanentMove = true;
    public float moveSpeed = 10;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
        
        if (permanentMove)
            //rigidbody.MovePosition(Vector3.right * moveSpeed * Time.deltaTime);
            //rigidbody.transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
    }
}
