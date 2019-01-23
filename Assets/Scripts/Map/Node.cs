using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node<T>
{
    private int x;
    private int y;
    public int visited_count = 0;
    public bool Visited = false;
    public bool[] walls = new bool[4] { false, false, false, false };
    public int position_in_iteration;
    public bool isdeadend = false;

    public int X
    {
        get
        {
            return x;
        }

        set
        {
            x = value;
        }
    }

    public int Y
    {
        get
        {
            return y;
        }

        set
        {
            y = value;
        }
    }

    /*Left = 0
*Up=1
* Right=2
* Down=3
*/

    public Node(int x, int y)
    {
        this.X = x;
        this.Y = y;

    }
}