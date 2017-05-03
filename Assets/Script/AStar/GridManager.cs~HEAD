using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    #region Properties
    public static Vector2 ConvertToWorldLoc(Vector2 gridPos)
    {
        return new Vector2(gridPos.x * Settings.Instance.TileSize.x, gridPos.y * Settings.Instance.TileSize.y);
    }
    public static Vector2 ConvertToGridLoc(Vector2 worldPos)
    {
        return new Vector2(Mathf.RoundToInt(worldPos.x / Settings.Instance.TileSize.x), Mathf.RoundToInt(worldPos.y / Settings.Instance.TileSize.y));
    }
    private Vector2 gridSize
    {
        get { return ConvertToGridLoc(worldGridSize); }
    }

    public static bool IsUnpassable(int x, int y, Vector3 dir)
    {
        if (Physics.Raycast(ConvertToWorldLoc(new Vector3(x, y, 0.5f)), dir, 10, 8))
        {
            return true;
        }
        else return false;
    }
    #endregion
    [SerializeField]
    private Vector2 worldGridSize;
    private Node[,] grid;
   
    void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position, new Vector3(worldGridSize.x, 1, worldGridSize.y));
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
                bool walkable = !(Physics.CheckBox(middleOfNode, new Vector3(Settings.Instance.TileSize.x, 1, Settings.Instance.TileSize.x), Quaternion.identity, Settings.Instance.UnpassableLayer));

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

        BlurPenaltyMap(3);

    }
}
