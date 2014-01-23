using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour
{
    public float moveSpeed = 0.1f;
    Transform cachedTransform;
    Vector3 startingPosition;

    // Use this for initialization
    void Start()
    {
        cachedTransform = transform;
        startingPosition = cachedTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            cachedTransform.position = new Vector3(
                Mathf.Clamp((Input.GetTouch(0).deltaPosition.x * 0.1f) * cachedTransform.position.x,
                startingPosition.x + 2.5f, startingPosition.x + 2.5f),
                cachedTransform.position.y,
                cachedTransform.position.z);
        }
        /*if (Input.GetKey(KeyCode.LeftArrow))
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        else if (Input.GetKey(KeyCode.RightArrow))
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        else if (Input.GetKey(KeyCode.Space))
            transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);*/
    }
}
