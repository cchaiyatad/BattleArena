using System.Collections.Generic;
using UnityEngine;

public static class AStar
{
    public static Vector3 FindWay(Vector3 start, Vector3 destination, bool isEscape)
    {
        float checkDistance = 1;
        bool isFound = false;
        var openList = new List<Node> { };
        var closeList = new List<Node> { };
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
            checkDistance = currentNode.CalculateH((destination.x, destination.z)) * 1 / 5;
            if (checkDistance < 1)
            {
                checkDistance = 1;
            }
            Vector3 rayStartPoint = new Vector3(currentNode.currentPosition.Item1, 0,
                currentNode.currentPosition.Item2);

            for (int i = 0; i < 360; i += 45)
            {
                xAxisRay = Mathf.Sin(Mathf.Deg2Rad * i) * checkDistance;
                zAxisRay = Mathf.Cos(Mathf.Deg2Rad * i) * checkDistance;
                isHitObstacle = false;

                if (Physics.Raycast(rayStartPoint, new Vector3(xAxisRay, 0, zAxisRay), out RaycastHit hitTarget, checkDistance))
                {
                    if (hitTarget.transform.CompareTag("Obstacle"))
                    {
                        isHitObstacle = true;
                        continue;
                    }
                    if (hitTarget.transform.CompareTag("Player"))
                    {
                        if (isEscape)
                        {
                            continue;
                        }
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

                if (!isHitObstacle)
                {
                    if (isEscape)
                    {
                        float currentX = currentNode.currentPosition.Item1;
                        float currentZ = currentNode.currentPosition.Item2;
                        if ((currentX + xAxisRay - destination.x) * (destination.x - currentX) >= 0 &&
                            (currentZ + zAxisRay - destination.z) * (destination.z - currentZ) >= 0)
                        {
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
                if (closeList.Contains(node))
                {
                    continue;
                }
                else if (!openList.Contains(node))
                {
                    openList.Add(node);
                }
                else
                {
                    if (openList.Find(j => j.Equals(node)).Compare(openList.Find(j => j.Equals(node)), node) == 1)
                    {
                        openList.Remove(openList.Find(j => j.Equals(node)));
                        openList.Add(node);
                    }
                }

            }

        } while (openList.Count != 0);

        return FindPath(endNode);
    }

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
