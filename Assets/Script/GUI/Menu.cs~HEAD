using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MainMenu
{
    void SaveGame()
    {
        PlayerPrefs.SetInt("Level", Settings.Instance.Level);
    }

    void OnGUI()
    {
        Vector2 origin = new Vector2(Screen.width / 2, Screen.height / 2);
        if (GUI.Button(new Rect(origin.x - Screen.width / 8, origin.y - Screen.height / 4, Screen.width / 4, Screen.height / 7), "Show High Score"))
        {
            ShowHighScore();
        }
        if (GUI.Button(new Rect(origin.x - Screen.width / 8, origin.y - Screen.height / 4 + Screen.height / 5, Screen.width / 4, Screen.height / 7), "Load Game"))
        {
            LoadGame();
        }
        if (GUI.Button(new Rect(origin.x - Screen.width / 8, origin.y - Screen.height / 4 + Screen.height / 2.5f, Screen.width / 4, Screen.height / 7), "Exit"))
        {
            Exit();
        }
    }
}