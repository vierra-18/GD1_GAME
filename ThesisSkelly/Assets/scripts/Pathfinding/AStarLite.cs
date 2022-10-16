using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AStarLite : MonoBehaviour
{
    public Transform seeker, target;
    int gridSizeX = 190;
    int gridSizeY = 110;

    float cellSize = 0.3f;

    AStarNode[,] aStarNodes;

    AStarNode startNode;

    List<AStarNode> nodesToCheck= new List<AStarNode>();
    List<AStarNode> nodesChecked= new List<AStarNode>();

    List<Vector3> aiPath = new List<Vector3>();

    //Debug
    Vector3 startPositionDebug = new Vector3(0, 0, 0);
    Vector3 destinationPositionDebug = new Vector3(1000, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        CreateGrid();

      
    }
    private void Update()
    {
        FindPath(target.position);
    }
    void CreateGrid()
    {
        //allocate space in the array for nodes
        aStarNodes = new AStarNode[gridSizeX, gridSizeY];

        //create grid of nodes
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                aStarNodes[x, y] = new AStarNode(new Vector2Int(x, y));

                Vector3 worldPosition = ConvertGridPositionToWorldPosition(aStarNodes[x, y]);

                //Check if node is obstacle
                Collider2D hitCollider2D = Physics2D.OverlapCircle(worldPosition, cellSize / 2.0f);

                if (hitCollider2D != null)
                {
                    //Ignore player car, they are not obstacles
                    if (hitCollider2D.transform.root.CompareTag("Player"))
                        continue;

                    if (hitCollider2D.CompareTag("CheckPoint"))
                        continue;

                    if (hitCollider2D.transform.root.CompareTag("AI"))
                        continue;

                    //Mark as obstacle
                    aStarNodes[x, y].isObstacle = true;
                }
            }
        }

        //Loop through grid again and populte neighbors
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                //Check north neighbor, if on the edge dont add
                if(y - 1 >= 0)
                {
                    if(!aStarNodes[x, y - 1].isObstacle)
                    {
                        aStarNodes[x, y].neighbors.Add(aStarNodes[x, y - 1]);
                    }
                }

                //Check south neighbor, if on the edge dont add
                if (y + 1 <= gridSizeY - 1)
                {
                    if (!aStarNodes[x, y + 1].isObstacle)
                    {
                        aStarNodes[x, y].neighbors.Add(aStarNodes[x, y + 1]);
                      
                    }
                }

                //Check east neighbor, if on the edge dont add
                if (x - 1 >= 0)
                {
                    if (!aStarNodes[x - 1, y].isObstacle)
                    {
                        aStarNodes[x, y].neighbors.Add(aStarNodes[x - 1, y]);
                    }
                }

                //Check west neighbor, if on the edge dont add
                if (x + 1 <= gridSizeX - 1)
                {
                    if (!aStarNodes[x + 1, y].isObstacle)
                    {
                        aStarNodes[x, y].neighbors.Add(aStarNodes[x + 1, y]);
                    }
                }
            }

        }
    }

    private void Reset()
    {
        nodesToCheck.Clear();
        nodesChecked.Clear();
        aiPath.Clear();

        for (int x = 0; x< gridSizeX; x++)
        {
            for (int y = 0; y< gridSizeY; y++)
            {
                aStarNodes[x,y].Reset();
            }
        }
    }

    public List<Vector3> FindPath(Vector3 destination)
    {
        if (aStarNodes == null)
            return null;

        Reset();

        //convert destination from world to grid position
        Vector2Int destinationGridPoint = ConvertWorldToGridPoint(destination);
        Vector2Int currentPositionGridPoint = ConvertWorldToGridPoint(transform.position);

        //set a debug position to show while developing
        destinationPositionDebug = destination;

        //start algorithm by calculating the costs for the first node
        startNode = GetNodeFromPoint(currentPositionGridPoint);

        //store start grid position to show while developing
        startPositionDebug = ConvertGridPositionToWorldPosition(startNode);

        //set current node as start node
        AStarNode currentNode = startNode;

        bool isDoneFindingPath = false;
        int pickedOrder = 1;

        //Loop while we are not done with path
        while (!isDoneFindingPath)
        {
            //remove current node from nodes to check
            nodesToCheck.Remove(currentNode);

            //set pick order
            currentNode.pickedOrder = pickedOrder;

            pickedOrder++;

            //add current node to checked list
            nodesChecked.Add(currentNode);

            //destination is found!!!
            if(currentNode.gridPosition == destinationGridPoint)
            {
                isDoneFindingPath = true;
                break;
            }


            //calculate cost for all nodes
            CalculateCostsForNodeAndNeighbors(currentNode, currentPositionGridPoint, destinationGridPoint);

            //check if neighbor nodes should be considered
            foreach (AStarNode neighbourNode in currentNode.neighbors)
            {
                //skip any node already checked
                if (nodesChecked.Contains(neighbourNode))
                    continue;

                //skip any node that is already on the list
                if (nodesToCheck.Contains(neighbourNode))
                    continue;

                //add node to list that we should check
                nodesToCheck.Add(neighbourNode);
            }

            //sort list so that items with lowest total cost will be picked to reach the goal
            nodesToCheck= nodesToCheck.OrderBy(x => x.fCostTotal).ThenBy(x => x.hCostDistanceFromGoal).ToList();

            //pick node with lowest cost as next node
            if(nodesToCheck.Count == 0)
            {
                Debug.LogWarning($"No nodes left in next nodes to check, we have no solution");
                return null;
            }
            else
            {
                currentNode = nodesToCheck[0]; 
            }

        }

        aiPath = CreatePathForAI(currentPositionGridPoint);

        return aiPath;

    }

    List<Vector3> CreatePathForAI(Vector2Int currentPositionGridPoint)
    {
        List<Vector3> resultAIPath = new List<Vector3>();
        List<AStarNode> aiPath = new List<AStarNode>();

        //reverse nodes to check as the last added node will be AI destination
        nodesChecked.Reverse();

        bool isPathCreated = false;

        AStarNode currentNode = nodesChecked[0];

        aiPath.Add(currentNode);

        int attempts = 0;

        while (!isPathCreated)
        {
            //Go backwards with lowest creation order
            currentNode.neighbors = currentNode.neighbors.OrderBy(x => x.pickedOrder).ToList();

            //pick neighbor with lowest cost if not already in the list
            foreach (AStarNode aStarNode in currentNode.neighbors)
            {
                if(!aiPath.Contains(aStarNode) && nodesChecked.Contains(aStarNode))
                {
                    aiPath.Add(aStarNode);
                    currentNode= aStarNode;

                    break;
                }
            }

            if(currentNode == startNode)
                isPathCreated = true;

            if (attempts > 1000)
            {
                Debug.LogWarning("CreatePathForAI() failed too many attempts");
                break;
            }
            attempts++;
        }

        foreach(AStarNode aStarNode in aiPath)
        {
            resultAIPath.Add(ConvertGridPositionToWorldPosition(aStarNode));
        }

        //flip result
        resultAIPath.Reverse();

        return resultAIPath;
    }


    void CalculateCostsForNodeAndNeighbors(AStarNode aStarNode, Vector2Int aiPosition, Vector2Int aiDestination)
    {
        aStarNode.CalculateCostsForNode(aiPosition, aiDestination);

        foreach (AStarNode neighbourNode in aStarNode.neighbors)
        {
            neighbourNode.CalculateCostsForNode(aiPosition, aiDestination);
        }
    }

    AStarNode GetNodeFromPoint(Vector2Int gridPoint)
    {
        if(gridPoint.x < 0)
            return null;

        if (gridPoint.x > gridSizeX - 1)
            return null;

        if (gridPoint.y < 0)
            return null;

        if (gridPoint.y > gridSizeY - 1)
            return null;

        return aStarNodes[gridPoint.x, gridPoint.y];
    }


    Vector2Int ConvertWorldToGridPoint(Vector2 position)
    {
        //calculate grid point
        Vector2Int gridPoint = new Vector2Int(Mathf.RoundToInt(position.x / cellSize + gridSizeX / 2.0f), Mathf.RoundToInt(position.y / cellSize + gridSizeY / 2.0f));
        return gridPoint;
    }

    Vector3 ConvertGridPositionToWorldPosition(AStarNode aStarNode)
    {
        return new Vector3(aStarNode.gridPosition.x * cellSize - (gridSizeX * cellSize) / 2.0f, 0 , aStarNode.gridPosition.y * cellSize - (gridSizeY * cellSize) / 2.0f);
    }

    void OnDrawGizmos()
    {
        if (aStarNodes == null)
            return;

        //draw grid
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                if (aStarNodes[x, y].isObstacle)
                    Gizmos.color = Color.red;
                else Gizmos.color = Color.green;

                Gizmos.DrawWireCube(ConvertGridPositionToWorldPosition(aStarNodes[x,y]), new Vector3(cellSize, cellSize, cellSize));
            }
        }

        //Draw nodes that we have checked
        foreach (AStarNode checkedNode in nodesChecked)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(ConvertGridPositionToWorldPosition(checkedNode), 0.1f);
        }

        //Draw nodes that we should check
        foreach (AStarNode toCheckNode in nodesToCheck)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(ConvertGridPositionToWorldPosition(toCheckNode), 0.1f);
        }

        Vector3 lastAIPoint = Vector3.zero;
        bool isFirstStep = true;

        Gizmos.color = Color.black;
        foreach (Vector2 point in aiPath)
        {
            if (!isFirstStep)
                Gizmos.DrawLine(lastAIPoint, point);

            lastAIPoint = point;

            isFirstStep = false;
        }
       

        //Draw start position
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(startPositionDebug, 0.1f);

        //Draw end position
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(destinationPositionDebug, 0.1f);
    }
}
