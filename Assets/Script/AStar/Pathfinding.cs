using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStar : MonoBehaviour
{
    private static int searchWidth;
    private static int searchHeight;

    void Start()
    {
        Enemy enemy = transform.GetOrAddComponent<Enemy>();
        searchWidth = Settings.Instance.SearchWidth;
        searchHeight = Settings.Instance.SearchHeight;
    }
    #region Declarations
    //Håller koll på de nodes som är öppna 
    List<Node> OpenList = new List<Node>();

    Node[,] nodes = new Node[searchWidth, searchHeight];
    Node startNode;
    Node goalNode;
    //Om vi öppnat mer än [detta talet] nodes, sluta sökningen där för att spara prestanda. Gör det sämsta möjliga scenariot betydligt bättre. 
    public int MaxNumberOfOpenNodes = 35;

    #endregion


    //Simpe A* Pathfinding algorithm
    public List<Vector2> FindPath(Vector2 startTile, Vector2 goalTile)
    {
        if ((startTile == goalTile) || (startTile.x > searchWidth) || (startTile.x < 0) || (goalTile.x > searchWidth) || (goalTile.x < 0) ||
             (startTile.y > searchHeight) || (startTile.y < 0) || (goalTile.y > searchHeight) || (goalTile.y < 0))
            return null;

        OpenList.Clear();

        List<Vector2> path = new List<Vector2>();
        startNode = nodes[(int)startTile.x, (int)startTile.y];
        goalNode = nodes[(int)goalTile.x, (int)goalTile.y];

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
        return path;
    }


    private bool Search(Node currentNode)
    {
        currentNode.State = NodeState.Closed;
        if (currentNode != startNode)
            OpenList.Remove(currentNode);

        OpenList.InsertRange(OpenList.Count, FindAdjecentWalkableNodesCost(currentNode, goalNode.LocationInGrid));

        //Ordna listan så att det lägsta f-talet ligger först
        OpenList.Sort((node1, node2) => node1.F.CompareTo(node2.F));


        foreach (Node nextNode in OpenList)
        {
            if (OpenList.Count > MaxNumberOfOpenNodes)
                return false;

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
        List<Vector2> nextLocations = FindAdjecentNodes(node);

        foreach (Vector2 location in nextLocations)
        {
            int x = (int)location.x;
            int y = (int)location.y;

            if ((x < 0) || (x > (searchWidth - 1)) || (y < 0) || (y > (searchHeight - 1)))
                continue;
            Node nextNode = nodes[x, y];
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
    private List<Vector2> FindAdjecentNodes(Node node)
    {
        bool upLeft = true;
        bool upRight = true;
        bool downLeft = true;
        bool downRight = true;
        List<Vector2> adjecentNodes = new List<Vector2>();
        for (int x = -1; x < 2; x++)
        {
            for (int y = -1; y < 2; y++)
            {
                int X = (int)node.LocationInGrid.x;
                int Y = (int)node.LocationInGrid.y;

                Node nextNode = new Node();
                nextNode.LocationInGrid = new Vector2(x + x, y + y);


                //Kollar vilka av de närliggande nodsen tillåter diagonal rörelse
                if (GridManager.IsUnpassable(x, y, new Vector3(x - 1, y)))
                {
                    upLeft = false;
                    downLeft = false;
                }
                if (GridManager.IsUnpassable(x, y, new Vector3(x + 1, y)))
                {
                    upRight = false;
                    downRight = false;
                }
                if (GridManager.IsUnpassable(x, y, new Vector3(x, y - 1)))
                {
                    upLeft = false;
                    upRight = false;
                }
                if (GridManager.IsUnpassable(x, y, new Vector3(x, y + 1)))
                {
                    downLeft = false;
                    downRight = false;
                }

                if ((x + x != x) && (y + y != y)) //Diagonal node hittad
                {
                    if (upLeft && nextNode.LocationInGrid.x < x)
                        adjecentNodes.Add(nextNode.LocationInGrid);
                    else continue;

                    if (upRight && nextNode.LocationInGrid.x > x)
                        adjecentNodes.Add(nextNode.LocationInGrid);
                    else continue;

                    if (downLeft && nextNode.LocationInGrid.y < y)
                        adjecentNodes.Add(nextNode.LocationInGrid);
                    else continue;

                    if (downRight && nextNode.LocationInGrid.y > y)
                        adjecentNodes.Add(nextNode.LocationInGrid);
                    else continue;
                }
                else if (nextNode.LocationInGrid != node.LocationInGrid)
                {
                    adjecentNodes.Add(nextNode.LocationInGrid);
                }
            }
        }
        return adjecentNodes;
    }
}