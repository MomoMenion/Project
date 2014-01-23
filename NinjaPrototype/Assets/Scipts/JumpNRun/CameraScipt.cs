using UnityEngine;
using System.Collections;

public class CameraScipt : MonoBehaviour
{
    // public unity setter
    public bool permanentMovement = true;
    public float XOffsetToPlayer = 2f;
    public int fieldOfViewSize = 10;
    //public float LookAtDistance = 1;

    public float MovementSpeed { private get; set; }
    Vector3 startPosition;

    // Use this for initialization
    void Start()
    {
        // Set startposition
        startPosition = transform.position;

        //camera = GetComponent<Camera>();
        //camera.fieldOfView = (float)Screen.height * 16 / 9 / (float)Screen.width * fieldOfViewSize;

        //float resolution = (float)Screen.width / (float)Screen.height;
        //Debug.Log("Screen Resolution: " + resolution + " " + Screen.width + " " + Screen.height);
        //Debug.Log("So müsste der height wert sein: " + (float)(Screen.width * 9 / 16) + " wenn groesser als resolution y dann nicht möglich also x kleiner!");
        //Debug.Log("Screen width muesste mal " + (float)Screen.height * 16 / 9 / (float)Screen.width + " genommen werden (wenn wert unter 1 sonst muss height veraendert werden!)");

        //if ((float)Screen.height < (float)(Screen.width * 9 / 16))
        //{
        //    Screen.SetResolution(Screen.height * 16 / 9 / Screen.width, Screen.height, false);
        ////    camera.rect.Set(0, 0, (float)Screen.height * 16 / 9 / (float)Screen.width, 1);
        ////    Debug.Log(camera.rect);
        //}
        //else
        //{
        //    Screen.SetResolution(Screen.width, Screen.width * 9 / 16 / Screen.height, false);
        //    //camera.rect.Set(0, 0, 1, (float)Screen.width * 9 / 16 / (float)Screen.height);
        //    //Debug.Log(camera.rect);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        // Permanent movement
        if (permanentMovement)
            transform.Translate(Vector3.right * MovementSpeed * Time.deltaTime);

        //camera.WorldToScreenPoint(new Vector3(transform.position.x, 0));

        //if (lookAtObject.transform.position.y < LookAtDistance)
        //{
        //    transform.LookAt(lookAtObject);
        //}
    }

    // Set camera to startposition
    public void CameraToStartposition() { transform.position = startPosition; }
}
