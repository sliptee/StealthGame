using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Denna fienden är 1.5 gånger så snabb, men har en lägre synvinkel.
/// </summary>
public class FastEnemy : Enemy
{
    protected override void Start()
    {
        base.Start();
        splight.spotAngle = 50;
        speed *= 1.5f;
        timeUntilPlayerSpotted = 0.2f;
    }

}
