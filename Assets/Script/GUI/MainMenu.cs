
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;
using System;

public class MainMenu : MonoBehaviour, iMenu
{
    [SerializeField]
    private GameObject highScore;
    public GameObject MenuCanvas;
    private Text[] hsText;
    Camera main;
    void Awake()
    {
        hsText = highScore.GetComponentsInChildren<Text>();
    }
    void Start()
    {
        main = Camera.main;
    }
    internal void TurnOffMenu()
    {
        if(main != null)
            main.enabled = true;
        MenuCanvas.SetActive(false);
        Time.timeScale = 1;
    }
    internal void TurnOnMenu()
    {
        if(main != null)
            main.enabled = false;
        MenuCanvas.SetActive(true);
        Time.timeScale = 0f;
    }
    public void ShowHighScore()
    {
        DisableEnable("Return to Menu"); //Aktiverar tillbaka-knappen och avaktivera alla andra
        if (!File.Exists(Settings.Instance.HighScoreFilePath))
        {
            hsText[0].text = "No high scores have yet been recorded";
            return;
        }
        else
            ResetHsText();

        Dictionary<string, int> LoadHighScore = LevelManager.LoadHighScore();
        var ordered = LoadHighScore.OrderByDescending(x => x.Value);

        for (int i = 0; i < LoadHighScore.Count; i++)
        {
            var hsItem = ordered.ElementAt(i);
            hsText[0].text += "Name: " + hsItem.Key + "\n";

            hsText[1].text += "Score: " + hsItem.Value + "\n";
        }
    }

    private void ResetHsText()
    {
        hsText[0].text = "";
        hsText[1].text = "";
    }
    public void ReturnToMenu()
    {
        ResetHsText();
        DisableEnable("Return to Menu"); //Avaktiverar tillbaka-knappen och aktivera alla andra
    }
    /// <summary>
    /// Avaktiverar samtliga knappar utom den med namnet [str]. 
    /// </summary>
    private void DisableEnable(string str)
    {
        foreach (Transform child in MenuCanvas.transform)  
        {
            if (child.tag == "Button")
                child.gameObject.SetActive(!child.gameObject.activeSelf);             
        }
    }
    public void NewGame()
    {
        Time.timeScale = 1;
        //Settings.Instance.TotalScore = 0;
        SceneManager.LoadScene(1);
    }
    public void DeleteSaves()
    {
        PlayerPrefs.DeleteAll();
        if (File.Exists(Settings.Instance.HighScoreFilePath))
            File.Delete(Settings.Instance.HighScoreFilePath);
        Refresh();
    }
    public void LoadGame()
    {

        Settings.Instance.Level = PlayerPrefs.GetInt("Level");
        if (Settings.Instance.Level == 0)
        {
            Debug.Log("No Saved Games");
            Refresh();
        }
        else
        {
            Settings.Instance.TotalScore = PlayerPrefs.GetInt("Score");
            SceneManager.LoadScene(Settings.Instance.Level);
            Time.timeScale = 1;
        }
    }
    private void Refresh()
    {
        TurnOffMenu(); //"Hacky" fix för att knappar forsätter vara makrerade
        TurnOnMenu();
    }
    public void Exit()
    {
        Application.Quit();
    }
}
