using System;
using System.Collections.Generic;
using UnityEngine;

public class AStarNode
{
    //position on the grid
    public Vector2Int gridPosition;

    //list of node neighbors
    public List<AStarNode> neighbors = new List<AStarNode>();

    //is node obstacle
    public bool isObstacle = false;

    //distance from start point to node
    public int gCostDistanceFromStart = 0;

    //distance from node to goal
    public int hCostDistanceFromGoal = 0;

    //total cost of movemment to grid position
    public int fCostTotal = 0;

    //the order which it was picked
    public int pickedOrder = 0;

    //state to check if cost has been calculated
    bool isCostCalculated = false;

    public AStarNode(Vector2Int gridPosition_)
    {
        gridPosition = gridPosition_;
    }

    public void CalculateCostsForNode(Vector2Int aiPosition, Vector2Int aiDestination)
    {
        //if cost is already calculated no need to do again
        if (isCostCalculated)
            return;

        gCostDistanceFromStart = Mathf.Abs(gridPosition.x - aiPosition.x) + Mathf.Abs(gridPosition.y - aiPosition.y);

        hCostDistanceFromGoal = Mathf.Abs(gridPosition.x - aiDestination.x) + Mathf.Abs(gridPosition.y - aiDestination.y);

        fCostTotal = gCostDistanceFromStart + hCostDistanceFromGoal;

        isCostCalculated = true;
    }

    public void Reset()
    {
        isCostCalculated=false;
        pickedOrder = 0;
        gCostDistanceFromStart=0;
        hCostDistanceFromGoal=0;
        fCostTotal=0;
    }
}

