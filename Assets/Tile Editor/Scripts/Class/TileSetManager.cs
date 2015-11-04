#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace TileEditor
{
    [System.Serializable]
    public class TileSetManager : ScriptableObject
    {
        #region Public Fields
        public TileData tileDataInScene;
        public List<TileSet> TileSets;
        #endregion

        #region Public Methods
        public void OnEnable()
        {
            if (TileSets == null)
            {
                TileSets = new List<TileSet>();
            }
        }

        public void ClearDeletedTextures()
        {
            if (TileSets == null)
            {
                return;
            }

            List<TileData> removeList = new List<TileData>();

            for (int i = 0; i < TileSets.Count; ++i)
            {
                TileSet set = TileSets[i];

                if (set != null)
                {
                    removeList.Clear();
                    foreach (TileData data in set.TileList)
                    {
                        if (data.Sprite == null)
                        {
                            removeList.Add(data);
                        }
                    }

                    foreach (TileData tileData in removeList)
                    {
                        set.TileList.Remove(tileData);
                    }

                }
            }
        }

        public void ClearTextures()
        {
            foreach (TileSet tileSet in TileSets)
            {
                foreach (TileData tile in tileSet.TileList)
                {
                    DestroyImmediate(tile.Texture);
                }
            }

            if (tileDataInScene != null)
            {
                DestroyImmediate(tileDataInScene.Texture);
                tileDataInScene = null;
            }
        }
        #endregion
    }
}
#endif