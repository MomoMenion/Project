using UnityEngine;
using System.Collections;

public class MenuBackgroundScript : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Transform child in transform)
            child.transform.Rotate(Vector3.up, 1);
    }
}
