#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections;

namespace TileEditor
{
    public class TileSetNameWizard : ScriptableWizard
    {
        public string TileSetName = null;

        private TileSetManager manager = null;
        private TileSet tileSet = null;

        public void SetTileSet(TileSet tileSet)
        {
            if (tileSet != null)
            {
                this.tileSet = tileSet;
                TileSetName = this.tileSet.Name;
                OnWizardUpdate();
            }
        }

        public void SetManager(TileSetManager manager)
        {
            this.manager = manager;
        }

        void OnWizardUpdate()
        {
            if (TileSetName == null || TileSetName == "")
            {
                isValid = false;
            }
            else
            {
                isValid = true;
            }

        }

        void OnWizardCreate()
        {
            if (this.tileSet != null && this.manager != null)
            {
                Undo.RecordObject(this.manager, null);
                this.tileSet.Name = TileSetName;
            }
        }
    }
}

#endif