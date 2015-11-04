#if  UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TileEditor
{
    [System.Serializable]
    public class TileSet
    {
        #region Public Fields
        public string Name = "Set";
        public List<TileData> TileList;
        #endregion
    }
}
#endif