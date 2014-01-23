using UnityEngine;
using System.Collections;

public class MainInterfaceScript : MonoBehaviour
{
    public Texture2D pauseButton, soundButton;

    Rect pauseButtonPosition, soundButtonPosition;

    GameObject[] soundObjects;

    // Use this for initialization
    void Start()
    {
        int scaler = 7;
        pauseButtonPosition = new Rect(Screen.width * 0.9f - pauseButton.width / scaler, Screen.height * 0.1f - pauseButton.height / scaler, pauseButton.width / scaler, pauseButton.height / scaler);
        soundButtonPosition = new Rect(Screen.width * 0.95f - soundButton.width / scaler, Screen.height * 0.1f - soundButton.height / scaler, soundButton.width / scaler, soundButton.height / scaler);

        soundObjects = GameObject.FindGameObjectsWithTag("SoundObject");
    }

    void OnGUI()
    {
        if (GUI.Button(pauseButtonPosition, pauseButton))
            DoPause();
        if (GUI.Button(soundButtonPosition, soundButton))
            DoSound();
    }

    void DoPause()
    {
        Debug.Log("Pause");
    }
    void DoSound()
    {
        foreach (GameObject soundObject in soundObjects)
            soundObject.SetActive(!soundObject.activeSelf);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
