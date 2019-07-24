using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : CharacterBase
{
    public GameObject target;
    private Vector3 targetPath;

    private List<(float, float)> pathList;
    private Vector3 escapedPoint;
    private bool hasEscapedPoint;
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetFloat("MoveSpeed", moveSpeed);
        hitAreaScript = hitArea.GetComponent<HitAreaScript>();
		playerName = "enemy";
    }

    void Update()
    {
        CharacterBehavior();
    }

    private void FixedUpdate()
    {
        if (isAlreadyDead)
        {
            return;
        }

        if (Time.time < nextAttackTime)
        {
            if (!hasEscapedPoint)
            {
                escapedPoint = Escape();
                hasEscapedPoint = true;
            }
            pathList = AStar(escapedPoint, true);
        }
        else
        {
            hasEscapedPoint = false;
            pathList = AStar(target.transform.position, false);
        }

        targetPath = new Vector3(pathList[0].Item1 - transform.position.x, 0, pathList[0].Item2 - transform.position.z);


        Move(targetPath);
    }


    protected override void Move(Vector3 dir)
    {

        float distanceToTarget = (target.gameObject.transform.position - transform.position).magnitude;

        if (attackState)
        {
            return;
        }

        if (distanceToTarget <= 0.6)
        {

            dir *= -1;
        }


        if (distanceToTarget <= 0.75)
        {
            animator.SetBool("IsMove", false);
            if (Time.time > nextAttackTime)
            {
				dir *= -1;
				//transform.rotation = Quaternion.LookRotation(dir);
				Attack();
            }
            return;
        }

        animator.SetBool("IsMove", true);
        dir.Normalize();
        transform.Translate(dir * moveSpeed * Time.deltaTime, Space.World);
        transform.rotation = Quaternion.LookRotation(dir);
    }


    protected override void CheckObstacle()
    {
        Debug.Log("obstacle");
    }

    private Vector3 Escape()
    {
        var corners = new List<Vector3> { };
        Vector3 size = GameObject.Find("Plane").GetComponent<Renderer>().bounds.size - (2 * Vector3.one);

        for (int i = -1; i < 2; i += 2)
        {
            for (int j = -1; j < 2; j += 2)
            {
                corners.Add(new Vector3(size.x / 2 * i, 0f, size.z / 2 * j));
            }
        }

        float max = -1;
        int index = -1;
        for (int i = 0; i < corners.Count; i++)
        {
            float distance = (corners[i] - transform.position).magnitude;
            if (max < distance)
            {
                max = distance;
                index = i;
            }

        }


        return corners[index];
    }

    public List<(float, float)> AStar(Vector3 destination, bool isEscape)
    {
        bool isFound = false;
        var openList = new List<Node> { };
        var closeList = new List<Node> { };
        Node currentNode;
        Node startNode = new Node((transform.position.x, transform.position.z))
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

        return FindPath(endNode);
    }

    public List<(float, float)> FindPath(Node node)
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

public class Node : IComparable<Node>
{

    public (float, float) currentPosition;
    public Node parentNode;

    public (float, float) DestinationPosition { get; set; }
    public float G { get; set; }


    public Node((float, float) currentPosition, Node parentNode = null)
    {

        this.currentPosition = currentPosition;
        this.parentNode = parentNode;
    }

    public int Compare(object x, object y)
    {
        Node node1 = (Node)x;
        Node node2 = (Node)y;
        if (node1.CalculateP() > node2.CalculateP())
            return 1;
        if (node1.CalculateP() < node2.CalculateP())
            return -1;
        return 0;

    }

    public override bool Equals(object obj)
    {
        Node node = (Node)obj;
        return node.currentPosition == currentPosition;
    }

    private float CalculateP()
    {
        return CalculateH(DestinationPosition) + G;
    }

    private float CalculateH((float, float) destination)
    {
        return Mathf.Abs(destination.Item1 - currentPosition.Item1) +
            Mathf.Abs(destination.Item2 - currentPosition.Item2);

    }

    public override string ToString()
    {
        if (parentNode == null)
        {
            return "postion = " + currentPosition;
        }
        return "postion = " + currentPosition + " Parent Postion = " + parentNode.currentPosition + " G = " + G + " H = " + CalculateH(DestinationPosition) + " P = " + CalculateP();
    }

    public int CompareTo(Node other)
    {
        return Compare(this, other);
    }
}



