using UnityEngine;
using System.Collections;

public interface iPlayer
{
    /// <summary>
    /// Get and handle input from the player
    /// </summary>
    void GetPlayerInput();

    /// <summary>
    /// Moves the character
    /// </summary>
    void MovePlayer();

    /// <summary>
    /// Allows sprinting
    /// </summary>
    void Sprint();
	
}
