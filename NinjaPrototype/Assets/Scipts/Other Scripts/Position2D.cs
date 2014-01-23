using UnityEngine;
using System.Collections;

public class Position2D : MonoBehaviour
{
    public float zAxisOffset = 0;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, zAxisOffset);
    }
}
