using System.Collections.Generic;
using UnityEngine;

public static class AStar
{
    public static List<(float, float)> FindWay(Vector3 start,Vector3 destination,bool isEscape)
    {
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
            for (int i = 0; i < 360; i += 45)
            {
                float xAxisRay = Mathf.Sin(Mathf.Deg2Rad * i);
                float zAxisRay = Mathf.Cos(Mathf.Deg2Rad * i);

                if (Physics.Raycast(
                    new Vector3(currentNode.currentPosition.Item1, 0, currentNode.currentPosition.Item2)
                    , new Vector3(xAxisRay, 0, zAxisRay)
                    , out RaycastHit hit, 1f))
                {
                    if (hit.transform.CompareTag("Obstacle"))
                    {
                        continue;
                    }
                    if (hit.transform.CompareTag("Player"))
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
                            G = currentNode.G + 1,
                            DestinationPosition = currentNode.DestinationPosition
                        };
                        isFound = true;
                        break;
                    }

                }
                else
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
                                G = currentNode.G + 1,
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
                        G = currentNode.G + 1,
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

        return AStar.FindPath(endNode);
    }

    public static List<(float, float)> FindPath(Node node)
    {

        var pathList = new List<(float, float)> { };
        while (node.parentNode != null)
        {
            pathList.Add(node.currentPosition);
            node = node.parentNode;
        }
        pathList.Reverse();
        return pathList;
    }
}
