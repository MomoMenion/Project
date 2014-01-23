using UnityEngine;
using System.Collections;

enum MenuState
{
    MainMenu,
    HelpMenu
}

public class MainMenuScript : MonoBehaviour
{
    // Public unity setter
    public float interfaceScale = 1;
    public string[] buttonCaption = { "Play", "Help", "Quit" };
    public Texture2D soundButton;
    //public GUIStyle menuStyle, buttonStyle;

    // Screen variables
    float screenWidth = Screen.width, screenHeight = Screen.height;

    // Menu variables
    MainInterfaceScript mainInterfaceScript;
    MenuState menuState = MenuState.MainMenu;
    Rect boxPosition, buttonPosition, soundButtonPosition;
    GameObject[] soundObjects;

    // Use this for initialization
    void Start()
    {
        // Box width = 2/10, height = 3/10
        boxPosition = new Rect(screenWidth / 2 - screenWidth / 10 * interfaceScale,
                               screenHeight / 2 - screenHeight / 20 * interfaceScale * 3,
                               screenWidth / 5 * interfaceScale,
                               screenHeight / 10 * interfaceScale * 3);

        buttonPosition = new Rect(boxPosition.x + boxPosition.width * 0.1f,
                                  boxPosition.y,
                                  boxPosition.width * 0.8f,
                                  boxPosition.height / buttonCaption.Length / 2);

        // Pause button
        int scaler = 7;
        soundButtonPosition = new Rect(Screen.width * 0.95f - soundButton.width / scaler, Screen.height * 0.1f - soundButton.height / scaler, soundButton.width / scaler, soundButton.height / scaler);

        soundObjects = GameObject.FindGameObjectsWithTag("SoundObject");
    }

    void OnGUI()
    {
        // Choose between menu
        switch (menuState)
        {
            case MenuState.HelpMenu:
                HelpMenuState();
                break;
            case MenuState.MainMenu:
            default:
                MainMenuState();
                break;
        }

        if (GUI.Button(soundButtonPosition, soundButton))
            DoSound();
    }

    void DoSound()
    {
        foreach (GameObject soundObject in soundObjects)
            soundObject.SetActive(!soundObject.activeSelf);
    }

    void MainMenuState()
    {
        // Create box
        GUI.Box(boxPosition, "Main Menu");

        // Set button position y
        buttonPosition.y = boxPosition.y;

        // Creat all buttons
        for (int i = 0; i < buttonCaption.Length; i++)
        {
            buttonPosition.y += buttonPosition.height * 1.5f;

            if (GUI.Button(buttonPosition, buttonCaption[i]))
                switch (buttonCaption[i])
                {
                    case "Play":
                        Application.LoadLevel("MainGame");
                        break;
                    case "Help":
                        menuState = MenuState.HelpMenu;
                        break;
                    case "Quit":
                        Application.Quit();
                        break;
                }
        }
    }

    void HelpMenuState()
    {
        // Create box
        GUI.Box(boxPosition, "Help Menu");

        // Create label with help text
        Rect labelPosition = boxPosition;
        labelPosition.y += buttonPosition.height * 1.5f;
        labelPosition.x = buttonPosition.x;
        GUI.Label(labelPosition, "Here you get some help...");

        // Set button position y
        buttonPosition.y = boxPosition.y + buttonPosition.height * 1.5f * buttonCaption.Length;

        // create return button
        if (GUI.Button(buttonPosition, "Return"))
            menuState = MenuState.MainMenu;
    }
}
