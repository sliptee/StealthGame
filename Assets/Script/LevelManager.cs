using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static bool IsInMenu
    {
        get
        {
            try
            {
                return !Camera.main.enabled;//Om vi är i pausmenyn är huvudkameran ej aktiverad 
            }
            catch
            {
                return true; //Anars finns ingen Camera.main... 
            }
        } 
    }
    public static bool GameIsOver;
    public GameObject GameOverUI;
    private static Menu GameMenu;

    void Start()
    {
        Enemy.PlayerSeen += ShowGameOverUI;
        GameMenu = GameObject.FindGameObjectWithTag("GameMenu").transform.GetOrAddComponent<Menu>();
    }
    void OnDestroy() //Statiska variabler förstörs ej då ny scen laddas... 
    {
        Enemy.PlayerSeen -= ShowGameOverUI;
    }
    void ShowGameOverUI()
    {
        print("Game Over");
        Time.timeScale = 0;
        GameIsOver = true;
        GameOverUI.SetActive(true);
        Camera.main.enabled = false;
        Enemy.PlayerSeen -= ShowGameOverUI;
    }
    public void TryAgain()
    {
        Time.timeScale = 1;
        GameIsOver = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public static void OnAdvancing()
    {
        Settings.Instance.TotalScore += Settings.Instance.TemporaryScore;       
        Settings.Instance.Level++;
        if (Settings.Instance.Level > SceneManager.sceneCountInBuildSettings-1) //Player won! 
        {
            GameMenu.TurnOnMenu();
            GameMenu.SaveHighScore();
        }
        else
        {
            SceneManager.LoadSceneAsync(Settings.Instance.Level);
        }
    }

    /// <summary>
    /// Sparar spelarens rekord i en textfil med sökvägen [Settings.Instance.HighScoreFilePath]. Ser till att listan är sorterad och tar bort överflödiga rekord. 
    /// </summary>
    /// <param name="name"> Player name</param>
    public static void SaveHighScore(string name)
    {
        Dictionary<string, int> highScores = LoadHighScore();
        int nr = 1;
        if(highScores.ContainsKey(name)) 
        {
            while(highScores.ContainsKey(name + nr)) //Ifall namnet +2 också existerar... 
            {
                nr++;
            }
            Debug.Log("Name already exists in the dictionary, adding a " + nr);
            name += nr;
        }
        highScores.Add(name, Settings.Instance.TotalScore);
        var ordered = highScores.OrderByDescending(x => x.Value);

        while (highScores.Count > Settings.Instance.MaxNumberofHighScores)
        {
            highScores.Remove(ordered.Last().Key);
        }

        using (StreamWriter file = File.CreateText(Settings.Instance.HighScoreFilePath)) //Skapar eller skriver över en textfil.or overwrites a textfile. Givet det låga antalet rekord krävs inte att ta hänsyn till prestanda. 
        {
            for (int i = 0; i < highScores.Count; i++)
            {
                file.WriteLine(highScores.ElementAt(i).Key + "|" +highScores.ElementAt(i).Value); // '|' används som seperator för namnet och värdet 
            }
        }
    }
    /// <summary>
    /// Laddar alla highscores i en dictionary 
    /// </summary>
    public static Dictionary<string, int> LoadHighScore()
    {
        if (!File.Exists(Settings.Instance.HighScoreFilePath))   //Om inga rekord har blivit sparade
        {
            Debug.Log("No High Scores has yet been saved");
            return new Dictionary<string, int>();
        }
        Dictionary<string, int> highScoreDict = new Dictionary<string, int>();
        string[] highScoreList = File.ReadAllLines(Settings.Instance.HighScoreFilePath);

        for (int line = 0; line < highScoreList.Length; line++)
        {
            string[] scoreName = highScoreList[line].Split('|'); //Rekord sparas i formatet: "name|score"
            try
            {
                string name = scoreName[0].ToString();
                int score = Convert.ToInt32(scoreName[1]);
                highScoreDict.Add(name, score);
            }
            catch (Exception e)
            {
                Debug.Log("Something went wrong while converting scores: " + e);
            }
        }
        return highScoreDict;
    }
}
