using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        float percentX = (worldPos.x + worldGridSize.x / 2) / worldGridSize.x;
        float percentY = (worldPos.z + worldGridSize.y / 2) / worldGridSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSize.x - 1) * percentX);
        int y = Mathf.RoundToInt((gridSize.y - 1) * percentY);
        return grid[x, y];
    }
    private Vector2 gridSize
    {
        get { return ConvertToGridLoc(worldGridSize); }
    }

    public bool IsUnpassable(int x, int y)
    {
        return grid[x, y].Walkable;
    }
    #endregion
    [SerializeField]
    private Vector2 worldGridSize;
    private LayerMask walkableMask;
    public Node[,] grid;
    public List<Vector2> path;

    void Update()
    {
        CreateGrid();
        var a = GameObject.FindGameObjectWithTag("AStar").GetComponent<AStar>();
        path = a.FindPath(new Vector3(5, 1, 5), new Vector3(0, 1, 0));
    }
    void Start()
    {

    }
    void OnDrawGizmos()
    {
        if (!Application.isPlaying) return; //Gets rid of null reference when game is stopped
        Gizmos.DrawWireCube(transform.position, new Vector3(worldGridSize.x, 1, worldGridSize.y));

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
        Vector3 worldBottomLeft = transform.position - Vector3.right * worldGridSize.x / 2 - Vector3.forward * worldGridSize.y / 2; //Could make property

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector3 middleOfNode = worldBottomLeft + Vector3.right * (Settings.Instance.TileSize.x * x + Settings.Instance.TileSize.x/2) + Vector3.forward * (y * Settings.Instance.TileSize.y + Settings.Instance.TileSize.y / 2);
                bool walkable = !(Physics.CheckBox(middleOfNode, new Vector3(Settings.Instance.TileSize.x, 1, Settings.Instance.TileSize.y) /2, Quaternion.identity, Settings.Instance.UnpassableLayer));

                int movementPenalty = 0;

                Ray ray = new Ray(middleOfNode + Vector3.up * 50, Vector3.down);
                RaycastHit hit;
                //if (Physics.Raycast(ray, out hit, 100, walkableMask))
                //{
                //    walkableRegionsDictionary.TryGetValue(hit.collider.gameObject.layer, out movementPenalty);
                //}

                //if (!walkable)
                //{
                //    movementPenalty += obstacleProximityPenalty;
                //}


                grid[x, y] = new Node(walkable, middleOfNode, new Vector2(x,y), movementPenalty);
            }
        }

       // BlurPenaltyMap(3);

    }
}
