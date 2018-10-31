using UnityEngine;
using System.Collections;

[System.Serializable]
public struct AABB
{
    public Vector2 center;
    public Vector2 halfSize;

    public AABB(Vector2 center, Vector2 halfSize)
    {
        this.center = center;
        this.halfSize = halfSize;
    }

    public bool Overlaps(AABB other)
    {
        if (Mathf.Abs(center.x - other.center.x) > halfSize.x + other.halfSize.x) return false;
        if (Mathf.Abs(center.y - other.center.y) > halfSize.y + other.halfSize.y) return false;
        return true;
    }

    public bool Overlaps(Vector2 otherCenter, Vector2 otherHalfSize)
    {
        if (Mathf.Abs(center.x - otherCenter.x) > halfSize.x + otherHalfSize.x) return false;
        if (Mathf.Abs(center.y - otherCenter.y) > halfSize.y + otherHalfSize.y) return false;
        return true;
    }

    public bool Overlaps(AABB other, out float overlapWidth, out float overlapHeight)
    {
        overlapWidth = overlapHeight = 0;
        if (Mathf.Abs(center.x - other.center.x) > halfSize.x + other.halfSize.x) return false;
        if (Mathf.Abs(center.y - other.center.y) > halfSize.y + other.halfSize.y) return false;
        overlapWidth = (other.halfSize.x + halfSize.x) - Mathf.Abs(center.x - other.center.x);
        overlapHeight = (other.halfSize.y + halfSize.y) - Mathf.Abs(center.y - other.center.y);
        return true;
    }

    public static bool Overlaps(AABB a, AABB b)
    {
        if (Mathf.Abs(a.center.x - b.center.x) > a.halfSize.x + b.halfSize.x) return false;
        if (Mathf.Abs(a.center.y - b.center.y) > a.halfSize.y + b.halfSize.y) return false;
        return true;
    }

    public static bool Overlaps(AABB a, Vector2 otherCenter, Vector2 otherHalfSize)
    {
        if (Mathf.Abs(a.center.x - otherCenter.x) > a.halfSize.x + otherHalfSize.x) return false;
        if (Mathf.Abs(a.center.y - otherCenter.y) > a.halfSize.y + otherHalfSize.y) return false;
        return true;
    }

    public static bool Overlaps(AABB a, AABB b, out float overlapWidth, out float overlapHeight)
    {
        overlapWidth = overlapHeight = 0;
        if (Mathf.Abs(a.center.x - b.center.x) > a.halfSize.x + b.halfSize.x) return false;
        if (Mathf.Abs(a.center.y - b.center.y) > a.halfSize.y + b.halfSize.y) return false;
        overlapWidth = (b.halfSize.x + a.halfSize.x) - Mathf.Abs(a.center.x - b.center.x);
        overlapHeight = (b.halfSize.y + a.halfSize.y) - Mathf.Abs(a.center.y - b.center.y);
        return true;
    }

    public static bool PointInside(AABB a, Vector2 p)
    {
        if (Mathf.Abs(a.center.x - p.x) > a.halfSize.x) return false;
        if (Mathf.Abs(a.center.y - p.y) > a.halfSize.y) return false;
        return true;
    }
}
