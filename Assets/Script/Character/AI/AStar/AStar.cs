using System.Collections.Generic;
using UnityEngine;

enum RayCastTarget
{
    Nothing,
    Player,
    Obstacle
}
// TODO-0: Rename calculateSTH


// TODO-1: should not use isEscape flag; should we spilt this to two function (FindWayToPlayer\2, FindEscapeWay\2?)
// TODO-5: isFound?
// TODO-6: checkDistance

public class AStar
{
    private static AStar instance;

    private AStar()
    {
    }

    public static AStar getInstance()
    {
        if (instance == null)
        {
            instance = new AStar();
        }
        return instance;
    }

    private List<Vector3> raycastDirectionList = new List<Vector3>(){
       new Vector3(Mathf.Sin(Mathf.Deg2Rad* 45), 0, Mathf.Cos(Mathf.Deg2Rad* 45)), //45
       new Vector3(Mathf.Sin(Mathf.Deg2Rad* 90), 0, Mathf.Cos(Mathf.Deg2Rad* 90)), //90
       new Vector3(Mathf.Sin(Mathf.Deg2Rad* 135), 0, Mathf.Cos(Mathf.Deg2Rad* 135)), //135
       new Vector3(Mathf.Sin(Mathf.Deg2Rad* 180), 0, Mathf.Cos(Mathf.Deg2Rad* 180)), //180
       new Vector3(Mathf.Sin(Mathf.Deg2Rad* 225), 0, Mathf.Cos(Mathf.Deg2Rad* 225)), //225
       new Vector3(Mathf.Sin(Mathf.Deg2Rad* 270), 0, Mathf.Cos(Mathf.Deg2Rad* 270)), //270
       new Vector3(Mathf.Sin(Mathf.Deg2Rad* 315), 0, Mathf.Cos(Mathf.Deg2Rad* 315)), //315
       new Vector3(Mathf.Sin(Mathf.Deg2Rad* 360), 0, Mathf.Cos(Mathf.Deg2Rad* 360))  //360
    };

    private RayCastTarget GetRayCastTarget(Vector3 rayStartPoint, Vector3 direction, float distance)
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
    private Node createNodeFromRaycast(Vector3 raycastDirection, Node currentNode, float distance)
    {
        return new Node(currentNode.currentPosition + raycastDirection, currentNode)
        {
            G = currentNode.G + distance,
            DestinationPosition = currentNode.DestinationPosition
        };
    }

    private bool calculateSTH(Node currentNode, Vector3 raycastDirection, Vector3 destination)
    {
        float currentX = currentNode.currentPosition.x;
        float currentZ = currentNode.currentPosition.z;
        return ((currentX + raycastDirection.x - destination.x) * (destination.x - currentX) >= 0 &&
                            (currentZ + raycastDirection.z - destination.z) * (destination.z - currentZ) >= 0);
    }

    private bool isNotInFringe(Node node, Dictionary<Node, bool> willVisitNode)
    {
        return !willVisitNode.ContainsKey(node);
    }

    private bool isAlreadyVisited(Node node, Dictionary<Node, bool> visitedNode)
    {
        return visitedNode.ContainsKey(node);
    }
    private void AddToFringe(Node node, List<Node> fringe, Dictionary<Node, bool> willVisitNode)
    {
        fringe.Add(node);
        willVisitNode.Add(node, true);
    }

    public Vector3 FindWay(Vector3 start, Vector3 destination, bool isEscape)
    {
        float checkDistance = 1;  // TODO: should we declare it here? What is this?
        bool isFound = false; // TODO:  Rename? isFoundPlayer? --> We might not need this field at all

        var fringe = new List<Node> { };
        var willVisitNode = new Dictionary<Node, bool>();
        var visitedNode = new Dictionary<Node, bool>();

        Node currentNode;
        Node startNode = new Node(start)
        {
            G = 0,
            DestinationPosition = destination
        };

        Node destinationNode = null;

        fringe.Add(startNode);

        do
        {
            fringe.Sort();

            currentNode = fringe[0];
            visitedNode.Add(currentNode, true);
            fringe.RemoveAt(0);

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

            Vector3 rayStartPoint = currentNode.currentPosition;


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
                        if (calculateSTH(currentNode, raycastDirection, destination))
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
                if (isAlreadyVisited(node, visitedNode))
                {
                    continue;
                }
                else if (isNotInFringe(node, willVisitNode))
                {
                    AddToFringe(node, fringe, willVisitNode);
                }
            }

        } while (fringe.Count != 0);


        if (destinationNode == null)
        {
            return start; // ??
        }

        return FindDirection(destinationNode);
    }

    public Vector3 FindDirection(Node node)
    {
        while (node.parentNode.parentNode != null)
        {
            node = node.parentNode;
        }
        return node.currentPosition;
    }
}
