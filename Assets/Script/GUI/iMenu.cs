using UnityEngine;
using System.Collections;

public interface iMenu
{
    /// <summary>
    /// Shows highscorelist
    /// </summary>
    void ShowHighScore();
    /// <summary>
    /// Load from local save
    /// </summary>
    void LoadGame();
    /// <summary>
    /// Exits game
    /// </summary>
    void Exit();
    
}
