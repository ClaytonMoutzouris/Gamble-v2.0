using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkNode
{
    public int x;
    public int y;

    public ChunkEdge[] neighbours;
    /*Left = 0
    *Up=1
    * Right=2
    * Down=3
    */

    public ChunkNode(int x, int y)
    {
        this.x = x;
        this.y = y;

        neighbours = new ChunkEdge[4];
    }

    public void AddNeighbour(Direction direction, ChunkNode node)
    {
        neighbours[(int)direction] = new ChunkEdge(node);
    }

    public ChunkNode GetNeighbour(Direction direction)
    {
        return neighbours[(int)direction].node;
    }

    public void ChangeEdge(Direction dir, bool b)
    {
        neighbours[(int)dir].isOpen = b;
    }
}

public class ChunkEdge
{
    public bool isOpen;

    public ChunkNode node;

    public ChunkEdge(ChunkNode n)
    {
        isOpen = false;
        node = n;
    }

}