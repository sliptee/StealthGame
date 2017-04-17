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
    /// Saves the player score as a highscore in a text document with the directory []. Also sorts the list and removes superflous ones. 
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

        using (StreamWriter file = File.CreateText(Settings.Instance.HighScoreFilePath)) //Creates or overwrites a textfile. May not be the most performance-friendly method, though considering the small number of high scores it won't have a large impact.
        {
            for (int i = 0; i < highScores.Count; i++)
            {
                file.WriteLine(highScores.ElementAt(i).Key + "|" +highScores.ElementAt(i).Value); //Using | as seperator for 
            }
        }
    }
    /// <summary>
    /// Loads all high scores as a dictionary
    /// </summary>
    public static SortedDictionary<int, string> LoadHighScore()
    {
        if (!File.Exists(Settings.Instance.HighScoreFilePath))   //If no highscore has yet been created 
        {
            Debug.Log("No High Scores has yet been saved");
            return new SortedDictionary<int, string>();
        }
        SortedDictionary<int, string> highScoreDict = new SortedDictionary<int, string>(new DescendingComparer<int>());
        string[] highScoreList = File.ReadAllLines(Settings.Instance.HighScoreFilePath);

        for (int line = 0; line < highScoreList.Length; line++)
        {
            string[] scoreName = highScoreList[line].Split('|'); //High Scores are saved in the following format: "name|score"
            string name = scoreName[0].ToString();
            int score = Convert.ToInt32(scoreName[1]);
            highScoreDict.Add(score, name);                     //Adds the score to the dictionary
        }
        return highScoreDict;
    }



}
