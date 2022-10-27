using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System;

public class Pathfinding : MonoBehaviour
{
	PathRequestManager requestManager;
	Grid_ grid;
	public static int n;

	void Awake()
	{
		requestManager = GetComponent<PathRequestManager>();
		grid = GetComponent<Grid_>();



	}

	public void StartFindPath(Vector3 startPos, Vector3 targetPos)
	{
		StartCoroutine(FindPath(startPos, targetPos));
	}

	IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
	{
		// waypoints for the pathfindging to follow
		Vector3[] waypoints = new Vector3[0];
		bool pathSuccess = false;

		Node startNode = grid.NodeFromWorldPoint(startPos);
		Node targetNode = grid.NodeFromWorldPoint(targetPos);

		// check if positions of start and target nodes are walkable or not
		if (startNode.walkable && targetNode.walkable)
		{
			Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
			HashSet<Node> closedSet = new HashSet<Node>();
			openSet.Add(startNode);

			while (openSet.Count > 0)
			{
				Node node = openSet.RemoveFirst();
				closedSet.Add(node);

				// once target node is found break out of loop
				if (node == targetNode)
				{
					// DEBUG FCOST THINGS
					// UnityEngine.Debug.Log("LogFile Distance: " + n);
					// UnityEngine.Debug.Log("TargetNode fCost:" + targetNode.fCost);
				
					pathSuccess = true;
					break;
				}

				foreach (Node neighbour in grid.GetNeighbours(node))
				{
					if (!neighbour.walkable || closedSet.Contains(neighbour))
					{
						continue;
					}

					int newCostToNeighbour = node.gCost + GetDistance(node, neighbour);
					n = node.gCost + GetDistance(node, neighbour);
					if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
					{
						neighbour.gCost = newCostToNeighbour;
						neighbour.hCost = GetDistance(neighbour, targetNode);
						neighbour.parent = node;

						if (!openSet.Contains(neighbour))
							openSet.Add(neighbour);
						else
							openSet.UpdateItem(neighbour);
					}
				}
			}
		}

		yield return null;

		// set path as the waypoints
		if (pathSuccess)
		{
			waypoints = RetracePath(startNode, targetNode);
			pathSuccess = waypoints.Length > 0;
		}

		// pass to path request manager through finished processing path method
		requestManager.FinishedProcessingPath(waypoints, pathSuccess);
	}

	Vector3[] RetracePath(Node startNode, Node endNode)
	{
		List<Node> path = new List<Node>();
		Node currentNode = endNode;

		while (currentNode != startNode)
		{
			path.Add(currentNode);
			currentNode = currentNode.parent;
		}

		Vector3[] waypoints = SimplifyPath(path);
		Array.Reverse(waypoints);
		return waypoints;

	}

	// simplifies the path of nodes so that it only takes waypoints needed to turn
	Vector3[] SimplifyPath(List<Node> path)
	{
		List<Vector3> waypoints = new List<Vector3>();

		// stores direction of last nodes
		Vector2 directionOld = Vector2.zero;

		for (int i = 1; i < path.Count; i++)
		{
			/* // directions of the nodes in the grid
			 Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);*/

			// add node to list of waypoints to change direction
			/* if (directionNew != directionOld)
			 {*/
			waypoints.Add(path[i].worldPosition);
			/*     }*/
			/*    directionOld = directionNew;*/
		}

		return waypoints.ToArray();
	}

	public static int GetDistance(Node nodeA, Node nodeB)
	{
		int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
		int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

		if (dstX > dstY)
		{
			
			return 14 * dstY + 10 * (dstX - dstY);
			
		}
		else
		{
			
			return 14 * dstX + 10 * (dstY - dstX);
		}

	}
	

}
