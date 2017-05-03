using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MainMenu
{
    Camera main;
    void Start()
    {
        main = Camera.main;
    }
    public void SaveGame()
    {
        PlayerPrefs.SetInt("Level", Settings.Instance.Level);
    }
    public void Continue()
    {
        main.enabled = true;
        TurnOffMenu();
    }
    public void Update()
    {
        if (Input.GetButton("Cancel"))
        {
            main.enabled = false;
            TurnOnMenu();
        }
    }
}