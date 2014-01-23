using UnityEngine;
using System.Collections;

public class Cube_Test : MonoBehaviour
{
    public GameObject objectRight;

    // Use this for initialization
    void Start()
    {
        Debug.Log("start up");
    }

    void Awake()
    {
        Debug.Log("wake up");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount != 0)
        {
            Debug.Log("touch");
            objectRight.SetActive(false);
        }
        else
        {
            Debug.Log("no touch");
            objectRight.SetActive(true);
        }
        //CheckBounce();
    }

    private void CheckBounce()
    {
        if (gameObject.transform.position.x > 3)
            objectRight.SetActive(false);
        else
            objectRight.SetActive(true);
    }
}
