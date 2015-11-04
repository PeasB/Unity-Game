using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TileEditor
{
    public class TileMap : MonoBehaviour
    {
        #region Public Fields
        public Vector2 NewMapSize;
        public Vector2 MapSize;
        public Vector2 TileSize;
        public string ResourceFoler;
        public List<TileLayer> TileLayers;
        public float PreviewSize;
        public int SelectedLayerIndex;
        public bool ShowEdit;

        public bool ShowLayer;
        #endregion

        #region Properties

        public int LayerCount
        {
            get
            {
                if (TileLayers != null)
                {
                    return TileLayers.Count;
                }
                return 0;
            }
        }

        public Rect Boundary
        {
            get
            {
                return new Rect(
                    transform.position.x,
                    transform.position.y + MapSize.y * TileSize.y,
                    MapSize.x * TileSize.x,
                    MapSize.y * TileSize.y
                    );
            }

        }

        #endregion

        #region Public Methods

        public GameObject TileAt(Vector2 worldPosition, int layer)
        {
            if (layer < 0 || TileLayers == null || layer >= TileLayers.Count)
            {
                return null;
            }

            Vector2 gridIndex = TransformPositionToGridIndex(worldPosition);
            if (gridIndex.x < 0 || gridIndex.x >= MapSize.x || gridIndex.y < 0 || gridIndex.y >= MapSize.y)
            {
                return null;
            }
            TileLayer tileLayer = TileLayers[layer];
            int key = (int)(MapSize.x * gridIndex.y + gridIndex.x);
            Tile tile = tileLayer.Tiles[key];
            if (tile == null)
            {
                return null;
            }
            return tile.gameObject;
        }

        public bool IsInTileMap(Vector2 pos)
        {
            float minX = transform.position.x;
            float maxX = minX + MapSize.x * TileSize.x;
            float minY = transform.position.y;
            float maxY = minY + MapSize.y * TileSize.y;
            return (pos.x >= minX && pos.x <= maxX && pos.y >= minY && pos.y <= maxY);
        }

        public Vector2 TransformPositionToGridIndex(Vector2 position)
        {
            Vector2 gridIndex = new Vector2(
                   (int)((position.x - transform.position.x) / TileSize.x),
                   (int)((position.y - transform.position.y) / TileSize.y)
                );

            return gridIndex;
        }

        public Vector2 ClampGridIndex(Vector2 gridIndex)
        {
            return new Vector2((int)Mathf.Clamp(gridIndex.x, 0, MapSize.x - 1), (int)Mathf.Clamp(gridIndex.y, 0, MapSize.y - 1));
        }

        public Vector2 GetGridIndexPosInWorldSpace(Vector2 gridIndex)
        {
            Vector2 pos = new Vector2(gridIndex.x * TileSize.x,
                gridIndex.y * TileSize.y);
            pos += new Vector2(0.5f * TileSize.x, 0.5f * TileSize.y);
            pos += new Vector2(transform.position.x, transform.position.y);
            return pos;
        }

        #endregion
    }

}