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

    // TODO: should not use isEscape flag; should we spilt this to two function (FindWayToPlayer\2, FindEscapeWay\2?)
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

        Node endNode = null;

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
            float xAxisRay;
            float zAxisRay;
            bool isHitObstacle;

            // TODO:  what is 1/5 do here?
            checkDistance = currentNode.CalculateH() * 1 / 5;
            if (checkDistance < 1)
            {
                checkDistance = 1;
            }

            Vector3 rayStartPoint = new Vector3(currentNode.currentPosition.Item1, 0,
                currentNode.currentPosition.Item2);

            for (int i = 0; i < 360; i += 45)
            {
                // TODO: make function? it is a bit hard to understand 
                xAxisRay = Mathf.Sin(Mathf.Deg2Rad * i) * checkDistance;
                zAxisRay = Mathf.Cos(Mathf.Deg2Rad * i) * checkDistance;
                isHitObstacle = false;

                var targetType = GetRayCastTarget(rayStartPoint, new Vector3(xAxisRay, 0, zAxisRay), checkDistance);
                if (targetType == RayCastTarget.Obstacle)
                {
                    isHitObstacle = true;
                    continue;
                }

                else if (targetType == RayCastTarget.Player)
                {
                    if (isEscape)
                    {
                        continue;
                    }
                    // TODO: Duplicate code
                    endNode = new Node((
                    currentNode.currentPosition.Item1 + xAxisRay,
                    currentNode.currentPosition.Item2 + zAxisRay)
                    , currentNode)
                    {
                        G = currentNode.G + checkDistance,
                        DestinationPosition = currentNode.DestinationPosition
                    };
                    isFound = true;
                    break;
                }


                // TODO: spilt function addToAdjacentList?
                if (!isHitObstacle)
                {
                    if (isEscape)
                    {
                        float currentX = currentNode.currentPosition.Item1;
                        float currentZ = currentNode.currentPosition.Item2;
                        // TODO: Move to another function
                        if ((currentX + xAxisRay - destination.x) * (destination.x - currentX) >= 0 &&
                            (currentZ + zAxisRay - destination.z) * (destination.z - currentZ) >= 0)
                        {
                            // TODO: Duplicate code
                            endNode = new Node((
                            currentNode.currentPosition.Item1 + xAxisRay,
                            currentNode.currentPosition.Item2 + zAxisRay)
                            , currentNode)
                            {
                                G = currentNode.G + checkDistance,
                                DestinationPosition = currentNode.DestinationPosition
                            };
                            isFound = true;
                            break;
                        }
                    }

                    // TODO: ?
                    adjacentList.Add(new Node((
                        currentNode.currentPosition.Item1 + xAxisRay,
                        currentNode.currentPosition.Item2 + zAxisRay)
                        , currentNode)
                    {
                        G = currentNode.G + checkDistance,
                        DestinationPosition = currentNode.DestinationPosition
                    });
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

        return FindPath(endNode);
    }
    // public static Vector3 FindWay(Vector3 start, Vector3 destination, bool isEscape) 
    // {
    //     float checkDistance = 1;  // TODO: should we declare it here? What is this?
    //     bool isFound = false; // TODO:  Rename? isFoundPlayer? --> We might not need this field at all
    //     var openList = new List<Node> { }; // TODO:  Rename? Can we use hash and queue?
    //     var closeList = new List<Node> { }; // TODO:  Rename? Can we use hash?
    //     Node currentNode;
    //     Node startNode = new Node((start.x, start.z))
    //     {
    //         G = 0,
    //         DestinationPosition = (destination.x, destination.z)
    //     };

    //     Node endNode = null;

    //     openList.Add(startNode);

    //     do
    //     {
    //         openList.Sort();

    //         currentNode = openList[0];
    //         closeList.Add(currentNode);
    //         openList.RemoveAt(0);

    //         if (isFound)
    //         {
    //             break;
    //         }

    //         var adjacentList = new List<Node> { };
    //         float xAxisRay;
    //         float zAxisRay;
    //         bool isHitObstacle;

    //         // TODO:  what is 1/5 do here?
    //         checkDistance = currentNode.CalculateH() * 1 / 5;
    //         if (checkDistance < 1)
    //         {
    //             checkDistance = 1;
    //         }

    //         Vector3 rayStartPoint = new Vector3(currentNode.currentPosition.Item1, 0,
    //             currentNode.currentPosition.Item2);

    //         for (int i = 0; i < 360; i += 45)
    //         {
    //             // TODO: make function? it is a bit hard to understand 
    //             xAxisRay = Mathf.Sin(Mathf.Deg2Rad * i) * checkDistance;
    //             zAxisRay = Mathf.Cos(Mathf.Deg2Rad * i) * checkDistance;
    //             isHitObstacle = false;

    //             // TODO: make function? return isHitObstacle, isHitPlayer instead?
    //             if (Physics.Raycast(rayStartPoint, new Vector3(xAxisRay, 0, zAxisRay), out RaycastHit hitTarget, checkDistance))
    //             {
    //                 // TODO: refactor? check tag function
    //                 if (hitTarget.transform.CompareTag("Obstacle"))
    //                 {
    //                     isHitObstacle = true;
    //                     continue;
    //                 }

    //                 // TODO: refactor? check tag function
    //                 if (hitTarget.transform.CompareTag("Player"))
    //                 {
    //                     if (isEscape)
    //                     {
    //                         continue;
    //                     }
    //                     // TODO: Duplicate code
    //                     endNode = new Node((
    //                     currentNode.currentPosition.Item1 + xAxisRay,
    //                     currentNode.currentPosition.Item2 + zAxisRay)
    //                     , currentNode)
    //                     {
    //                         G = currentNode.G + checkDistance,
    //                         DestinationPosition = currentNode.DestinationPosition
    //                     };
    //                     isFound = true;
    //                     break;
    //                 }

    //             }

    //             // TODO: spilt function addToAdjacentList?
    //             if (!isHitObstacle)
    //             {
    //                 if (isEscape)
    //                 {
    //                     float currentX = currentNode.currentPosition.Item1;
    //                     float currentZ = currentNode.currentPosition.Item2;
    //                     // TODO: Move to another function
    //                     if ((currentX + xAxisRay - destination.x) * (destination.x - currentX) >= 0 &&
    //                         (currentZ + zAxisRay - destination.z) * (destination.z - currentZ) >= 0)
    //                     {
    //                         // TODO: Duplicate code
    //                         endNode = new Node((
    //                         currentNode.currentPosition.Item1 + xAxisRay,
    //                         currentNode.currentPosition.Item2 + zAxisRay)
    //                         , currentNode)
    //                         {
    //                             G = currentNode.G + checkDistance,
    //                             DestinationPosition = currentNode.DestinationPosition
    //                         };
    //                         isFound = true;
    //                         break;
    //                     }
    //                 }

    //                 // TODO: ?
    //                 adjacentList.Add(new Node((
    //                     currentNode.currentPosition.Item1 + xAxisRay,
    //                     currentNode.currentPosition.Item2 + zAxisRay)
    //                     , currentNode)
    //                 {
    //                     G = currentNode.G + checkDistance,
    //                     DestinationPosition = currentNode.DestinationPosition
    //                 });
    //             }
    //         }


    //         foreach (Node node in adjacentList)
    //         {
    //             // bigO = n, hash?
    //             if (closeList.Contains(node))
    //             {
    //                 continue;
    //             }
    //             // bigO = n, hash?
    //             else if (!openList.Contains(node))
    //             {
    //                 openList.Add(node);
    //             }
    //             else
    //             {
    //                 //TODO: make function
    //                 if (openList.Find(j => j.Equals(node)).Compare(openList.Find(j => j.Equals(node)), node) == 1)
    //                 {
    //                     openList.Remove(openList.Find(j => j.Equals(node)));
    //                     openList.Add(node);
    //                 }
    //             }

    //         }

    //     } while (openList.Count != 0); //BFS?

    //     return FindPath(endNode);
    // }

    // TODO: should rename FindFirstPath\1 ?
    public static Vector3 FindPath(Node node)
    {
        var pathList = new List<(float, float)> { };
        while (node.parentNode != null)
        {
            pathList.Add(node.currentPosition);
            node = node.parentNode;
        }
        pathList.Reverse();
        return new Vector3(pathList[0].Item1, 0, pathList[0].Item2);
    }
}
