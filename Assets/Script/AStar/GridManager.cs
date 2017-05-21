using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Skapar en grid för A*.
/// </summary>
public class GridManager : MonoBehaviour
{
    #region Properties
    public static Vector2 ConvertToGrid(Vector3 worldPos)
    {
        return new Vector2(Mathf.RoundToInt(worldPos.x / (Settings.Instance.TileSize.x)), Mathf.RoundToInt(worldPos.x / (Settings.Instance.TileSize.y)));
    }
    public Vector2 PositionToGrid(Vector3 worldPos)
    {
        float percentX = (worldPos.x + WorldGridSize.x / 2) / WorldGridSize.x;
        float percentY = (worldPos.z + WorldGridSize.y / 2) / WorldGridSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((GridSize.x - 1) * percentX);
        int y = Mathf.RoundToInt((GridSize.y - 1) * percentY);
        return new Vector2(x, y);
    }
    public Node PositionToNode(Vector3 worldPos)
    {
        Vector2 gridPos = PositionToGrid(worldPos);
        return grid[(int)gridPos.x, (int)gridPos.y];
    }
    public static Vector2 GridSize
    {
        get { return ConvertToGrid(wSize); }
    }

    public bool IsUnpassable(int x, int y)
    {
        return !grid[x, y].Walkable;
    }
    public bool IsUnpassable(Vector2 gridLoc)
    {
        return IsUnpassable((int)gridLoc.x, (int)gridLoc.y);
    }
    #endregion
    public Vector2 WorldGridSize;
    private static Vector2 wSize;
    public LayerMask UnpassableLayer;
    public Node[,] grid;
    private Enemy enem;
    void Awake()
    {
        wSize = WorldGridSize;
        CreateGrid();
        //enem = GameObject.Find("Enemy").GetComponent<Enemy>();
    }


    void OnDrawGizmos()
    {
        if (!Selection.Contains(gameObject)) //Visa bara gizmon om det här objektet är valt i hierarkin
            return;
        Gizmos.DrawWireCube(transform.position, new Vector3(WorldGridSize.x, 1, WorldGridSize.y));
        if (!Application.isPlaying || GridSize.sqrMagnitude > 10000) return; //När spelet inte körs finns ingen grid.. Om GridSize.sqrMagnitude > det godtyckliga talet 10000 riskerar prestandan att försummas till en sådan nivå att det ej är lämpligt.

        if (grid != null)
        {
            foreach (Node n in grid)
            {
                //if (enem.path != null)
                //    if (enem.path.Contains(n.LocationInGrid))
                //    {
                //        Gizmos.color = Color.cyan;
                //        Gizmos.DrawCube(n.WorldPos, Vector3.one * (tileDia));
                //        continue;
                //    }

                Gizmos.color = (n.Walkable) ? Color.green : Color.red;
                Gizmos.DrawCube(n.WorldPos, Vector3.one * (Settings.Instance.TileSize.x));
            }
        }
    }
    /// <summary>
    /// Skapar en grid för användning av A*. 
    /// </summary>
    void CreateGrid()
    {
        grid = new Node[(int)GridSize.x, (int)GridSize.y];
        Vector3 worldBottomLeft = transform.position - Vector3.right * WorldGridSize.x / 2 - Vector3.forward * WorldGridSize.y / 2;

        for (int x = 0; x < GridSize.x; x++)
        {
            for (int y = 0; y < GridSize.y; y++)
            {
                Vector3 middleOfNode = worldBottomLeft + Vector3.right * (Settings.Instance.TileSize.x * x + Settings.Instance.TileSize.x / 2) + Vector3.forward * (y * Settings.Instance.TileSize.y + Settings.Instance.TileSize.y / 2);
                bool walkable = !(Physics.CheckBox(middleOfNode, new Vector3(Settings.Instance.TileSize.x, 1, Settings.Instance.TileSize.y) / 2, Quaternion.identity, UnpassableLayer));
                grid[x, y] = new Node(walkable, middleOfNode, new Vector2(x, y));
            }
        }
    }
}
