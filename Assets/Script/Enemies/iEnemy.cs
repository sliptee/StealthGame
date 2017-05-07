using UnityEngine;
using System.Collections;

public interface iEnemy
{
    /// <summary>
    /// Walk around in a predictable manner, set from pathFinderAI 
    /// </summary>
    void Walk();
    /// <summary>
    /// Turn around looking for the player in a predictable manner, also visualises enemy FOV
    /// </summary>
    void Look(); 
	
}
