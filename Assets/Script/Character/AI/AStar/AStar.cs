using System.Collections.Generic;
using UnityEngine;

enum RayCastTarget
{
    Nothing,
    Player,
    Obstacle
}

public static class AStar
{
    static List<Vector3> raycastDirectionList = new List<Vector3>(){
       new Vector3(Mathf.Sin(Mathf.Deg2Rad* 45), 0, Mathf.Cos(Mathf.Deg2Rad* 45)), //45
       new Vector3(Mathf.Sin(Mathf.Deg2Rad* 90), 0, Mathf.Cos(Mathf.Deg2Rad* 90)), //90
       new Vector3(Mathf.Sin(Mathf.Deg2Rad* 135), 0, Mathf.Cos(Mathf.Deg2Rad* 135)), //135
       new Vector3(Mathf.Sin(Mathf.Deg2Rad* 180), 0, Mathf.Cos(Mathf.Deg2Rad* 180)), //180
       new Vector3(Mathf.Sin(Mathf.Deg2Rad* 225), 0, Mathf.Cos(Mathf.Deg2Rad* 225)), //225
       new Vector3(Mathf.Sin(Mathf.Deg2Rad* 270), 0, Mathf.Cos(Mathf.Deg2Rad* 270)), //270
       new Vector3(Mathf.Sin(Mathf.Deg2Rad* 315), 0, Mathf.Cos(Mathf.Deg2Rad* 315)), //315
       new Vector3(Mathf.Sin(Mathf.Deg2Rad* 360), 0, Mathf.Cos(Mathf.Deg2Rad* 360))  //360
    };

    private static RayCastTarget GetRayCastTarget(Vector3 rayStartPoint, Vector3 direction, float distance)
    {
        if (Physics.Raycast(rayStartPoint, direction, out RaycastHit hitTarget, distance))
        {
            if (hitTarget.transform.CompareTag("Player"))
            {
                return RayCastTarget.Player;
            }
            if (hitTarget.transform.CompareTag("Obstacle"))
            {
                return RayCastTarget.Obstacle;
            }

        }
        return RayCastTarget.Nothing;
    }
    private static Node createNodeFromRaycast(Vector3 raycastDirection, Node currentNode, float distance)
    {
        return new Node((
                    currentNode.currentPosition.Item1 + raycastDirection.x,
                    currentNode.currentPosition.Item2 + raycastDirection.z)
                    , currentNode)
        {
            G = currentNode.G + distance,
            DestinationPosition = currentNode.DestinationPosition
        };
    }

    private static bool calculateSTH(Node currentNode, Vector3 raycastDirection, float distance)
    {
        return true;
    }

    // TODO-0: finish calculateSTH
    // TODO-1: should not use isEscape flag; should we spilt this to two function (FindWayToPlayer\2, FindEscapeWay\2?)
    // TODO-2: change from (float, float) to vector3 in Node
    // TODO-3: FindDirection should not use list
    // TODO-4: openList, closeList can be hashmap
    // TODO-5: isFound?
    // TODO-6: checkDistance
    // TODO-7: Singleton
    public static Vector3 FindWay(Vector3 start, Vector3 destination, bool isEscape)
    {
        float checkDistance = 1;  // TODO: should we declare it here? What is this?
        bool isFound = false; // TODO:  Rename? isFoundPlayer? --> We might not need this field at all
        var openList = new List<Node> { }; // TODO:  Rename? Can we use hash and queue? 
        // var map = new Dictionary<string, string>(); map.Add("cat", "orange");
        var closeList = new List<Node> { }; // TODO:  Rename? Can we use hash?
        Node currentNode;
        Node startNode = new Node((start.x, start.z))
        {
            G = 0,
            DestinationPosition = (destination.x, destination.z)
        };

        Node destinationNode = null;

        openList.Add(startNode);

        do
        {
            openList.Sort();

            currentNode = openList[0];
            closeList.Add(currentNode);
            openList.RemoveAt(0);

            if (isFound)
            {
                break;
            }

            var adjacentList = new List<Node> { };


            // TODO:  what is 1/5 do here?
            checkDistance = currentNode.CalculateH() * 1 / 5;
            if (checkDistance < 1)
            {
                checkDistance = 1;
            }

            Vector3 rayStartPoint = currentNode.getCurrentPositionAsVector3();


            foreach (Vector3 rawRaycastDirection in raycastDirectionList)
            {
                Vector3 raycastDirection = rawRaycastDirection * checkDistance;

                var targetType = GetRayCastTarget(rayStartPoint, raycastDirection, checkDistance);

                if (targetType == RayCastTarget.Obstacle)
                {
                    continue;
                }
                else if (targetType == RayCastTarget.Player)
                {
                    if (isEscape)
                    {
                        continue;
                    }
                    destinationNode = createNodeFromRaycast(raycastDirection, currentNode, checkDistance);
                    isFound = true;
                    break;
                }
                else
                {
                    if (isEscape)
                    {
                        float currentX = currentNode.currentPosition.Item1;
                        float currentZ = currentNode.currentPosition.Item2;
                        // TODO: Move to another function
                        if ((currentX + raycastDirection.x - destination.x) * (destination.x - currentX) >= 0 &&
                            (currentZ + raycastDirection.z - destination.z) * (destination.z - currentZ) >= 0)
                        {
                            destinationNode = createNodeFromRaycast(raycastDirection, currentNode, checkDistance);
                            isFound = true;
                            break;
                        }
                    }

                    var nextNode = createNodeFromRaycast(raycastDirection, currentNode, checkDistance);
                    adjacentList.Add(nextNode);
                }
            }


            foreach (Node node in adjacentList)
            {
                // bigO = n, hash?
                if (closeList.Contains(node))
                {
                    continue;
                }
                // bigO = n, hash?
                else if (!openList.Contains(node))
                {
                    openList.Add(node);
                }
                else
                {
                    //TODO: make function
                    if (openList.Find(j => j.Equals(node)).Compare(openList.Find(j => j.Equals(node)), node) == 1)
                    {
                        openList.Remove(openList.Find(j => j.Equals(node)));
                        openList.Add(node);
                    }
                }
            }

        } while (openList.Count != 0); //BFS?

        return FindDirection(destinationNode);
    }

    public static Vector3 FindDirection(Node node)
    {

        var pathList = new List<(float, float)> { };
        while (node.parentNode != null)
        {
            pathList.Add(node.currentPosition);
            node = node.parentNode;
        }
        // return new Vector3(node.currentPosition.Item1, 0, node.currentPosition.Item2);
        pathList.Reverse();
        return new Vector3(pathList[0].Item1, 0, pathList[0].Item2);
    }
}
