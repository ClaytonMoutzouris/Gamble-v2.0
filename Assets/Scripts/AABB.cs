using UnityEngine;
using System.Collections;

[System.Serializable]
public struct AABB
{

    public Vector2 scale;
    public Vector2 center;
    private Vector2 halfSize;

    public Vector2 HalfSize
    {
        set { halfSize = value; }
        get { return new Vector2(halfSize.x * scale.x, halfSize.y * scale.y); }
    }

    public float HalfSizeX
    {
        set { halfSize.x = value; }
        get { return halfSize.x * scale.x; }
    }

    public float HalfSizeY
    {
        set { halfSize.y = value; }
        get { return halfSize.y * scale.y; }
    }

    public AABB(Vector2 center, Vector2 halfSize)
    {
        scale = Vector2.one;
        this.center = center;
        this.halfSize = halfSize;
    }

    public Vector2 Max()
    {
        return center + HalfSize;
    }

    public Vector2 Min()
    {
        return center - HalfSize;
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

    public static bool OverlapsUnsigned(AABB a, AABB b, out float overlapWidth, out float overlapHeight)
    {
        overlapWidth = overlapHeight = 0;
        if (a.HalfSizeX == 0.0f || a.HalfSizeY == 0.0f || b.HalfSizeX == 0.0f || b.HalfSizeY == 0.0f
            || Mathf.Abs(a.center.x - b.center.x) > a.HalfSizeX + b.HalfSizeX
            || Mathf.Abs(a.center.y - b.center.y) > a.HalfSizeY + b.HalfSizeY)
            return false;
        overlapWidth = (b.HalfSizeX + a.HalfSizeX) - Mathf.Abs(a.center.x - b.center.x);
        overlapHeight = (b.HalfSizeY + a.HalfSizeY) - Mathf.Abs(a.center.y - b.center.y);
        return true;
    }

    public bool OverlapsUnsigned(AABB other, out float overlapWidth, out float overlapHeight)
    {
        overlapWidth = overlapHeight = 0;
        if (HalfSizeX == 0.0f || HalfSizeY == 0.0f || other.HalfSizeX == 0.0f || other.HalfSizeY == 0.0f
            || Mathf.Abs(center.x - other.center.x) > HalfSizeX + other.HalfSizeX
            || Mathf.Abs(center.y - other.center.y) > HalfSizeY + other.HalfSizeY)
            return false;
        overlapWidth = (other.HalfSizeX + HalfSizeX) - Mathf.Abs(center.x - other.center.x);
        overlapHeight = (other.HalfSizeY + HalfSizeY) - Mathf.Abs(center.y - other.center.y);
        return true;
    }

    public bool OverlapsSigned(AABB other, out float overlapWidth, out float overlapHeight)
    {
        overlapWidth = overlapHeight = 0;

        if (HalfSizeX == 0.0f || HalfSizeY == 0.0f || other.HalfSizeX == 0.0f || other.HalfSizeY == 0.0f
            || Mathf.Abs(center.x - other.center.x) > HalfSizeX + other.HalfSizeX
            || Mathf.Abs(center.y - other.center.y) > HalfSizeY + other.HalfSizeY) return false;

        overlapWidth = Mathf.Sign(center.x - other.center.x) * ((other.HalfSizeX + HalfSizeX) - Mathf.Abs(center.x - other.center.x));
        overlapHeight = Mathf.Sign(center.y - other.center.y) * ((other.HalfSizeY + HalfSizeY) - Mathf.Abs(center.y - other.center.y));
        return true;
    }

    public bool OverlapsSigned(AABB other, out Vector2 overlap)
    {
        overlap = Vector2.zero;

        if (HalfSizeX == 0.0f || HalfSizeY == 0.0f || other.HalfSizeX == 0.0f || other.HalfSizeY == 0.0f
            || Mathf.Abs(center.x - other.center.x) > HalfSizeX + other.HalfSizeX
            || Mathf.Abs(center.y - other.center.y) > HalfSizeY + other.HalfSizeY) return false;

        overlap = new Vector2(Mathf.Sign(center.x - other.center.x) * ((other.HalfSizeX + HalfSizeX) - Mathf.Abs(center.x - other.center.x)),
            Mathf.Sign(center.y - other.center.y) * ((other.HalfSizeY + HalfSizeY) - Mathf.Abs(center.y - other.center.y)));

        return true;
    }

    public static bool PointInside(AABB a, Vector2 p)
    {
        if (Mathf.Abs(a.center.x - p.x) > a.halfSize.x) return false;
        if (Mathf.Abs(a.center.y - p.y) > a.halfSize.y) return false;
        return true;
    }
}
