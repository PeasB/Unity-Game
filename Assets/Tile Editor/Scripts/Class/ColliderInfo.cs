#if UNITY_EDITOR
using UnityEngine;
using System.Collections;

namespace TileEditor
{
    public class ColliderInfo
    {
        public CollisionType CollisionType = CollisionType.None;
    }

    public class BoxCollider2DInfo : ColliderInfo
    {
        public Vector2 Center;
        public Vector2 Size;
        public BoxCollider2DInfo(Vector2 _center, Vector2 _size)
        {
            CollisionType = CollisionType.Box;
            Center = _center;
            Size = _size;
        }
    }

    public class CircleCollider2DInfo : ColliderInfo
    {
        public Vector2 Center;
        public float Radius;
        public CircleCollider2DInfo(Vector2 _center, float _radius)
        {
            CollisionType = CollisionType.Circle;
            Center = _center;
            Radius = _radius;
        }
    }

    public class PolygonCollider2DInfo : ColliderInfo
    {
        public int PathCount;
        public Vector2[] Points;

        public PolygonCollider2DInfo(int _pathCount, Vector2[] _points)
        {
            CollisionType = CollisionType.Polygon;
            PathCount = _pathCount;
            Points = _points;
        }

    }
}
#endif