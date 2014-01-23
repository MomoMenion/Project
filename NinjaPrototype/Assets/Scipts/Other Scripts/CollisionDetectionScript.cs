using UnityEngine;
using System.Collections;

public class CollisionDetectionScript : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("detection enter trigger");
    }
    void OnTriggerStay(Collider other)
    {
        Debug.Log("detection stay trigger");
    }
    void OnTriggerExit(Collider other)
    {
        Debug.Log("detection exit trigger");
    }
}
