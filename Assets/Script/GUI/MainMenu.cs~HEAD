
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour, iMenu
{

    public void ShowHighScore()
    {
        if (File.Exists(Settings.Instance.HighScoreFilePath))
            return;
        SortedDictionary<int, string> LoadHighScore = LevelManager.LoadHighScore();
        for (int i = 0; i < LoadHighScore.Count; i++)
        {

        }

    }

    public void LoadGame()
    {
        int level = PlayerPrefs.GetInt("Level");
        SceneManager.LoadScene(level);
    }
    public void Exit()
    {
        Application.Quit();
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
