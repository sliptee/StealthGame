using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStar : MonoBehaviour
{
    private static int searchWidth;
    private static int searchHeight;
    private GridManager grid;

    void Awake()
    {
        grid = GetComponent<GridManager>();
        searchWidth = Settings.Instance.SearchWidth;
        searchHeight = Settings.Instance.SearchHeight;
    }
    #region Declarations
    //Håller koll på de nodes som är öppna 
    List<Node> OpenList = new List<Node>();

    Node startNode;
    Node goalNode;
    #endregion


    //Simpe A* Pathfinding algorithm
    public List<Vector2> FindPath(Vector3 startPos, Vector3 goalPos)
    {
        OpenList.Clear();

        List<Vector2> path = new List<Vector2>();
        startNode = grid.PositionToNode(startPos);
        goalNode = grid.PositionToNode(goalPos);
        if(startNode.Walkable && goalNode.Walkable)
        {
            bool success = Search(startNode);
            if (success) //Ifall vi hittar en väg leta upp de föräldrar vi satt till våran slutnode och returnera dem i en lista
            {
                Node node = goalNode;
                while (node.Parent != null)
                {
                    path.Add(node.LocationInGrid);
                    node = node.Parent;
                }
                path.Reverse();
            }
        }
        return path;
    }


    private bool Search(Node currentNode)
    {
        currentNode.State = NodeState.Closed;
        if (currentNode != startNode)
            OpenList.Remove(currentNode); //Noden är en del av vägen 

        OpenList.InsertRange(OpenList.Count, FindAdjecentWalkableNodesCost(currentNode, goalNode.LocationInGrid));

        //Ordna listan så att det lägsta f-talet ligger först
        OpenList.Sort((node1, node2) => node1.F.CompareTo(node2.F));


        foreach (Node nextNode in OpenList)
        {
            if (nextNode.LocationInGrid == goalNode.LocationInGrid) //Vi hittade målet. Returnera vägen. 
            {
                return true;
            }
            else
            {
                if (Search(nextNode)) //Rekursion. Söker efter nodes bortom den första 
                    return true;
            }

        }
        return false;
    }

    private List<Node> FindAdjecentWalkableNodesCost(Node node, Vector2 goalTile)
    {
        List<Node> adjecentWalkable = new List<Node>();
        List<Node> nextLocations = FindAdjecentNodes(node);

        foreach (Node locNode in nextLocations)
        {
            int x = (int)locNode.LocationInGrid.x;
            int y = (int)locNode.LocationInGrid.y;

            if ((x < 0) || (Mathf.Abs(x) > (searchWidth - 1)) || (y < 0) || (Mathf.Abs(y) > (searchHeight - 1)))
                continue;
            Node nextNode = grid.grid[x, y];
            if (!nextNode.Walkable)
                continue;

            if (nextNode.State == NodeState.Closed)
                continue;


            if (nextNode.State == NodeState.Open)
            {
                float traversalCost = Node.GetTraversalCost(nextNode.LocationInGrid, node.LocationInGrid);
                float gTemp = node.G + traversalCost;

                if (gTemp < nextNode.G) // Lägg till noden ifall dens G värde är mindre på denna vägen
                {
                    nextNode.Parent = node;
                    nextNode.G = gTemp;
                    adjecentWalkable.Add(nextNode);
                }
            }
            else
            {
                //Om noden inte blivit kollad, sätter vi föräldern och flaggar den som "öppen" för senare övervägande
                nextNode.Parent = node;
                nextNode.H = nextNode.HCost(goalTile);
                nextNode.State = NodeState.Open;
                adjecentWalkable.Add(nextNode);
            }
        }
        return adjecentWalkable;
    }

    //Hittar Närliggande (vertikalt,diagonalt och horisontellt) Nodes vilka inte är väggar, gör även diagonalchecks mot väggar
    private List<Node> FindAdjecentNodes(Node node)
    {
        bool upLeft = true;
        bool upRight = true;
        bool downLeft = true;
        bool downRight = true;
        List<Node> adjecentNodes = new List<Node>();
        int X = (int)node.LocationInGrid.x;
        int Y = (int)node.LocationInGrid.y;
        //Kollar vilka av de närliggande nodsen tillåter diagonal rörelse
        if (grid.IsUnpassable(X - 1, Y))
        {
            upRight = false;
            downRight = false;
        }
        if (grid.IsUnpassable(X + 1, Y))
        {
            upLeft = false;
            downLeft = false;
        }
        if (grid.IsUnpassable(X, Y - 1))
        {
            downLeft = false;
            downRight = false;
        }
        if (grid.IsUnpassable(X, Y + 1))
        {
            upLeft = false;
            upRight = false;
        }
        for (int x = -1; x < 2; x++)
        {
            for (int y = -1; y < 2; y++)
            {
                if (X + x > grid.WorldGridSize.x || y + Y > grid.WorldGridSize.y)
                    continue; 
                Node nextNode = grid.grid[X+x, Y+y];
                if (node.LocationInGrid == nextNode.LocationInGrid)
                    continue;

                if ((X + x != X) && (Y + y != Y)) //Om den närligande noden ligger diagonalt ifrån denna. 
                {
                    if (upLeft && nextNode.LocationInGrid.x > X && nextNode.LocationInGrid.y > Y)
                        adjecentNodes.Add(nextNode);
                    else continue;

                    if (upRight && nextNode.LocationInGrid.x > X && nextNode.LocationInGrid.y > Y)
                        adjecentNodes.Add(nextNode);
                    else continue;

                    if (downLeft && nextNode.LocationInGrid.y < Y && nextNode.LocationInGrid.x < X)
                        adjecentNodes.Add(nextNode);
                    else continue;

                    if (downRight && nextNode.LocationInGrid.y < Y && nextNode.LocationInGrid.x > X)
                        adjecentNodes.Add(nextNode);
                    else continue;
                }
                else
                {
                    adjecentNodes.Add(nextNode);
                }
            }
        }
        return adjecentNodes;
    }
}