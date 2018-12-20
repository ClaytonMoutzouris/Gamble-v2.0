using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public struct CustomAABB
{
    
    public Vector3 center;
    public Vector3 halfSize;
    public Vector3 offset;
    public Vector3 baseOffset;
    public Vector3 baseHalfSize;
    public Vector3 scale;
    [HideInInspector]
    public AABBType type;
    [HideInInspector]
    public float rotation;
    [HideInInspector]
    public bool isCircle;
    public bool flipped;


    public void Copy(CustomAABB other)
    {
        center = other.center;
        halfSize = other.halfSize;
        offset = other.offset;
        baseOffset = other.baseOffset;
        baseHalfSize = other.baseHalfSize;
        scale = other.scale;
        type = other.type;
        rotation = other.rotation;
        isCircle = other.isCircle;
        flipped = other.flipped;
    }

    public void SetAngle(float angle)
    {
        RotateBy(-Rotation + angle);
        Rotation = angle;
    }

    public void RotateBy(float angle)
    {
        float s = Mathf.Sin(angle);
        float c = Mathf.Cos(angle);

        float xnew = offset.x * c - offset.y * s;
        float ynew = offset.x * s + offset.y * c;

        Rotation += angle;

        if ((Mathf.Abs(Rotation) < Mathf.Deg2Rad * 135.0f && Mathf.Abs(Rotation) > Mathf.Deg2Rad * 45.0f)
            || (Mathf.Abs(Rotation) < Mathf.Deg2Rad * 315.0f && Mathf.Abs(Rotation) > Mathf.Deg2Rad * 225.0f))
        {
            if (!flipped)
            {
                flipped = true;
                float tmp = halfSize.x;
                halfSize.x = halfSize.y;
                halfSize.y = tmp;
            }
        }
        else if (flipped)
        {
            flipped = false;
            float tmp = halfSize.x;
            halfSize.x = halfSize.y;
            halfSize.y = tmp;
        }

        Offset = new Vector3(xnew, ynew);
    }

    public float Rotation
    {
        set
        {
            if (value == rotation)
                return;

            float s = Mathf.Sin(value * Mathf.Deg2Rad);
            float c = Mathf.Cos(value * Mathf.Deg2Rad);
            float offsetX = baseOffset.x * scale.x;
            float offsetY = baseOffset.y * scale.y;

            offset.x = offsetX * c - offsetY * s;
            offset.y = offsetX * s + offsetY * c;
        }
        get
        {
            return rotation;
        }
    }
    public Vector3 HalfSize
    {
        set
        {
            baseHalfSize = value;
            halfSize = new Vector3(value.x * Mathf.Abs(scale.x), value.y * Mathf.Abs(scale.y), value.z * Mathf.Abs(scale.z));
        }
        get
        {
            return halfSize;
        }
    }

    public float HalfSizeX
    {
        set
        {
            baseHalfSize.x = value;
            halfSize.x = value * Mathf.Abs(scale.x);
        }
        get
        {
            return halfSize.x;
        }
    }

    public float HalfSizeY
    {
        set
        {
            baseHalfSize.y = value;
            halfSize.y = value * Mathf.Abs(scale.y);
        }
        get
        {
            return halfSize.y;
        }
    }

    public float HalfSizeZ
    {
        set
        {
            baseHalfSize.z = value;
            halfSize.z = value * Mathf.Abs(scale.z);
        }
        get
        {
            return halfSize.z;
        }
    }

    public Vector3 Center
    {
        get { return center + offset; }
        set { center = value; }
    }

    public float CenterX
    {
        get { return center.x + offset.x; }
        set { center.x = value; }
    }

    public float CenterY
    {
        get { return center.y + offset.y; }
        set { center.y = value; }
    }

    public float CenterZ
    {
        get { return center.z + offset.z; }
        set { center.z = value; }
    }

    public Vector3 Scale
    {
        set
        {
            scale = value;
            halfSize = new Vector3(baseHalfSize.x * Mathf.Abs(value.x), baseHalfSize.y * Mathf.Abs(value.y), baseHalfSize.z * Mathf.Abs(value.z));
            offset = new Vector3(baseOffset.x * value.x, baseOffset.y * value.y, baseOffset.z * value.z);
            Rotation = rotation;
        }
        get
        {
            return scale;
        }
    }

    public float ScaleX
    {
        set
        {
            scale.x = value;
            halfSize.x = baseHalfSize.x * Mathf.Abs(value);
            offset.x = baseOffset.x * value;
            Rotation = rotation;
        }
        get { return scale.x; }
    }

    public float ScaleY
    {
        set
        {
            scale.y = value;
            halfSize.y = baseHalfSize.y * Mathf.Abs(value);
            offset.y = baseOffset.y * value;
            Rotation = rotation;
        }
        get { return scale.y; }
    }

    public float ScaleZ
    {
        set
        {
            scale.z = value;
            halfSize.z = baseHalfSize.z * Mathf.Abs(value);
            offset.z = baseOffset.z * value;
            Rotation = rotation;
        }
        get { return scale.z; }
    }

    public Vector3 Offset
    {
        set
        {
            baseOffset = value;
            offset = new Vector3(value.x * scale.x, value.y * scale.y, value.z * scale.z);
            Rotation = rotation;
        }
        get
        {
            return offset;
        }
    }

    public float OffsetX
    {
        set
        {
            baseOffset.x = value;
            offset.x = value * scale.x;
            Rotation = rotation;
        }
        get
        {
            return offset.x;
        }
    }

    public float OffsetY
    {
        set
        {
            baseOffset.y = value;
            offset.y = value * scale.y;
            Rotation = rotation;
        }
        get
        {
            return offset.y;
        }
    }

    public float OffsetZ
    {
        set
        {
            baseOffset.z = value;
            offset.z = value * scale.z;
            Rotation = rotation;
        }
        get
        {
            return offset.z;
        }
    }

    public Vector3 Max()
    {
        return Center + HalfSize;
    }

    public Vector3 Min()
    {
        return Center - HalfSize;
    }


    public void SetFromBounds(Vector3 bottomNearLeft, Vector3 topFarRight)
    {
        HalfSize = (topFarRight - bottomNearLeft) * 0.5f;
        Center = bottomNearLeft + halfSize;
    }

    public bool Overlaps(CustomAABB other)
    {
        if (HalfSizeX == 0.0f || HalfSizeY == 0.0f
            || other.HalfSizeX == 0.0f || other.HalfSizeY == 0.0f
            || Mathf.Abs(CenterX - other.CenterX) > HalfSizeX + other.HalfSizeX
            || Mathf.Abs(CenterY - other.CenterY) > HalfSizeY + other.HalfSizeY
            || (HalfSizeZ > 0.0f && other.HalfSizeZ > 0.0f && Mathf.Abs(CenterZ - other.CenterZ) > HalfSizeZ + other.HalfSizeZ))
            return false;
        return true;
    }

    public bool Overlaps(Vector3 otherCenter, Vector3 otherHalfSize)
    {
        if (HalfSizeX == 0.0f || HalfSizeY == 0.0f
            || otherHalfSize.x == 0.0f || otherHalfSize.y == 0.0f
            || Mathf.Abs(CenterX - otherCenter.x) > HalfSizeX + otherHalfSize.x
            || Mathf.Abs(CenterY - otherCenter.y) > HalfSizeY + otherHalfSize.y
            || (HalfSizeZ > 0.0f && otherHalfSize.z > 0.0f && Mathf.Abs(CenterZ - otherCenter.z) > HalfSizeZ + otherHalfSize.z))
            return false;
        return true;
    }


    public static bool Collides(CustomAABB a, CustomAABB b)
    {
        if (a.isCircle && b.isCircle)
            return CircleVsCircle(a, b);
        if (a.isCircle && !b.isCircle)
            return CircleVsAABB(b, a);
        if (!a.isCircle && b.isCircle)
            return CircleVsAABB(a, b);
        return Overlaps(a, b);
    }

    public static bool CircleVsAABB(CustomAABB aabb, CustomAABB circle)
    {
        if (aabb.HalfSizeX == 0.0f || aabb.HalfSizeY == 0.0f)
            return false;

        float s, d = 0;
        float r = circle.HalfSizeX;
        Vector3 min = aabb.Min();
        Vector3 max = aabb.Max();
        Vector3 c = circle.Center;

        if (c.x < min.x)
        {
            s = c.x - min.x;
            d += s * s;
        }
        else if (c.x > max.x)
        {
            s = c.x - max.x;
            d += s * s;
        }

        if (c.y < min.y)
        {
            s = c.y - min.y;
            d += s * s;
        }
        else if (c.y > max.y)
        {

            s = c.y - max.y;
            d += s * s;
        }

        return d <= r * r;
    }

    public static bool CircleVsCircle(CustomAABB a, CustomAABB b)
    {
        if (a.HalfSizeX == 0.0f || a.HalfSizeY == 0.0f
            || b.HalfSizeX == 0.0f || b.HalfSizeY == 0.0f)
            return false;
        if (a.halfSize.x * a.halfSize.x + b.halfSize.x * b.halfSize.x < (a.Center - b.Center).sqrMagnitude)
            return true;
        return false;
    }

    /// <summary>
    /// Overlaps the specified other Axis Aligned Bounding Box.
    /// </summary>
    /// <param name='other'>
    /// The AABB to test against.
    /// </param>
    /// <param name='overlapWidth'>
    /// The signed overlap width.
    /// </param>
    /// <param name='overlapHeight'>
    /// The signed overlap height.
    /// </param>
    public bool Overlaps(CustomAABB other, out float overlapWidth, out float overlapHeight, out float overlapDepth)
    {
        overlapWidth = overlapHeight = overlapDepth = 0;
        if (HalfSizeX == 0.0f || HalfSizeY == 0.0f
            || other.HalfSizeX == 0.0f || other.HalfSizeY == 0.0f
            || Mathf.Abs(CenterX - other.CenterX) > HalfSizeX + other.HalfSizeX
            || Mathf.Abs(CenterY - other.CenterY) > HalfSizeY + other.HalfSizeY
            || (HalfSizeZ > 0.0f && other.HalfSizeZ > 0.0f && Mathf.Abs(CenterZ - other.CenterZ) > HalfSizeZ + other.HalfSizeZ)) return false;

        overlapWidth = (other.HalfSizeX + HalfSizeX) - Mathf.Abs(CenterX - other.CenterX);
        overlapHeight = (other.HalfSizeY + HalfSizeY) - Mathf.Abs(CenterY - other.CenterY);
        overlapDepth = (other.HalfSizeZ + HalfSizeZ) - Mathf.Abs(CenterZ - other.CenterZ);
        return true;
    }

    /// <summary>
    /// Checks for overlap between AABBs a and b.
    /// </summary>
    /// <param name='a'>
    /// A reference to the first AABB.
    /// </param>
    /// <param name='b'>
    /// A reference to the second AABB.
    /// </param>
    public static bool Overlaps(CustomAABB a, CustomAABB b)
    {
        if (a.HalfSizeX == 0.0f || a.HalfSizeY == 0.0f
            || b.HalfSizeX == 0.0f || b.HalfSizeY == 0.0f
            || Mathf.Abs(a.CenterX - b.CenterX) > a.HalfSizeX + b.HalfSizeX
            || Mathf.Abs(a.CenterY - b.CenterY) > a.HalfSizeY + b.HalfSizeY
            || (a.HalfSizeZ > 0.0f && b.HalfSizeZ > 0.0f && Mathf.Abs(a.CenterZ - b.CenterZ) > a.HalfSizeZ + b.HalfSizeZ))
            return false;
        return true;
    }

    public static bool Overlaps(CustomAABB a, Vector3 otherCenter, Vector3 otherHalfSize)
    {
        if (a.HalfSizeX == 0.0f || a.HalfSizeY == 0.0f
            || otherHalfSize.x == 0.0f || otherHalfSize.y == 0.0f
            || Mathf.Abs(a.CenterX - otherCenter.x) > a.HalfSizeX + otherHalfSize.x
            || Mathf.Abs(a.CenterY - otherCenter.y) > a.HalfSizeY + otherHalfSize.y
            || (a.HalfSizeZ > 0.0f && otherHalfSize.z > 0.0f && Mathf.Abs(a.CenterZ - otherCenter.z) > a.HalfSizeZ + otherHalfSize.z))
            return false;
        return true;
    }

    public static bool OverlapsUnsigned(CustomAABB a, CustomAABB b, out float overlapWidth, out float overlapHeight)
    {
        overlapWidth = overlapHeight = 0;
        if (a.HalfSizeX == 0.0f || a.HalfSizeY == 0.0f
            || b.HalfSizeX == 0.0f || b.HalfSizeY == 0.0f
            || Mathf.Abs(a.Center.x - b.CenterX) > a.HalfSizeX + b.HalfSizeX
            || Mathf.Abs(a.Center.y - b.CenterY) > a.HalfSizeY + b.HalfSizeY)
            return false;
        overlapWidth = (b.HalfSizeX + a.HalfSizeX) - Mathf.Abs(a.CenterX - b.CenterX);
        overlapHeight = (b.HalfSizeY + a.HalfSizeY) - Mathf.Abs(a.CenterY - b.CenterY);
        return true;
    }

    public static bool OverlapsUnsigned(CustomAABB a, CustomAABB b, out float overlapWidth, out float overlapHeight, out float overlapDepth)
    {
        overlapWidth = overlapHeight = overlapDepth = 0;
        if (a.HalfSizeX == 0.0f || a.HalfSizeY == 0.0f
            || b.HalfSizeX == 0.0f || b.HalfSizeY == 0.0f
            || Mathf.Abs(a.Center.x - b.CenterX) > a.HalfSizeX + b.HalfSizeX
            || Mathf.Abs(a.Center.y - b.CenterY) > a.HalfSizeY + b.HalfSizeY
            || (a.HalfSizeZ > 0.0f && b.HalfSizeZ > 0.0f && Mathf.Abs(a.Center.z - b.CenterZ) > a.HalfSizeZ + b.HalfSizeZ))
            return false;

        overlapWidth = (b.HalfSizeX + a.HalfSizeX) - Mathf.Abs(a.CenterX - b.CenterX);
        overlapHeight = (b.HalfSizeY + a.HalfSizeY) - Mathf.Abs(a.CenterY - b.CenterY);
        overlapDepth = (b.HalfSizeZ + a.HalfSizeZ) - Mathf.Abs(a.CenterZ - b.CenterZ);
        return true;
    }

    public bool OverlapsUnsigned(CustomAABB other, out float overlapWidth, out float overlapHeight)
    {
        overlapWidth = overlapHeight = 0;
        if (HalfSizeX == 0.0f || HalfSizeY == 0.0f
            || other.HalfSizeX == 0.0f || other.HalfSizeY == 0.0f
            || Mathf.Abs(Center.x - other.CenterX) > HalfSizeX + other.HalfSizeX
            || Mathf.Abs(Center.y - other.CenterY) > HalfSizeY + other.HalfSizeY)
            return false;
        overlapWidth = (other.HalfSizeX + HalfSizeX) - Mathf.Abs(CenterX - other.CenterX);
        overlapHeight = (other.HalfSizeY + HalfSizeY) - Mathf.Abs(CenterY - other.CenterY);
        return true;
    }

    public bool OverlapsUnsigned(CustomAABB other, out float overlapWidth, out float overlapHeight, out float overlapDepth)
    {
        overlapWidth = overlapHeight = overlapDepth = 0;
        if (HalfSizeX == 0.0f || HalfSizeY == 0.0f
            || other.HalfSizeX == 0.0f || other.HalfSizeY == 0.0f
            || Mathf.Abs(Center.x - other.CenterX) > HalfSizeX + other.HalfSizeX
            || Mathf.Abs(Center.y - other.CenterY) > HalfSizeY + other.HalfSizeY
            || (HalfSizeZ > 0.0f && other.HalfSizeZ > 0.0f && Mathf.Abs(Center.z - other.CenterZ) > HalfSizeZ + other.HalfSizeZ))
            return false;
        overlapWidth = (other.HalfSizeX + HalfSizeX) - Mathf.Abs(CenterX - other.CenterX);
        overlapHeight = (other.HalfSizeY + HalfSizeY) - Mathf.Abs(CenterY - other.CenterY);
        overlapDepth = (other.HalfSizeZ + HalfSizeZ) - Mathf.Abs(CenterZ - other.CenterZ);
        return true;
    }

    public bool OverlapsSigned(CustomAABB other, out float overlapWidth, out float overlapHeight)
    {
        overlapWidth = overlapHeight = 0;

        if (HalfSizeX == 0.0f || HalfSizeY == 0.0f
            || other.HalfSizeX == 0.0f || other.HalfSizeY == 0.0f
            || Mathf.Abs(CenterX - other.CenterX) > HalfSizeX + other.HalfSizeX
            || Mathf.Abs(CenterY - other.CenterY) > HalfSizeY + other.HalfSizeY) return false;

        overlapWidth = Mathf.Sign(CenterX - other.CenterX) * ((other.HalfSizeX + HalfSizeX) - Mathf.Abs(CenterX - other.CenterX));
        overlapHeight = Mathf.Sign(CenterY - other.CenterY) * ((other.HalfSizeY + HalfSizeY) - Mathf.Abs(CenterY - other.CenterY));
        return true;
    }

    public bool OverlapsSigned(CustomAABB other, out float overlapWidth, out float overlapHeight, out float overlapDepth)
    {
        overlapWidth = overlapHeight = overlapDepth = 0;

        if (HalfSizeX == 0.0f || HalfSizeY == 0.0f
            || other.HalfSizeX == 0.0f || other.HalfSizeY == 0.0f
            || Mathf.Abs(CenterX - other.CenterX) > HalfSizeX + other.HalfSizeX
            || Mathf.Abs(CenterY - other.CenterY) > HalfSizeY + other.HalfSizeY
            || (HalfSizeZ > 0.0f && other.HalfSizeZ > 0.0f && Mathf.Abs(CenterY - other.CenterZ) > HalfSizeZ + other.HalfSizeZ))
            return false;

        overlapWidth = Mathf.Sign(CenterX - other.CenterX) * ((other.HalfSizeX + HalfSizeX) - Mathf.Abs(CenterX - other.CenterX));
        overlapHeight = Mathf.Sign(CenterY - other.CenterY) * ((other.HalfSizeY + HalfSizeY) - Mathf.Abs(CenterY - other.CenterY));
        overlapDepth = Mathf.Sign(CenterZ - other.CenterZ) * ((other.HalfSizeZ + HalfSizeZ) - Mathf.Abs(CenterZ - other.CenterZ));
        return true;
    }

    public bool OverlapsSigned(CustomAABB other, out Vector3 overlap)
    {
        overlap = Vector3.zero;

        if (HalfSizeX == 0.0f || HalfSizeY == 0.0f
            || other.HalfSizeX == 0.0f || other.HalfSizeY == 0.0f
            || Mathf.Abs(CenterX - other.CenterX) > HalfSizeX + other.HalfSizeX
            || Mathf.Abs(CenterY - other.CenterY) > HalfSizeY + other.HalfSizeY
            || (HalfSizeZ > 0.0f && other.HalfSizeZ > 0.0f && Mathf.Abs(CenterZ - other.CenterZ) > HalfSizeZ + other.HalfSizeZ)) return false;

        overlap = new Vector3(Mathf.Sign(CenterX - other.CenterX) * ((other.HalfSizeX + HalfSizeX) - Mathf.Abs(CenterX - other.CenterX)),
            Mathf.Sign(CenterY - other.CenterY) * ((other.HalfSizeY + HalfSizeY) - Mathf.Abs(CenterY - other.CenterY)),
            Mathf.Sign(CenterZ - other.CenterZ) * ((other.HalfSizeZ + HalfSizeZ) - Mathf.Abs(CenterZ - other.CenterZ)));

        return true;
    }

    /// <summary>
    /// Checks whether a Vector3i p lies inside AABB a.
    /// </summary>
    /// <returns>
    /// True if the Vector3i is inside the AABB, otherwise false.
    /// </returns>
    /// <param name='a'>
    /// If set to <c>true</c> a.
    /// </param>
    /// <param name='p'>
    /// If set to <c>true</c> p.
    /// </param>
    public static bool PointInside(CustomAABB a, Vector3 p)
    {
        if (Mathf.Abs(a.CenterX - p.x) > a.HalfSizeX
            || Mathf.Abs(a.CenterY - p.y) > a.HalfSizeY
            || Mathf.Abs(a.CenterZ - p.z) > a.HalfSizeZ) return false;
        return true;
    }

    public bool PointInside(Vector3 p)
    {
        if (Mathf.Abs(CenterX - p.x) > HalfSizeX
            || Mathf.Abs(CenterY - p.y) > HalfSizeY
            || Mathf.Abs(CenterZ - p.z) > HalfSizeZ) return false;
        return true;
    }

    public bool Overlaps2D(CustomAABB other)
    {
        if (HalfSizeX == 0.0f || HalfSizeY == 0.0f || other.HalfSizeX == 0.0f || other.HalfSizeY == 0.0f
            || Mathf.Abs(CenterX - other.CenterX) > HalfSizeX + other.HalfSizeX
            || Mathf.Abs(CenterY - other.CenterY) > HalfSizeY + other.HalfSizeY)
            return false;
        return true;
    }

    public bool Overlaps2D(Vector2 otherCenter, Vector2 otherHalfSize)
    {
        if (HalfSizeX == 0.0f || HalfSizeY == 0.0f || otherHalfSize.x == 0.0f || otherHalfSize.y == 0.0f
            || Mathf.Abs(CenterX - otherCenter.x) > HalfSizeX + otherHalfSize.x
            || Mathf.Abs(CenterY - otherCenter.y) > HalfSizeY + otherHalfSize.y)
            return false;
        return true;
    }

    public static bool Collides2D(CustomAABB a, CustomAABB b)
    {
        if (a.isCircle && b.isCircle)
            return CircleVsCircle(a, b);
        if (a.isCircle && !b.isCircle)
            return CircleVsAABB2D(b, a);
        if (!a.isCircle && b.isCircle)
            return CircleVsAABB2D(a, b);
        return Overlaps(a, b);
    }

    public static bool CircleVsAABB2D(CustomAABB aabb, CustomAABB circle)
    {
        if (aabb.HalfSizeX == 0.0f || aabb.HalfSizeY == 0.0f || circle.HalfSizeX == 0.0f)
            return false;

        float s, d = 0;
        float r = circle.HalfSizeX;
        Vector2 min = aabb.Min();
        Vector2 max = aabb.Max();
        Vector2 c = circle.Center;

        if (c.x < min.x)
        {
            s = c.x - min.x;
            d += s * s;
        }
        else if (c.x > max.x)
        {

            s = c.x - max.x;
            d += s * s;
        }

        if (c.y < min.y)
        {
            s = c.y - min.y;
            d += s * s;
        }
        else if (c.y > max.y)
        {

            s = c.y - max.y;
            d += s * s;
        }

        return d <= r * r;
    }

    public static bool CircleVsCircle2D(CustomAABB a, CustomAABB b)
    {
        if (a.HalfSizeX == 0.0f || a.HalfSizeY == 0.0f || b.HalfSizeX == 0.0f || b.HalfSizeY == 0.0f)
            return false;
        if (a.halfSize.x * a.halfSize.x + b.halfSize.x * b.halfSize.x < (a.Center - b.Center).sqrMagnitude)
            return true;
        return false;
    }

    /// <summary>
    /// Overlaps the specified other Axis Aligned Bounding Box.
    /// </summary>
    /// <param name='other'>
    /// The AABB2D to test against.
    /// </param>
    /// <param name='overlapWidth'>
    /// The signed overlap width.
    /// </param>
    /// <param name='overlapHeight'>
    /// The signed overlap height.
    /// </param>
    public bool Overlaps2D(CustomAABB other, out float overlapWidth, out float overlapHeight)
    {
        overlapWidth = overlapHeight = 0;
        if (HalfSizeX == 0.0f || HalfSizeY == 0.0f || other.HalfSizeX == 0.0f || other.HalfSizeY == 0.0f
            || Mathf.Abs(CenterX - other.CenterX) > HalfSizeX + other.HalfSizeX
            || Mathf.Abs(CenterY - other.CenterY) > HalfSizeY + other.HalfSizeY) return false;

        overlapWidth = (other.HalfSizeX + HalfSizeX) - Mathf.Abs(CenterX - other.CenterX);
        overlapHeight = (other.HalfSizeY + HalfSizeY) - Mathf.Abs(CenterY - other.CenterY);
        return true;
    }

    /// <summary>
    /// Checks for overlap between AABB2Ds a and b.
    /// </summary>
    /// <param name='a'>
    /// A reference to the first AABB2D.
    /// </param>
    /// <param name='b'>
    /// A reference to the second AABB2D.
    /// </param>
    public static bool Overlaps2D(CustomAABB a, CustomAABB b)
    {
        if (a.HalfSizeX == 0.0f || a.HalfSizeY == 0.0f || b.HalfSizeX == 0.0f || b.HalfSizeY == 0.0f
            || Mathf.Abs(a.CenterX - b.CenterX) > a.HalfSizeX + b.HalfSizeX
            || Mathf.Abs(a.CenterY - b.CenterY) > a.HalfSizeY + b.HalfSizeY)
            return false;
        return true;
    }

    public static bool Overlaps2D(CustomAABB a, Vector2 otherCenter, Vector2 otherHalfSize)
    {
        if (a.HalfSizeX == 0.0f || a.HalfSizeY == 0.0f || otherHalfSize.x == 0.0f || otherHalfSize.y == 0.0f
            || Mathf.Abs(a.CenterX - otherCenter.x) > a.HalfSizeX + otherHalfSize.x
            || Mathf.Abs(a.CenterY - otherCenter.y) > a.HalfSizeY + otherHalfSize.y)
            return false;
        return true;
    }

    public static bool OverlapsUnsigned2D(CustomAABB a, CustomAABB b, out float overlapWidth, out float overlapHeight)
    {
        overlapWidth = overlapHeight = 0;
        if (a.HalfSizeX == 0.0f || a.HalfSizeY == 0.0f || b.HalfSizeX == 0.0f || b.HalfSizeY == 0.0f
            || Mathf.Abs(a.Center.x - b.CenterX) > a.HalfSizeX + b.HalfSizeX
            || Mathf.Abs(a.Center.y - b.CenterY) > a.HalfSizeY + b.HalfSizeY)
            return false;
        overlapWidth = (b.HalfSizeX + a.HalfSizeX) - Mathf.Abs(a.CenterX - b.CenterX);
        overlapHeight = (b.HalfSizeY + a.HalfSizeY) - Mathf.Abs(a.CenterY - b.CenterY);
        return true;
    }

    public bool OverlapsUnsigned2D(CustomAABB other, out float overlapWidth, out float overlapHeight)
    {
        overlapWidth = overlapHeight = 0;
        if (HalfSizeX == 0.0f || HalfSizeY == 0.0f || other.HalfSizeX == 0.0f || other.HalfSizeY == 0.0f
            || Mathf.Abs(Center.x - other.CenterX) > HalfSizeX + other.HalfSizeX
            || Mathf.Abs(Center.y - other.CenterY) > HalfSizeY + other.HalfSizeY)
            return false;
        overlapWidth = (other.HalfSizeX + HalfSizeX) - Mathf.Abs(CenterX - other.CenterX);
        overlapHeight = (other.HalfSizeY + HalfSizeY) - Mathf.Abs(CenterY - other.CenterY);
        return true;
    }

    public bool OverlapsSigned2D(CustomAABB other, out float overlapWidth, out float overlapHeight)
    {
        overlapWidth = overlapHeight = 0;

        if (HalfSizeX == 0.0f || HalfSizeY == 0.0f || other.HalfSizeX == 0.0f || other.HalfSizeY == 0.0f
            || Mathf.Abs(CenterX - other.CenterX) > HalfSizeX + other.HalfSizeX
            || Mathf.Abs(CenterY - other.CenterY) > HalfSizeY + other.HalfSizeY) return false;

        overlapWidth = Mathf.Sign(CenterX - other.CenterX) * ((other.HalfSizeX + HalfSizeX) - Mathf.Abs(CenterX - other.CenterX));
        overlapHeight = Mathf.Sign(CenterY - other.CenterY) * ((other.HalfSizeY + HalfSizeY) - Mathf.Abs(CenterY - other.CenterY));
        return true;
    }

    public bool OverlapsSigned2D(CustomAABB other, out Vector2 overlap)
    {
        overlap = Vector2.zero;

        if (HalfSizeX == 0.0f || HalfSizeY == 0.0f || other.HalfSizeX == 0.0f || other.HalfSizeY == 0.0f
            || Mathf.Abs(CenterX - other.CenterX) > HalfSizeX + other.HalfSizeX
            || Mathf.Abs(CenterY - other.CenterY) > HalfSizeY + other.HalfSizeY) return false;

        overlap = new Vector2(Mathf.Sign(CenterX - other.CenterX) * ((other.HalfSizeX + HalfSizeX) - Mathf.Abs(CenterX - other.CenterX)),
            Mathf.Sign(CenterY - other.CenterY) * ((other.HalfSizeY + HalfSizeY) - Mathf.Abs(CenterY - other.CenterY)));

        return true;
    }

    /// <summary>
    /// Checks whether a Vector2i p lies inside AABB2D a.
    /// </summary>
    /// <returns>
    /// True if the Vector2i is inside the AABB2D, otherwise false.
    /// </returns>
    /// <param name='a'>
    /// If set to <c>true</c> a.
    /// </param>
    /// <param name='p'>
    /// If set to <c>true</c> p.
    /// </param>
    public static bool PointInside2D(CustomAABB a, Vector2 p)
    {
        if (Mathf.Abs(a.CenterX - p.x) > a.HalfSizeX
            || Mathf.Abs(a.CenterY - p.y) > a.HalfSizeY) return false;
        return true;
    }

    public bool PointInside2D(Vector2 p)
    {
        if (Mathf.Abs(CenterX - p.x) > HalfSizeX
            || Mathf.Abs(CenterY - p.y) > HalfSizeY) return false;
        return true;
    }
}
