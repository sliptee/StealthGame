using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Skapar en grid för A*. Mindre juteringar krävs ifall objectet denna script tillhör inte har koordinaterna 0,0. 
/// </summary>
public class GridManager : MonoBehaviour
{
    #region Properties
    public Vector2 ConvertToWorldLoc(Vector2 gridPos)
    {
        return new Vector2(gridPos.x * Settings.Instance.TileSize.x, gridPos.y * Settings.Instance.TileSize.y);
    }
    public Vector2 ConvertToGridLoc(Vector3 worldPos)
    {
        return new Vector2(Mathf.RoundToInt(worldPos.x / Settings.Instance.TileSize.x), Mathf.RoundToInt(worldPos.y / Settings.Instance.TileSize.y));
    }
    public Node PositionToNode(Vector3 worldPos)
    {
        //float percentX = (worldPos.x + WorldGridSize.x / 2) / WorldGridSize.x;
        //float percentY = (worldPos.z + WorldGridSize.y / 2) / WorldGridSize.y;
        //percentX = Mathf.Clamp01(percentX);
        //percentY = Mathf.Clamp01(percentY);

        //int x = Mathf.RoundToInt((gridSize.x - 1) * percentX);
        //int y = Mathf.RoundToInt((gridSize.y - 1) * percentY);
        Vector2 gridLoc = ConvertToGridLoc(worldPos);
        return grid[(int)gridLoc.x, (int)gridLoc.y];
    }
    private Vector2 gridSize
    {
        get { return ConvertToGridLoc(WorldGridSize); }
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
    private LayerMask walkableMask;
    public Node[,] grid;
    public List<Vector2> path;

    void Awake()
    {
        CreateGrid();
    }

    void Start()
    {
        var a = GameObject.FindGameObjectWithTag("AStar").GetComponent<AStar>();
        path = a.FindPath(new Vector3(1, 1,1), new Vector3(49, 49, 1));
    }
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(WorldGridSize.x, 1, WorldGridSize.y));
        if (!Application.isPlaying) return; //När spelet inte körs finns ingen grid.. 

        if(grid != null)
        {
            foreach(Node n in grid)
            {
                if(path != null)
                    if(path.Contains(n.LocationInGrid))
                    {
                        Gizmos.color = Color.cyan;
                        Gizmos.DrawCube(n.WorldPos, new Vector3(Settings.Instance.TileSize.x - .1f, 1, Settings.Instance.TileSize.y - .1f));
                        continue;
                    }
                Gizmos.color = (n.Walkable) ? Color.green : Color.red;
                Gizmos.DrawCube(n.WorldPos, new Vector3(Settings.Instance.TileSize.x - .1f, 1, Settings.Instance.TileSize.y - .1f));
            }
        }
    }

    void CreateGrid()
    {
        grid = new Node[(int)gridSize.x, (int)gridSize.y];
        Vector3 worldBottomLeft = transform.position - Vector3.right * WorldGridSize.x / 2 - Vector3.forward * WorldGridSize.y / 2; 

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector3 middleOfNode = worldBottomLeft + Vector3.right * (Settings.Instance.TileSize.x * x + Settings.Instance.TileSize.x/2) + Vector3.forward * (y * Settings.Instance.TileSize.y + Settings.Instance.TileSize.y / 2);
                bool walkable = !(Physics.CheckBox(middleOfNode, new Vector3(Settings.Instance.TileSize.x, 1, Settings.Instance.TileSize.y) /2, Quaternion.identity, Settings.Instance.UnpassableLayer));

                grid[x, y] = new Node(walkable, middleOfNode, new Vector2(x,y));
            }
        }

       // BlurPenaltyMap(3);

    }
}
