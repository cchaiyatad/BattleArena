using System;
using UnityEngine;

public class Node : IComparable<Node>
{
    // TODO: Can we use vector3? because y will always be the same
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
        return CalculateH() + G;
    }

    public float CalculateH()
    {
        return Math.Abs(DestinationPosition.Item1 - currentPosition.Item1) +
            Math.Abs(DestinationPosition.Item2 - currentPosition.Item2);
    }

    public override string ToString()
    {
        if (parentNode == null)
        {
            return "postion = " + currentPosition;
        }
        return "postion = " + currentPosition + " Parent Postion = " + parentNode.currentPosition + " G = " + G + " H = " + CalculateH() + " P = " + CalculateP();
    }
    public Vector3 getCurrentPositionAsVector3()
    {
        return new Vector3(currentPosition.Item1,
                0,
                currentPosition.Item2);
    }

    public int CompareTo(Node other)
    {
        return Compare(this, other);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}