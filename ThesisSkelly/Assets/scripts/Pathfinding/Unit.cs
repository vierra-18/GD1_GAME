using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    const float minPathUpdateTime = 0.2f;
    const float pathUpdateMoveThreshold = 0.5f;
    // public Transform target;
    // public float speed = 1f;
    // Vector3[] path;
    int targetIndex;

    public Transform target;
    public float speed = 20;
    public float turnSpeed = 3;
    public float turnDst = 5;
    public float stoppingDst = 10;
    bool reachedTarget;

    Path path;

    void Start()
    {
        StartCoroutine(UpdatePath());
    }


    IEnumerator UpdatePath()
    {
        if (Time.timeSinceLevelLoad < .3f)
        {
            yield return new WaitForSeconds(0.3f);
        }
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);

        float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
        Vector3 targetPosOld = target.position;

        while (true)
        {
            yield return new WaitForSeconds(minPathUpdateTime);
            if ((target.position - targetPosOld).sqrMagnitude > sqrMoveThreshold)
            {
                PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
                targetPosOld = target.position;
            }
        }
    }


    public void OnPathFound(Vector3[] waypoints, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = new Path(waypoints, transform.position, turnDst, stoppingDst);

            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }


    IEnumerator FollowPath()
    {

        bool followingPath = true;
        int pathIndex = 0;
        transform.LookAt(path.lookPoints[0]);

        float speedPercent = 1;

        while (followingPath)
        {
            Vector2 pos2D = new Vector2(transform.position.x, transform.position.z);
            while (path.turnBoundaries[pathIndex].HasCrossedLine(pos2D))
            {
                if (pathIndex == path.finishLineIndex)
                {
                    followingPath = false;
                    break;
                }
                else
                {
                    pathIndex++;
                }
            }

            if (followingPath)
            {

                if (pathIndex >= path.slowDownIndex && stoppingDst > 0)
                {
                    speedPercent = Mathf.Clamp01(path.turnBoundaries[path.finishLineIndex].DistanceFromPoint(pos2D) / stoppingDst);
                    if (speedPercent < 0.01f)
                    {
                        followingPath = false;
                    }
                }

                Quaternion targetRotation = Quaternion.LookRotation(path.lookPoints[pathIndex] - transform.position);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
                transform.Translate(Vector3.forward * Time.deltaTime * speed * speedPercent, Space.Self);
            }

            yield return null;

        }
    }

    //     IEnumerator FollowPath()
    // {
    //     // start from the first waypoint
    //     Vector3 currentWaypoint = path[0];

    //     while (true)
    //     {
    //         // advance to next waypoint
    //         if (transform.position == currentWaypoint)
    //         {
    //             targetIndex++;
    //             if (targetIndex >= path.Length)
    //             {
    //                 /* targetIndex = 0;
    //                  path = new Vector3[0];*/
    //                 yield break;
    //             }
    //             currentWaypoint = path[targetIndex];
    //         }

    //         transform.LookAt(currentWaypoint);
    //         transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed);
    //         yield return null;
    //     }

    // }

   public void OnDrawGizmos() {
		if (path != null) {
			path.DrawWithGizmos ();
		}
	}
}


//   public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
//     {
//         if (pathSuccessful)
//         {
//             path = newPath;
//             targetIndex = 0;
//             StopCoroutine("FollowPath");
//             StartCoroutine("FollowPath");
//         }
//     }

//     IEnumerator FollowPath()
//     {
//         // start from the first waypoint
//         Vector3 currentWaypoint = path[0];

//         while (true)
//         {
//             // advance to next waypoint
//             if (transform.position == currentWaypoint)
//             {
//                 targetIndex++;
//                 if (targetIndex >= path.Length)
//                 {
//                     /* targetIndex = 0;
//                      path = new Vector3[0];*/
//                     yield break;
//                 }
//                 currentWaypoint = path[targetIndex];
//             }

//             transform.LookAt(currentWaypoint);
//             transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed);
//             yield return null;
//         }

//     }

//   public void OnDrawGizmos()
//     {
//         if (path != null)
//         {
//             for (int i = targetIndex; i < path.Length; i++)
//             {
//                 Gizmos.color = Color.black;
//                 Gizmos.DrawCube(path[i], Vector3.one);

//                 if (i == targetIndex)
//                 {
//                     Gizmos.DrawLine(transform.position, path[i]);
//                 }
//                 else
//                 {
//                     Gizmos.DrawLine(path[i - 1], path[i]);
//                 }
//             }
//         }
//     }