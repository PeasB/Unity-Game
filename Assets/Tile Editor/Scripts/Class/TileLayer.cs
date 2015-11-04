using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;

namespace TileEditor
{
    public class TileLayer : MonoBehaviour
    {
        #region Public Fields

        [HideInInspector]
        public Tile[] Tiles;
        #endregion

        #region Properties
        public int Alpha
        {
            get
            {
                return _alpha;
            }
            set
            {


#if UNITY_EDITOR
                Undo.RecordObject(this, null);

#endif
                _alpha = Mathf.Clamp(value, 0, 255);
                foreach (Tile tile in Tiles)
                {
                    if (tile)
                    {
                        SpriteRenderer renderer = tile.GetComponent<SpriteRenderer>();
                        if (renderer)
                        {
#if UNITY_EDITOR
                            Undo.RecordObject(renderer, null);
#endif
                            Color color = renderer.color;
                            color = new Color(color.r, color.g, color.b, _alpha / 255.0f);
                            renderer.color = color;
                        }
                    }
                }
            }
        }

        public int PhysicsLayer
        {

            get
            {
                return _physcisLayer;
            }
            set
            {
                value = Mathf.Clamp(value, 0, 31);

#if UNITY_EDITOR
                Undo.RecordObject(this, null);
#endif
                _physcisLayer = value;
                gameObject.layer = _physcisLayer;

                foreach (Tile tile in Tiles)
                {
                    if (tile)
                    {
#if UNITY_EDITOR
                        Undo.RecordObject(tile.gameObject, null);
#endif
                        tile.gameObject.layer = _physcisLayer;
                    }
                }

            }
        }

        public string SortingLayer
        {
            get
            {
                return _sortingLayer;
            }
            set
            {
#if UNITY_EDITOR
                Undo.RecordObject(this, null);
#endif
                _sortingLayer = value;
                foreach (Tile tile in Tiles)
                {
                    if (tile)
                    {
                        SpriteRenderer renderer = tile.GetComponent<SpriteRenderer>();
                        if (renderer)
                        {
#if UNITY_EDITOR
                            Undo.RecordObject(renderer, null);
#endif
                            renderer.sortingLayerName = _sortingLayer;

                        }
                    }
                }



            }
        }

        public float Z
        {
            get
            {
                return transform.position.z;
            }
            set
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, value);
            }
        }
        #endregion

        #region Private Fields
        [HideInInspector]
        public LayerMask Layer;

        [SerializeField]
        [HideInInspector]
        private int _alpha = 255;

        [SerializeField]
        [HideInInspector]
        private string _sortingLayer = "Default";

        [SerializeField]
        [HideInInspector]
        private int _physcisLayer = 0;
        #endregion

    }
}