#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

namespace TileEditor
{
    [System.Serializable]
    public class TileData
    {
        #region Public Fields
        public Sprite Sprite;
        public CollisionType Collision;
        public PhysicsMaterial2D PhysicsMaterial;
        public bool IsTrigger;
        public float Rotation;
        public bool FlipHorizontally;
        public bool FlipVertically;
        public int OrderInLayer;
        public string Tag = "Untagged";
        #endregion

        #region Properties
        public Texture2D Texture
        {
            get
            {
                if (_texture == null)
                {
                    _texture = CreateTexture(Sprite);
                }
                return _texture;
            }
        }
        #endregion

        #region Private Fields

        [System.NonSerialized]

        private Texture2D _texture;
        #endregion

        #region  Private Methods
        private Texture2D CreateTexture(Sprite sprite)
        {

            if (!sprite)
            {
                return null;
            }
            Texture2D texture = null;
            try
            {
                texture = SetupTexture(sprite);
            }
            catch
            {
                string path = AssetDatabase.GetAssetPath(sprite);
                string fileName = Path.GetFileNameWithoutExtension(path);
                string tpPath = Path.ChangeExtension(path, ".tpsheet");
                if (File.Exists(tpPath))
                {
                    AssetDatabase.RenameAsset(tpPath, fileName + "_temp");
                }

                TextureImporter importer = (TextureImporter)AssetImporter.GetAtPath(path);
                importer.isReadable = true;
                importer.textureFormat = TextureImporterFormat.RGBA32;
                AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);

                Debug.Log(Path.GetPathRoot(tpPath));

                try
                {
                    texture = SetupTexture(sprite);
                }
                catch
                {
                    Debug.LogError("Texture error!");
                }

            }
            return texture;
        }

        private static Texture2D SetupTexture(Sprite sprite)
        {
            Texture2D texture = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
            var pixels = sprite.texture.GetPixels((int)sprite.rect.x, (int)sprite.rect.y, (int)sprite.rect.width, (int)sprite.rect.height);
            texture.SetPixels(pixels);
            texture.name = sprite.name;
            texture.hideFlags = HideFlags.DontSave;
            texture.filterMode = FilterMode.Point;
            texture.Apply();
            return texture;
        }
        #endregion

    }
}
#endif