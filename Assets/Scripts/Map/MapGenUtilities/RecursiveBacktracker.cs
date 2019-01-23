using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RecursiveBacktracker<T>
{
    Node<T>[,] nodes;
    int sizeX;
    int sizeY;
    Vector2i start;
    Vector2i end;

    /*
    public void Generate(int startX, int startY)
    {
        start = new Vector2i(startX, startY);
        Points = new List<Tuple<Cell, Direction>>();
        CarvePassage(startX, startY);
    }
    */

    private void ValidateDirections(Node<Room> node, List<Direction> directions)
    {
        List<Direction> invalidDirections = new List<Direction>();

        // Check for invalid moves
        for (int i = 0; i < directions.Count; i++)
        {
            switch (directions[i])
            {
                case Direction.Up:
                    if (node.Y == 0 || CellVisited(node.X, node.Y - 1))
                        invalidDirections.Add(Direction.Up);
                    break;
                case Direction.Left:
                    if (node.X == sizeX - 1 || CellVisited(node.X + 1, node.Y))
                        invalidDirections.Add(Direction.Left);
                    break;
                case Direction.Down:
                    if (node.Y == sizeY - 1 || CellVisited(node.X, node.Y + 1))
                        invalidDirections.Add(Direction.Down);
                    break;
                case Direction.Right:
                    if (node.X == 0 || CellVisited(node.X - 1, node.Y))
                        invalidDirections.Add(Direction.Right);
                    break;
            }
        }

        // Eliminating invalid moves
        foreach (var item in invalidDirections)
            directions.Remove(item);
    }

    public static List<Direction> GetAllDirections()
    {
        return new List<Direction>()
        {
            Direction.Up,
            Direction.Left,
            Direction.Down,
            Direction.Right,

        };
    }

    private bool CellVisited(int x, int y)
    {
        return nodes[y, x].Visited;
    }
}
