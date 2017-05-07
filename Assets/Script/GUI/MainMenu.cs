
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;
using System;

public class MainMenu : MonoBehaviour, iMenu
{
    private Vector2 origin;
    public Text HighScoreText;
    public GameObject Canvas;

    void Awake()
    {
        origin = new Vector2(Screen.width / 2, Screen.height / 2);
    }
    internal void TurnOffMenu()
    {
        Canvas.SetActive(false);
    }
    internal void TurnOnMenu()
    {
        Canvas.SetActive(true);
    }
    public void ShowHighScore()
    {
        foreach (Transform child in Canvas.transform)  //Avaktiverar alla aktiva knappar och aktiverar tillbaka-knappen. 
        {
            if (child.tag != "Button")
                continue;
            if (child.name == "Return to Menu")
                child.gameObject.SetActive(true);
            else
            child.gameObject.SetActive(false);
        }
        HighScoreText.text = "No high scores have yet been recorded";
        if (!File.Exists(Settings.Instance.HighScoreFilePath))
            return;
        SortedDictionary<int, string> LoadHighScore = LevelManager.LoadHighScore();
        HighScoreText.gameObject.transform.position = origin;
        HighScoreText.fontSize = 24; 

        for (int i = 0; i < LoadHighScore.Count; i++)
        {
            var hsItem = LoadHighScore.ElementAt(i);
            HighScoreText.text += "Name: " + hsItem.Key + "       Score: " + hsItem.Value + "\n"; 
        }
    }
    public void ReturnToMenu()
    {
        HighScoreText.text = "";
        foreach (Transform child in Canvas.transform)  //Avaktiverar tillbaka-knappen och aktivera alla andra
        {
            if (child.name == "Return to Menu")
                child.gameObject.SetActive(false);
            else
                child.gameObject.SetActive(true);
        }
    }
    public void NewGame()
    {
        SceneManager.LoadScene(1);
    }
    public void LoadGame()
    {
        if(PlayerPrefs.GetInt("Level") == 0)
        {
            Debug.Log("No Saved Games");
            TurnOffMenu(); //"Hacky" fix för att "Load Game" knappen forsätter vara makrerad när inga sparningar hittas. 
            TurnOnMenu();
            return;
        }
        int level = PlayerPrefs.GetInt("Level");
        SceneManager.LoadScene(level);
        RenderSettings.ambientLight = Color.white;

    }
    public void Exit()
    {
        Application.Quit();
    }
}
