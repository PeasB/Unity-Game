#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEditorInternal;
using System;
using System.Reflection;
using System.Collections.Generic;

namespace TileEditor
{
    public class TileEditorUtility
    {
        #region Public Static Methods
        public static void DrawGrids(Vector3 origin, Vector2 mapSize, Vector2 tileSize, Color gridColor)
        {
            Handles.color = gridColor;
            int hCount = (int)mapSize.x;
            int vCount = (int)mapSize.y;

            float hLineLength = hCount * tileSize.x;
            float vLineLength = vCount * tileSize.y;
            for (var y = 0; y <= vCount; ++y)
            {
                Vector3 p1 = new Vector3(0, y * tileSize.y) + origin;
                Vector3 p2 = new Vector3(hLineLength, y * tileSize.y) + origin;
                Handles.DrawLine(p1, p2);
            }
            for (var x = 0; x <= hCount; ++x)
            {
                Vector3 p1 = new Vector3(x * tileSize.x, 0) + origin;
                Vector3 p2 = new Vector3(x * tileSize.x, vLineLength) + origin;
                Handles.DrawLine(p1, p2);
            }
        }

        public static Vector2 GetMousePosInWorld(Vector2 mousePosition)
        {
            if (Camera.current == null)
            {
                return Vector2.zero;
            }
            return Camera.current.ScreenToWorldPoint(new Vector2(mousePosition.x, Camera.current.pixelHeight - mousePosition.y));
        }

        public static GameObject CreateTile(Vector3 position, TileData tileData, float alpha, string sortingLayerName)
        {
            if (tileData == null || tileData.Sprite == null)
            {
                return null;
            }
            GameObject go = new GameObject();
            go.transform.position = position;

            Vector3 localScale = go.transform.localScale;
            if (tileData.FlipHorizontally)
            {
                localScale.x = -1;
            }
            else
            {
                localScale.x = 1;
            }

            if (tileData.FlipVertically)
            {
                localScale.y = -1;
            }
            else
            {
                localScale.y = 1;
            }
            go.transform.localScale = localScale;
            SpriteRenderer renderer = go.AddComponent(typeof(SpriteRenderer)) as SpriteRenderer;
            renderer.sprite = tileData.Sprite;
            renderer.sortingLayerName = sortingLayerName;
            renderer.sortingOrder = tileData.OrderInLayer;
            Color color = renderer.color;
            renderer.color = new Color(color.r, color.g, color.b, alpha);
            return go;
        }


        #region ColliderInfo Methods

        public static bool IsTileColliderEqualsToColliderInfo(Tile tile, ColliderInfo colliderInfo)
        {
            if (tile == null || colliderInfo == null)
            {
                return false;
            }
            switch (colliderInfo.CollisionType)
            {
                case CollisionType.Box:
                    {
                        BoxCollider2D collider = tile.GetComponent(typeof(BoxCollider2D)) as BoxCollider2D;
                        BoxCollider2DInfo info = colliderInfo as BoxCollider2DInfo;
                        if (collider == null || info == null)
                        {
                            return false;
                        }
#if UNITY_5_0
                        if (info.Center == collider.offset && info.Size == collider.size)
#else
                         if (info.Center == collider.offset && info.Size == collider.size)
#endif
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                case CollisionType.Circle:
                    {
                        CircleCollider2D collider = tile.GetComponent(typeof(CircleCollider2D)) as CircleCollider2D;
                        CircleCollider2DInfo info = colliderInfo as CircleCollider2DInfo;
                        if (collider == null || info == null)
                        {
                            return false;
                        }
#if UNITY_5_0
                        if (collider.offset == info.Center && collider.radius == info.Radius)
#else
                        if (collider.offset == info.Center && collider.radius == info.Radius)

#endif
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                case CollisionType.Polygon:
                    {
                        PolygonCollider2D collider = tile.GetComponent(typeof(PolygonCollider2D)) as PolygonCollider2D;
                        PolygonCollider2DInfo info = colliderInfo as PolygonCollider2DInfo;
                        if (collider == null || info == null)
                        {
                            return false;
                        }

                        if (collider.pathCount == info.PathCount)
                        {
                            for (int i = 0; i < collider.pathCount; ++i)
                            {
                                if (collider.points[i] != info.Points[i])
                                {
                                    return false;
                                }
                            }
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                default:
                    return true;
            }
        }

        public static ColliderInfo GetColliderInfoFromTile(Tile tile)
        {
            if (tile == null)
            {
                return null;
            }

            ColliderInfo info = null;
            CollisionType collisionType = tile.Collision;

            switch (collisionType)
            {
                case CollisionType.Box:
                    {
                        BoxCollider2D collider = tile.GetComponent(typeof(BoxCollider2D)) as BoxCollider2D;
                        if (collider)
                        {
#if UNITY_5_0
                            info = new BoxCollider2DInfo(collider.offset, collider.size);
#else
                              info = new BoxCollider2DInfo(collider.offset, collider.size);
#endif

                        }
                        break;
                    }
                case CollisionType.Circle:
                    {
                        CircleCollider2D collider = tile.GetComponent(typeof(CircleCollider2D)) as CircleCollider2D;
                        if (collider)
                        {
#if UNITY_5_0
                            info = new CircleCollider2DInfo(collider.offset, collider.radius);
#else
                              info = new CircleCollider2DInfo(collider.offset, collider.radius);
#endif

                        }
                        break;
                    }
                case CollisionType.Polygon:
                    {
                        PolygonCollider2D collider = tile.GetComponent(typeof(PolygonCollider2D)) as PolygonCollider2D;
                        if (collider)
                        {
                            info = new PolygonCollider2DInfo(collider.pathCount, collider.points);
                        }
                        break;
                    }
                default:
                    break;
            }

            return info;
        }

        public static void ApplyColliderInfoToTile(Tile tile, ColliderInfo colliderInfo)
        {
            if (tile == null || colliderInfo == null)
            {
                return;
            }

            CollisionType collisionType = colliderInfo.CollisionType;

            switch (collisionType)
            {
                case CollisionType.Box:
                    {
                        BoxCollider2DInfo info = colliderInfo as BoxCollider2DInfo;
                        BoxCollider2D collider = tile.GetComponent(typeof(BoxCollider2D)) as BoxCollider2D;
                        if (info != null && collider != null)
                        {
                            collider.size = info.Size;

#if UNITY_5_0
                            collider.offset = info.Center;
#else
                             collider.offset = info.Center;
#endif

                        }
                        break;
                    }

                case CollisionType.Circle:
                    {
                        CircleCollider2DInfo info = colliderInfo as CircleCollider2DInfo;
                        CircleCollider2D collider = tile.GetComponent(typeof(CircleCollider2D)) as CircleCollider2D;
                        if (info != null && collider != null)
                        {
#if UNITY_5_0
                            collider.offset = info.Center;
#else
                            collider.offset = info.Center;
#endif

                            collider.radius = info.Radius;
                        }
                        break;
                    }

                case CollisionType.Polygon:
                    {
                        PolygonCollider2DInfo info = colliderInfo as PolygonCollider2DInfo;
                        PolygonCollider2D collider = tile.GetComponent(typeof(PolygonCollider2D)) as PolygonCollider2D;
                        if (info != null && collider != null)
                        {
                            collider.pathCount = info.PathCount;
                            collider.points = info.Points;
                        }
                        break;
                    }

                default:
                    break;
            }

        }

        #endregion

        public static bool IsTheSameTile(Tile tile, TileData tileData)
        {
            Vector3 localScale = tile.transform.localScale;
            SpriteRenderer renderer = tile.GetComponent<SpriteRenderer>();

            if (tile.tag != tileData.Tag)
            {
                return false;
            }

            if (renderer.sortingOrder != tileData.OrderInLayer)
            {
                return false;
            }

            if ((tileData.FlipHorizontally && localScale.x != -1) || (!tileData.FlipHorizontally && localScale.x != 1))
            {
                return false;
            }

            if ((tileData.FlipVertically && localScale.y != -1) || (!tileData.FlipVertically && localScale.y != 1))
            {
                return false;
            }

            if (renderer.sprite != tileData.Sprite)
            {
                return false;
            }

            if (tile.Collision != tileData.Collision)
            {
                return false;
            }

            if (Mathf.Abs(tile.transform.eulerAngles.z - tileData.Rotation) > 0.0001f)
            {
                return false;
            }

            Collider2D collider = tile.GetComponent<Collider2D>();
            if (collider != null)
            {
                if (collider.isTrigger != tileData.IsTrigger || collider.sharedMaterial != tileData.PhysicsMaterial)
                {
                    return false;
                }
            }

            return true;
        }

        public static TileLayer CreateLayer(TileMap tileMap)
        {
            GameObject go = new GameObject("Tile Layer");
            TileLayer layer = go.AddComponent(typeof(TileLayer)) as TileLayer;
            layer.Tiles = new Tile[(int)(tileMap.MapSize.x * tileMap.MapSize.y)];
            layer.transform.parent = tileMap.transform;
            go.transform.position = tileMap.transform.position;
            return layer;
        }

        public static int GetSortingLayerID(string sortingLayer)
        {
            string[] sortingLayerNames = TileEditorUtility.GetSortingLayerNames();
            for (int i = 0; i < sortingLayerNames.Length; ++i)
            {
                if (sortingLayerNames[i] == sortingLayer)
                {
                    return i;
                }
            }
            return -1;
        }

        public static String[] GetTags()
        {
            return UnityEditorInternal.InternalEditorUtility.tags;
        }

        public static string[] GetSortingLayerNames()
        {
            Type internalEditorUtilityType = typeof(InternalEditorUtility);
            PropertyInfo sortingLayersProperty = internalEditorUtilityType.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
            return (string[])sortingLayersProperty.GetValue(null, new object[0]);
        }

        public static string[] GetLayerNames()
        {
            List<string> layers = new List<string>();

            for (int i = 0; i < 32; ++i)
            {
                string name = LayerMask.LayerToName(i);
                if (name != null && name.Length > 0)
                {
                    layers.Add(name);
                }
            }
            return layers.ToArray();
        }

        public static List<Sprite> GetSpritesFromTexture(Texture2D texture)
        {

            if (texture == null)
            {
                return null;
            }
            List<Sprite> retValue = null;
            string path = AssetDatabase.GetAssetPath(texture);
            UnityEngine.Object[] assets = AssetDatabase.LoadAllAssetsAtPath(path);
            if (assets != null)
            {
                retValue = new List<Sprite>();
                foreach (UnityEngine.Object asset in assets)
                {
                    if (asset is Sprite)
                    {
                        retValue.Add(asset as Sprite);
                    }
                }
            }


            return retValue;
        }

        public static TileData GetTileDataFromTile(Tile tile)
        {
            if (tile == null)
            {
                return null;
            }
            TileData tileData = new TileData();
            SpriteRenderer renderer = tile.GetComponent<SpriteRenderer>();
            Collider2D collider = tile.GetComponent<Collider2D>();
            Vector3 localScale = tile.transform.localScale;
            tileData.FlipHorizontally = localScale.x == -1;
            tileData.FlipVertically = localScale.y == -1;
            tileData.Sprite = renderer.sprite;
            tileData.OrderInLayer = renderer.sortingOrder;
            tileData.Rotation = tile.transform.rotation.eulerAngles.z;
            tileData.Collision = tile.Collision;
            tileData.Tag = tile.tag;
            if (collider != null)
            {
                tileData.IsTrigger = collider.isTrigger;
                tileData.PhysicsMaterial = collider.sharedMaterial;
            }
            return tileData;
        }

        #endregion
    }
}
#endif