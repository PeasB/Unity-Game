#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System.Collections.Generic;

namespace TileEditor
{
    public class TileMapWizard : ScriptableWizard
    {
        public string MapName = "Tile Map";
        public int MapWidth = 32;
        public int MapHeight = 32;
        public float TileWidth = 1;
        public float TileHeight = 1;

        [MenuItem("GameObject/Create TileMap %#m")]
        static void CreateWizard()
        {
            TileMapWizard wizard = ScriptableWizard.DisplayWizard<TileMapWizard>("Create TileMap");
            wizard.minSize = new Vector2(300, 250);
        }

        void OnWizardUpdate()
        {
            MapWidth = Mathf.Max(MapWidth, 1);
            MapHeight = Mathf.Max(MapHeight, 1);
            TileWidth = Mathf.Max(TileWidth, 0.1f);
            TileHeight = Mathf.Max(TileHeight, 0.1f);
        }

        void OnWizardCreate()
        {
            GameObject go = new GameObject(MapName);
            TileMap tileMap = go.AddComponent<TileMap>();
            tileMap.MapSize = new Vector2(MapWidth, MapHeight);
            tileMap.TileSize = new Vector2(TileWidth, TileHeight);
            TileLayer newLayer = TileEditorUtility.CreateLayer(tileMap);
            newLayer.gameObject.hideFlags = HideFlags.HideInHierarchy;
            tileMap.TileLayers = new List<TileLayer>();
            tileMap.TileLayers.Add(newLayer);
            Undo.RegisterCreatedObjectUndo(go, "Create Tile Map");
            Selection.activeGameObject = go;

        }
    }
}
#endif