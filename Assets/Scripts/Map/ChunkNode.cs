using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node<T>
{
    public int x;
    public int y;

    public Edge<T>[] neighbours;
    /*Left = 0
    *Up=1
    * Right=2
    * Down=3
    */

    public Node(int x, int y)
    {
        this.x = x;
        this.y = y;

        neighbours = new Edge<T>[4];
    }

    public void AddNeighbour(Direction direction, Node<T> node)
    {
        neighbours[(int)direction] = new Edge<T>(node);
    }

    public Node<T> GetNeighbour(Direction direction)
    {
        return neighbours[(int)direction].node;
    }

    public void ChangeEdge(Direction dir, bool b)
    {
        neighbours[(int)dir].isOpen = b;
    }
}

public class Edge<T>
{
    public bool isOpen;

    public Node<T> node;

    public Edge(Node<T> n)
    {
        isOpen = false;
        node = n;
    }

}