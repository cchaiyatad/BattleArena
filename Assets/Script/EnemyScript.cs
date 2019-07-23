using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : CharacterBase
{
    public GameObject target;
    private Vector3 targetPath;
    private int newVector;
    private bool isEscaping;

    private bool forwardHit;
    private bool sideHit;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetFloat("MoveSpeed", moveSpeed);
        hitAreaScript = hitArea.GetComponent<HitAreaScript>();
        AStar();
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
        targetPath = target.gameObject.transform.position - transform.position;
        if (Time.time < nextAttackTime && !attackState)
        {
            targetPath *= -1;
        }

        Move(targetPath);
    }


    protected override void Move(Vector3 dir)
    {

        if (attackState)
        {
            return;
        }

        if (dir.magnitude <= 0.6)
        {
            dir *= -1;
        }

        else if (dir.magnitude <= 0.75)
        {
            animator.SetBool("IsMove", false);
            if (Time.time > nextAttackTime)
            {
                Attack();
            }
            return;
        }
        CheckObstacle();


        animator.SetBool("IsMove", true);
        dir.Normalize();
        transform.Translate(dir * moveSpeed * Time.deltaTime, Space.World);
        transform.rotation = Quaternion.LookRotation(dir);
    }

    protected override void AttackRotate(Vector3 dir)
    {
        Debug.Log("rotate");

    }

    protected override void CheckObstacle()
    {


        if (Physics.Raycast(transform.position, targetPath, out RaycastHit hit))
        {
            //isHitObstacle = (transform.position - hit.transform.position).magnitude < 0.8
            if (hit.transform.CompareTag("Obstacle"))
            {
                isHitObstacle = true;
            }

        }


        if (isHitObstacle)
        {
            //AStar();
            isHitObstacle = false;

        }


    }

    public void AStar()
    {
        bool isFound = false;
        var openList = new List<Node> { };
        var closeList = new List<Node> { };

        (float, float) startPosition = (transform.position.x, transform.position.z);
        (float, float) destinationPosition = (target.transform.position.x, target.transform.position.z);
        Node currentNode;
        Node startNode = new Node(startPosition)
        {
            G = 0,
            DestinationPosition = destinationPosition
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
                if (Physics.Raycast(
                    new Vector3(currentNode.currentPosition.Item1, 0, currentNode.currentPosition.Item2)
                    , new Vector3(Mathf.Sin(Mathf.Deg2Rad * i), 0, Mathf.Cos(Mathf.Deg2Rad * i))
                    , out RaycastHit hit, 1f))
                {
                    if (hit.transform.CompareTag("Obstacle"))
                    {
                        continue;
                    }
                    if (hit.transform.CompareTag("Player")) {
                        endNode = new Node((
                        currentNode.currentPosition.Item1 + Mathf.Sin(Mathf.Deg2Rad * i),
                        currentNode.currentPosition.Item2 + Mathf.Cos(Mathf.Deg2Rad * i))
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
                    adjacentList.Add(new Node((
                        currentNode.currentPosition.Item1 + Mathf.Sin(Mathf.Deg2Rad * i),
                        currentNode.currentPosition.Item2 + Mathf.Cos(Mathf.Deg2Rad * i))
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
                else {
                    if (openList.Find(x => x.Equals(node)).Compare(openList.Find(x => x.Equals(node)), node) == 1)
                    {
                        openList.Remove(openList.Find(x => x.Equals(node)));
                        openList.Add(node);
                    }
                }

            }

            

        } while (openList.Count != 0);


        var temp = FindPath(endNode);
        foreach ((float, float) i in temp)
        {
            print(i);
        }
    }

    public List<(float, float)> FindPath(Node node)
    {
        
        var pathList = new List<(float, float)> { };
        while(node.parentNode != null)
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
        if(parentNode == null)
        {
            return "postion = " + currentPosition;
        }
        return "postion = " + currentPosition +  " Parent Postion = " + parentNode.currentPosition + " G = " + G + " H = " + CalculateH(DestinationPosition) + " P = " + CalculateP();
    }

    public int CompareTo(Node other)
    {
        return Compare(this, other);
    }
}



