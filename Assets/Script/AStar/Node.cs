using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Hjälpklass för A*
/// </summary>
public class Node
{
    #region Declarations
    private const int StraightLineCost = 10;
    private const int DiagonalCost = 14; //Pythagoras sats säger att hyptoenusan ges av roten ur summan av (katederna i kvadrat), vilket ger roten ur(10^2+10^2) ≈ 14. Därför används 14 för diagonalla kostnader.
                                         /// <summary>
                                         /// G = längden från första punkten till denna
                                         /// </summary>
    public float G { get; set; }
    /// <summary>
    /// H = (Heurestisk) Avståndet från denna till den slutpunkten. Beräknas här med pythagoras sats, kan annars beräknas med bla. manhattangeometri. ||a.x-b.x|| + ||a.y-b.y||
    /// </summary>
    public float H { get; set; }
    /// <summary>
    /// F =  beräkning av den totala distansen då denna punkten väljs (F = G + H) 
    /// </summary>
    public float F { get { return G + H; } }
    public Node Parent { get { return parent; } set { parent = value; if(parent != null) SetGValue(); } } //Vi har en förälder, därför vet vi nu också G värdet. 
    private Node parent;

    public NodeState State { get; set; }

    public bool Walkable { get; set; }

    public Vector3 WorldPos { get; set; }

    #endregion
    #region Properties
    //Positionen i förhållande till andra tiles

    public Vector2 LocationInGrid
    {
        get
        {
            return locationInGrid;
        }
        set
        {
            //Makes sure the location stays inside the bounds of the map
            locationInGrid.x = Mathf.Clamp(value.x, 0, GridManager.GridSize.x);
            locationInGrid.y = Mathf.Clamp(value.y, 0, GridManager.GridSize.y);
        }
    }
    private Vector2 locationInGrid;
    #endregion
    #region Help Methods
    public static float GetTraversalCost(Vector2 thisNodeLoc, Vector2 parentNodeLoc) //Få G Kostnaden
    {
        //Om både y och x förändras krävs diagonal rörelse, därför blir G den diagonala kostnaden
        if ((thisNodeLoc.x + parentNodeLoc.x) % 2 == (thisNodeLoc.y + parentNodeLoc.y) % 2)
        {
            return DiagonalCost;
        }
        else
        {
            return StraightLineCost;
        }
    }

    public float HCost(Vector2 EndTile)
    {
        return Mathf.Abs(Vector2.Distance(locationInGrid, EndTile) * DiagonalCost);
        //return (float)Math.Sqrt(Math.Pow((EndTile.x - locationInGrid.x), 2) + Math.Pow((EndTile.y - locationInGrid.y), 2)) * StraightLineCost;
    }
    private void SetGValue()
    {
        G = (parent.G + GetTraversalCost(LocationInGrid, Parent.LocationInGrid));
    }

    #endregion
    #region Constructor
    public Node(bool walkable, Vector3 worldPos, Vector2 locationInGrid)
    {
        Walkable = walkable;
        WorldPos = worldPos;
        LocationInGrid = locationInGrid;
    }

    #endregion
}


public enum NodeState
{
    NotChecked, Open, Closed
}


