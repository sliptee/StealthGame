using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class LevelManager : MonoBehaviour
{
    public static void OnAdvancing()
    {
        Settings.Instance.Level++;
        PlayerPrefs.SetInt("Level", Settings.Instance.Level);
    }
    /// <summary>
    /// Sparar spelarens rekord i en textfil med sökvägen [Settings.Instance.HighScoreFilePath]. Ser till att listan är sorterad och tar bort överflödiga rekord. 
    /// </summary>
    /// <param name="name"> Player name</param>
    public static void SaveHighScore(string name)
    {
        SortedDictionary<int, string> highScores = LoadHighScore();
        highScores.Add(Settings.Instance.Score, name);
        while (highScores.Count > Settings.Instance.MaxNumberofHighScores)
        {
            highScores.Remove(highScores.Last().Key);
        }

        using (StreamWriter file = File.CreateText(Settings.Instance.HighScoreFilePath)) //Skapar eller skriver över en textfil.or overwrites a textfile. Givet det låga antalet rekord krävs inte att ta hänsyn till prestanda. 
        {
            for (int i = 0; i < highScores.Count; i++)
            {
                file.WriteLine(highScores.ElementAt(i).Key + "|" +highScores.ElementAt(i).Value); //Using | as seperator for 
            }
        }
    }
    /// <summary>
    /// Laddar alla highscores i en dictionary 
    /// </summary>
    public static SortedDictionary<int, string> LoadHighScore()
    {
        if (!File.Exists(Settings.Instance.HighScoreFilePath))   //Om inga rekord har blivit sparade
        {
            Debug.Log("No High Scores has yet been saved");
            return new SortedDictionary<int, string>();
        }
        SortedDictionary<int, string> highScoreDict = new SortedDictionary<int, string>(new DescendingComparer<int>());
        string[] highScoreList = File.ReadAllLines(Settings.Instance.HighScoreFilePath);

        for (int line = 0; line < highScoreList.Length; line++)
        {
            string[] scoreName = highScoreList[line].Split('|'); //Rekord sparas i formatet: "name|score"
            string name = scoreName[0].ToString();
            int score = Convert.ToInt32(scoreName[1]);
            highScoreDict.Add(score, name);                    
        }
        return highScoreDict;
    }



}
