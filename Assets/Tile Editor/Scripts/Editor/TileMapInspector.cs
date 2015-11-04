#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace TileEditor
{
    [CustomEditor(typeof(TileMap))]
    public class TileMapInspector : Editor
    {
        #region Enum
        public enum EditMode
        {
            Paint = 0,
            Move = 1,
            Select = 2,
            Erase = 3,
        }

        public enum TileSelectMode
        {
            None = 0,
            TileSet = 1,
            Scene = 2,
        }

        #endregion

        #region Private Fields
        private SerializedObject m_Object;
        private SerializedProperty newMapSize;
        private SerializedProperty previewSize;
        private SerializedProperty selectedLayer;
        private SerializedProperty showEdit;
        private TileMap tileMap;
        private TileSetManager tileSetManager;
        private const float minPreviewSize = 30;
        private const float maxPreviewSize = 90;
        private string tileSetManagerDir = "Tile Editor/TileSet";
        private string tileSetManagerPath = "Assets/Tile Editor/TileSet/TileSets.asset";
        private Tool savedTool;
        private Vector3[] mouseRectCorner = new Vector3[4];
        private int currentEditMode = 0;
        private int lastEditMode = 0;
        private GUISkin skin = null;
        private Vector2 tileScrollPosition;
        private int selectedTileIndex = 0;
        private int selectedTileSet;
        private GameObject selectedTile = null;
        private int lastKey = -1;
        //tile layer window
        private bool showTileLayerWindow = true;
        private const int tileLayerWindowId = 1 << 0;
        private const float tileLayerWindowWidth = 220;
        private const float tileLayerWindowHeight = 120;
        private Rect tileLayerWindowRect;
        //tile window
        private const int tileWindowId = 1 << 1;
        private bool showTileWindow = true;
        private const int tileWindowWidth = 220;
        private const int tileWindowHeight = 220;
        private const int minimizedWindowWidth = 80;
        private Rect tileWindowRect;
        private TileSelectMode tileSelectMode = TileSelectMode.None;
        //tile layer orer list;
        private ReorderableList tileLayerList;
        //collision info
        private ColliderInfo colliderInfo = null;
        #endregion

        #region Properties
        private List<TileData> TileDataList
        {
            get
            {
                if (selectedTileSet < 0 || selectedTileSet >= tileSetManager.TileSets.Count)
                {
                    return null;
                }
                List<TileData> tileList = tileSetManager.TileSets[selectedTileSet].TileList;

                if (tileList == null)
                {
                    tileList = new List<TileData>();
                    tileSetManager.TileSets[selectedTileSet].TileList = tileList;
                }

                return tileList;
            }
        }
        #endregion

        #region Init & Destroy Methods
        void OnEnable()
        {
            init();
        }

        private void init()
        {
            LoadTileSetManager();
            m_Object = new SerializedObject(target);
            newMapSize = m_Object.FindProperty("NewMapSize");
            previewSize = m_Object.FindProperty("PreviewSize");
            selectedLayer = m_Object.FindProperty("SelectedLayerIndex");
            showEdit = m_Object.FindProperty("ShowEdit");
            m_Object.Update();
            previewSize.floatValue = Mathf.Clamp(previewSize.floatValue, minPreviewSize, maxPreviewSize);
            m_Object.ApplyModifiedProperties();
            tileMap = (TileMap)target;
            tileMap.NewMapSize = tileMap.MapSize;
            savedTool = Tools.current;
            showTileWindow = true;
            showTileWindow = true;
            tileSelectMode = TileSelectMode.TileSet;
            skin = AssetDatabase.LoadAssetAtPath("Assets/Tile Editor/GUI/Editor Skin.guiskin", typeof(GUISkin)) as GUISkin;
            Undo.undoRedoPerformed += OnUndoRedo;
            initTileLayerList();
        }

        private void initTileLayerList()
        {
            tileLayerList = new ReorderableList(m_Object, m_Object.FindProperty("TileLayers"), true, true, true, true);
            //draw header
            tileLayerList.drawHeaderCallback = (Rect rect) =>
            {
                EditorGUI.LabelField(rect, "Tile Layers");
            };

            //draw layers
            tileLayerList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                if (index < 0 || index >= tileLayerList.serializedProperty.arraySize)
                {
                    return;
                }
                rect.y += 2;
                SerializedProperty item = tileLayerList.serializedProperty.GetArrayElementAtIndex(index);
                TileLayer tileLayer = item.objectReferenceValue as TileLayer;
                if (tileLayer == null)
                {
                    return;
                }
                Undo.RecordObject(tileLayer.gameObject, null);
                int toggleButtonW = 20;
                bool active = tileLayer.gameObject.activeSelf;
                tileLayer.gameObject.SetActive(EditorGUI.Toggle(new Rect(rect.x, rect.y, toggleButtonW, EditorGUIUtility.singleLineHeight), GUIContent.none, active));

                rect.x += toggleButtonW;
                tileLayer.gameObject.name = EditorGUI.TextField(new Rect(rect.x, rect.y, (int)(0.6f * rect.width), EditorGUIUtility.singleLineHeight), tileLayer.gameObject.name);
            };

            //remove layers
            tileLayerList.onRemoveCallback = (ReorderableList list) =>
            {
                int index = list.index;
                if (index < 0 || index >= list.count)
                {
                    return;
                }
                //
                if (selection.hasSelection && selection.selectingState == Selection.SelectionState.Selected && selection.selectionToolMode == Selection.SelectionToolMode.Move)
                {
                    ApplyMovedTiles();
                    selection.selectionToolMode = Selection.SelectionToolMode.None;
                }

                //
                SerializedProperty item = list.serializedProperty.GetArrayElementAtIndex(index);
                TileLayer tileLayer = item.objectReferenceValue as TileLayer;
                if (tileLayer != null)
                {
                    GameObject go = tileLayer.gameObject;
                    EditorApplication.delayCall += () => Undo.DestroyObjectImmediate(go);
                }
                for (int i = index; i < list.serializedProperty.arraySize - 1; ++i)
                {
                    list.serializedProperty.GetArrayElementAtIndex(i).objectReferenceValue = list.serializedProperty.GetArrayElementAtIndex(i + 1).objectReferenceValue;
                }

                list.serializedProperty.arraySize--;
                if (list.serializedProperty.arraySize > 0)
                {
                    list.index = Mathf.Clamp(list.index, 0, list.serializedProperty.arraySize - 1);
                }
            };

            //add tile layer
            tileLayerList.onAddCallback = (ReorderableList list) =>
            {
                int index = list.serializedProperty.arraySize;
                list.serializedProperty.arraySize++;
                list.index = index;
                SerializedProperty item = list.serializedProperty.GetArrayElementAtIndex(index);
                TileLayer newLayer = TileEditorUtility.CreateLayer(tileMap);
                newLayer.gameObject.hideFlags = tileMap.ShowLayer ? HideFlags.None : HideFlags.HideInHierarchy;
                Undo.RegisterCreatedObjectUndo(newLayer.gameObject, null);
                item.objectReferenceValue = newLayer;
            };


            tileLayerList.onReorderCallback = (ReorderableList list) =>
            {
                int index = list.index;
                if (index < 0 || index >= list.serializedProperty.arraySize)
                {
                    return;
                }
                selectedLayer.intValue = index;
            };
        }

        private void LoadTileSetManager()
        {
            tileSetManager = AssetDatabase.LoadAssetAtPath(tileSetManagerPath, typeof(TileSetManager)) as TileSetManager;
            if (tileSetManager == null)
            {
                string path = Application.dataPath + "/" + tileSetManagerDir;
                Directory.CreateDirectory(path);
                tileSetManager = CreateInstance<TileSetManager>();
                AssetDatabase.CreateAsset(tileSetManager, tileSetManagerPath);
                AssetDatabase.SaveAssets();
            }

            tileSetManager.ClearDeletedTextures();
        }

        void OnUndoRedo()
        {
            Repaint();
        }

        void OnDisable()
        {

            Tools.current = savedTool;
            Undo.undoRedoPerformed -= OnUndoRedo;
            tileSetManager.ClearTextures();
            tileSetManager.tileDataInScene = null;
            ResetSelect();

        }
        #endregion

        #region InspectorGUI Methods
        public override void OnInspectorGUI()
        {
            if (showEdit.boolValue)
            {
                Tools.current = Tool.None;
            }
            else
            {
                Tools.current = savedTool;
            }

            m_Object.Update();
            HandleGUIKeyboardEvent();
            OnMapInfoGUI();
            showEdit.boolValue = EditorGUILayout.Foldout(showEdit.boolValue, "Edit");
            if (showEdit.boolValue)
            {
                OnTileLayerGUI();
                OnDrawTilesetGUI();
            }
            m_Object.ApplyModifiedProperties();
            if (GUI.changed)
            {
                EditorUtility.SetDirty(tileSetManager);
                EditorUtility.SetDirty(target);
                Repaint();
            }
            GUI.skin = null;
        }

        private void HandleGUIKeyboardEvent()
        {
            Event e = Event.current;
            if (e.type == EventType.KeyUp && e.keyCode == KeyCode.Delete)
            {
                if (TileDataList != null && selectedTileIndex < TileDataList.Count && selectedTileIndex >= 0)
                {
                    Undo.RecordObject(tileSetManager, null);
                    DestroyImmediate(TileDataList[selectedTileIndex].Texture);
                    TileDataList.RemoveAt(selectedTileIndex);
                }
                selectedTileIndex = -1;
                Repaint();
            }
        }

        private void OnMapInfoGUI()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
            int w = EditorGUILayout.IntField("Map Width", (int)newMapSize.vector2Value.x);
            int h = EditorGUILayout.IntField("Map Height", (int)newMapSize.vector2Value.y);
            EditorGUILayout.EndVertical();
            w = Mathf.Max(w, 1);
            h = Mathf.Max(h, 1);

            newMapSize.vector2Value = new Vector2(w, h);

            if (GUILayout.Button("Resize", GUILayout.Height(2 * EditorGUIUtility.singleLineHeight)))
            {
                //Resize Map 
                ResetSelect();
                ResizeMap(newMapSize.vector2Value);
            }
            GUILayout.Space(10);
            EditorGUILayout.EndHorizontal();
        }

        public void ResizeMap(Vector2 newSize)
        {
            if (tileMap.TileLayers == null)
            {
                return;
            }

            Undo.RecordObject(tileMap, null);

            foreach (TileLayer layer in tileMap.TileLayers)
            {
                if (layer == null)
                {
                    continue;
                }

                Tile[] newTiles = new Tile[(int)(newSize.x * newSize.y)];

                for (int x = 0; x < tileMap.MapSize.x; ++x)
                {
                    for (int y = 0; y < tileMap.MapSize.y; ++y)
                    {
                        if (x < newSize.x && y < newSize.y)
                        {
                            newTiles[(int)(x + y * newSize.x)] = layer.Tiles[(int)(x + y * tileMap.MapSize.x)];
                        }
                        else
                        {

                            Tile tile = layer.Tiles[(int)(x + y * tileMap.MapSize.x)];
                            if (tile != null)
                            {
                                EditorApplication.delayCall += () => Undo.DestroyObjectImmediate(tile.gameObject);
                            }
                        }
                    }
                }
                Undo.RecordObject(layer, null);

                layer.Tiles = newTiles;
            }
            tileMap.MapSize = newSize;

        }

        private void OnTileLayerGUI()
        {


            tileLayerList.DoLayoutList();

            bool savedFlag = tileMap.ShowLayer;
            tileMap.ShowLayer = EditorGUILayout.Toggle("Show Layer", tileMap.ShowLayer);

            if (savedFlag != tileMap.ShowLayer)
            {
                ChangeLayerVisiblity();
            }

            if (tileMap.ShowLayer)
            {
                EditorGUILayout.HelpBox("Do not delete or move any tile directly in Hierarchy.", MessageType.Info);
            }

            EditorGUILayout.Space();
        }

        void ChangeLayerVisiblity()
        {

            HideFlags flags = tileMap.ShowLayer ? HideFlags.None : HideFlags.HideInHierarchy;

            foreach (TileLayer tileLayer in tileMap.TileLayers)
            {
                tileLayer.gameObject.hideFlags = flags;
            }
        }

        #endregion

        #region SceneView Methods

        void OnSceneGUI()
        {
            Event e = Event.current;

            m_Object.ApplyModifiedProperties();
            DrawWindowsInScene();
            HandleSceneKeyboardEvent(e);
            DrawGrids();
            if (showEdit.boolValue)
            {
                DrawToolBarInSceneView();
                HandleControlClick(e);
                HandleMiddleMouse(e);
                HandleEditTools(e);
            }
            else
            {
                if (e.button == 0)
                {
                    HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
                }
            }

            RepaintScene();
        }

        private void DrawWindowsInScene()
        {
            if (showEdit.boolValue)
            {
                if (tileLayerWindowRect.width <= 0)
                {
                    tileLayerWindowRect = new Rect(Camera.current.pixelWidth - tileLayerWindowWidth, (Camera.current.pixelHeight - tileLayerWindowHeight) / 2, tileLayerWindowWidth, tileLayerWindowHeight);
                }
                if (tileWindowRect.width <= 0)
                {
                    tileWindowRect = new Rect(Camera.current.pixelWidth - tileWindowWidth, Camera.current.pixelHeight - tileWindowHeight, tileWindowWidth, tileWindowHeight);
                }
                DrawTileLayerWindow();
                DrawTileWindow();
            }
        }

        private void DrawTileLayerWindow()
        {
            tileLayerWindowRect.x = Camera.current.pixelWidth - tileLayerWindowRect.width;
            tileLayerWindowRect.y = Mathf.Clamp(tileLayerWindowRect.y, 0, tileWindowRect.y - tileLayerWindowRect.height);
            tileLayerWindowRect = GUI.Window(tileLayerWindowId, tileLayerWindowRect, DoTileLayerWindow, GUIContent.none);
        }

        private void DoTileLayerWindow(int windowID)
        {
            showTileLayerWindow = EditorGUI.Foldout(new Rect(0, 0, 10, EditorGUIUtility.singleLineHeight), showTileLayerWindow, new GUIContent("Layers"));
            if (!showTileLayerWindow)
            {
                tileLayerWindowRect.width = minimizedWindowWidth;
                tileLayerWindowRect.height = EditorGUIUtility.singleLineHeight;
            }
            else
            {
                tileLayerWindowRect.width = tileLayerWindowWidth;
                tileLayerWindowRect.height = tileLayerWindowHeight;
            }

            if (!showTileLayerWindow)
            {
                GUI.DragWindow();
                return;
            }

            if (tileLayerList.serializedProperty.arraySize <= 0)
            {
                EditorGUILayout.HelpBox("You need to add a layer first before place a tile", MessageType.Info);
            }

            List<TileLayer> layers = GetAllTileLayers();

            string[] names = new string[layers.Count];

            for (int i = 0; i < names.Length; ++i)
            {
                names[i] = (i + 1) + ". " + layers[i].gameObject.name;
            }

            if (tileLayerList.serializedProperty.arraySize > 0)
            {
                GUILayout.BeginVertical();
                int lastSelectedLayer = selectedLayer.intValue;
                selectedLayer.intValue = EditorGUILayout.Popup("Tile Layer", selectedLayer.intValue, names, EditorStyles.popup);
                selectedLayer.intValue = Mathf.Clamp(selectedLayer.intValue, 0, tileLayerList.serializedProperty.arraySize - 1);
                if (selectedLayer.intValue != lastSelectedLayer && currentEditMode == (int)EditMode.Select)
                {
                    RevertMovingTiles();
                }
                TileLayer tileLayer = GetTileLayerAtIndex(selectedLayer.intValue);
                if (tileLayer == null)
                {
                    GUI.DragWindow();
                    return;
                }
                //layer mask
                string[] layerNames = TileEditorUtility.GetLayerNames();
                string layerName = LayerMask.LayerToName(tileLayer.PhysicsLayer);
                int i = 0;
                for (i = 0; i < layerNames.Length; ++i)
                {
                    if (layerNames[i] == layerName)
                    {
                        break;
                    }
                }

                int layerIndex = EditorGUILayout.Popup("Physics Layer", i, layerNames);
                if (layerIndex != i)
                {
                    tileLayer.PhysicsLayer = LayerMask.NameToLayer(layerNames[layerIndex]);
                }

                //sorting layer
                string[] sortingLayerNames = TileEditorUtility.GetSortingLayerNames();
                i = TileEditorUtility.GetSortingLayerID(tileLayer.SortingLayer);
                int index = EditorGUILayout.Popup("Sorting Layer", i, sortingLayerNames);
                if (i != index)
                {
                    tileLayer.SortingLayer = sortingLayerNames[index];
                }
                //z

                float z = EditorGUILayout.FloatField("Z", tileLayer.Z);
                if (z != tileLayer.Z)
                {
                    tileLayer.Z = z;
                }

                //alpha
                int alpha = EditorGUILayout.IntField("Alpha", tileLayer.Alpha);
                if (alpha != tileLayer.Alpha)
                {
                    tileLayer.Alpha = alpha;
                }

                GUILayout.EndVertical();
            }

            GUI.DragWindow();
        }

        private void DrawTileWindow()
        {
            tileWindowRect.x = Camera.current.pixelWidth - tileWindowRect.width;
            tileWindowRect.y = Mathf.Clamp(tileWindowRect.y, tileLayerWindowRect.y + tileLayerWindowRect.height, Camera.current.pixelHeight);
            tileWindowRect = GUI.Window(tileWindowId, tileWindowRect, DoTileWindow, GUIContent.none);
        }

        void DoTileWindow(int windowID)
        {
            showTileWindow = EditorGUI.Foldout(new Rect(0, 0, 10, EditorGUIUtility.singleLineHeight), showTileWindow, new GUIContent("Tile"));
            if (!showTileWindow)
            {
                tileWindowRect.width = minimizedWindowWidth;
            }
            else
            {
                tileWindowRect.width = tileWindowWidth;
                tileWindowRect.height = tileWindowHeight;
            }

            TileData data = GetSelectedTileData();
            if (data != null)
            {
                if (data.Texture == null)
                {
                    tileWindowRect.height = EditorGUIUtility.singleLineHeight;
                    GUI.DragWindow();
                    return;
                }
                if (!showTileWindow)
                {
                    tileWindowRect.height = EditorGUIUtility.singleLineHeight + 60;
                }
                Undo.RecordObject(tileSetManager, null);
                GUILayout.BeginVertical();
                int boxWidth = 50;
                int w = data.Texture.width;
                int h = data.Texture.height;
                float boxHeight = 1.0f * h / w * boxWidth;
                if (boxHeight > boxWidth)
                {
                    boxHeight = boxWidth;
                }
                //Draw Texture
                Rect rect = GUILayoutUtility.GetRect(boxWidth, boxHeight);
                rect = new Rect(5, EditorGUIUtility.singleLineHeight + 5, boxWidth, boxHeight);
                GUI.Box(rect, GUIContent.none);
                rect = new Rect(rect.x + 1, rect.y + 1, boxWidth - 2, boxHeight - 2);


                GUI.DrawTexture(rect, data.Texture);
                //
                if (!showTileWindow)
                {
                    GUI.DragWindow();
                    return;
                }
                //
                data.OrderInLayer = EditorGUILayout.IntField("Order In Layer", data.OrderInLayer);
                data.FlipHorizontally = EditorGUILayout.Toggle("Flip Horizontally", data.FlipHorizontally);
                data.FlipVertically = EditorGUILayout.Toggle("Flip Vertically", data.FlipVertically);
                data.Rotation = EditorGUILayout.FloatField(new GUIContent("Rotation"), data.Rotation);
                data.Rotation = Mathf.Clamp(data.Rotation, 0, 360);
                data.PhysicsMaterial = EditorGUILayout.ObjectField("Physics Material", data.PhysicsMaterial, typeof(PhysicsMaterial2D), false) as PhysicsMaterial2D;
                data.Collision = (CollisionType)EditorGUILayout.EnumPopup("Collision Type", data.Collision);
                data.IsTrigger = GUILayout.Toggle(data.IsTrigger, "Is Trigger");
                //tag
                string[] tags = TileEditorUtility.GetTags();
                int index = 0;
                for (int i = 0; i < tags.Length; ++i)
                {
                    if (tags[i] == data.Tag)
                    {
                        index = i;
                        break;
                    }
                }
                index = EditorGUILayout.Popup("Tag", index, tags);
                if (index >= 0 && index < tags.Length)
                {
                    data.Tag = tags[index];
                }
                GUILayout.EndVertical();
            }
            else
            {
                tileWindowRect.height = EditorGUIUtility.singleLineHeight;
            }
            GUI.DragWindow();
        }

        private void RepaintScene()
        {
            Vector2 mousePosition = Event.current.mousePosition;
            mousePosition = TileEditorUtility.GetMousePosInWorld(mousePosition);
            if (tileMap.IsInTileMap(mousePosition))
            {
                SceneView.RepaintAll();
            }
        }

        private void HandleSceneKeyboardEvent(Event e)
        {
            if (e.type == EventType.KeyDown)
            {
                if (e.keyCode == KeyCode.E)
                {
                    e.Use();
                }
            }
            bool lastEditModeIsSelect = (currentEditMode == (int)EditMode.Select);

            if (e.type == EventType.KeyUp)
            {
                if (e.keyCode == KeyCode.E)
                {
                    currentEditMode = (int)EditMode.Erase;
                }

                if (e.keyCode == KeyCode.S)
                {
                    int lastEditMode = currentEditMode;
                    currentEditMode = (int)EditMode.Select;
                    if (lastEditMode != currentEditMode)
                    {
                        ResetSelect();
                    }
                }
                if (e.keyCode == KeyCode.B)
                {
                    currentEditMode = (int)EditMode.Paint;
                }
                if (e.keyCode == KeyCode.M)
                {
                    currentEditMode = (int)EditMode.Move;
                }
            }

            if (lastEditModeIsSelect && currentEditMode != (int)EditMode.Select)
            {
                ResetSelect();
            }

        }

        private void HandleMiddleMouse(Event e)
        {

            if (e.button == 2)
            {

                int controlID = GUIUtility.GetControlID(GetHashCode(), FocusType.Passive);

                if (e.type == EventType.MouseDown)
                {
                    lastEditMode = currentEditMode;
                    currentEditMode = (int)EditMode.Erase;
                    GUIUtility.hotControl = GUIUtility.keyboardControl = controlID;
                    e.Use();
                }
                else if (e.type == EventType.MouseUp)
                {
                    GUIUtility.hotControl = GUIUtility.keyboardControl = 0;
                    currentEditMode = lastEditMode;
                }
            }

        }

        private void DrawToolBarInSceneView()
        {
            try
            {
                Handles.BeginGUI();
                GUILayout.BeginArea(new Rect(10, 10, 430, 20));
                GUILayout.BeginHorizontal();
                int lastEditMode = currentEditMode;
                currentEditMode = GUILayout.Toolbar(currentEditMode, new GUIContent[] {
              new GUIContent(EditMode.Paint.ToString()+"(B)"),
                 new GUIContent(EditMode.Move.ToString()+"(M)"),
               new GUIContent(EditMode.Select.ToString()+"(S)"),
                new GUIContent(EditMode.Erase.ToString()+"(E)"),
            });

                if (lastEditMode != currentEditMode && lastEditMode == (int)EditMode.Select)
                {
                    selection.Reset();
                }

                GUILayout.EndHorizontal();
                GUILayout.EndArea();

                if (currentEditMode == (int)EditMode.Select)
                {
                    GUILayout.BeginArea(new Rect(10, 40, 120, 4 * (EditorGUIUtility.singleLineHeight + 3)), GUI.skin.box);
                    GUILayout.Label(new GUIContent("D -> Fill"));
                    GUILayout.Label(new GUIContent("G -> Translate"));
                    GUILayout.Label(new GUIContent("C -> Clear"));
                    GUILayout.Label(new GUIContent("Space -> Copy"));
                    GUILayout.EndArea();
                }

                Handles.EndGUI();
            }
            catch
            {
                // do nothing
            }
        }

        private void DrawGrids()
        {
            TileEditorUtility.DrawGrids(tileMap.transform.position, tileMap.MapSize, tileMap.TileSize, Color.gray);
        }
        #endregion

        #region Tileset GUI

        private void OnDrawTilesetGUI()
        {

            GUILayout.BeginVertical();

            string[] names = new string[tileSetManager.TileSets.Count];
            for (int i = 0; i < names.Length; ++i)
            {

                string name = tileSetManager.TileSets[i].Name;
                if (name == null)
                {
                    name = "Set";
                    tileSetManager.TileSets[i].Name = name;
                }
                names[i] = "#" + i + " " + name;
            }
            GUILayout.BeginHorizontal();
            GUILayout.Label("Tile Set: ");

            selectedTileSet = EditorGUILayout.Popup(selectedTileSet, names);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("New"))
            {
                AddTileSet();
            }

            bool oldEnabled = GUI.enabled;
            if (tileSetManager.TileSets.Count <= 0 || selectedTileSet >= tileSetManager.TileSets.Count || selectedTileSet < 0)
            {
                GUI.enabled = false;
            }

            if (GUILayout.Button("Delete"))
            {
                foreach (TileData tile in TileDataList)
                {
                    if (tile.Texture)
                    {
                        DestroyImmediate(tile.Texture);
                    }
                }
                RemoveTileSet(selectedTileSet);
            }

            if (GUILayout.Button("Rename"))
            {
                TileSetNameWizard wizard = ScriptableWizard.DisplayWizard<TileSetNameWizard>("Rename", "Apply");
                TileSet set = tileSetManager.TileSets[selectedTileSet];
                wizard.SetManager(tileSetManager);
                wizard.SetTileSet(set);
            }

            GUI.enabled = oldEnabled;
            GUILayout.EndHorizontal();
            OnTileGUI();
            GUILayout.EndVertical();
        }

        private void OnTileGUI()
        {
            GUILayout.BeginVertical(GUI.skin.box);
            DrawTiles();
            GUILayout.EndVertical();
            //
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Preview Size", GUILayout.ExpandWidth(false));
            previewSize.floatValue = GUILayout.HorizontalSlider(previewSize.floatValue, minPreviewSize, maxPreviewSize, GUILayout.ExpandWidth(true));
            GUILayout.Space(10);
            EditorGUILayout.EndHorizontal();
        }

        private void DrawTiles()
        {
            Event e = Event.current;
            float gap = 2;
            float tileWidth = previewSize.floatValue;
            int colCount = (int)((Screen.width - 40) / (tileWidth + gap));
            int rowCount = 0;
            if (TileDataList != null)
            {
                rowCount = TileDataList.Count / colCount;
                if (rowCount * colCount < TileDataList.Count)
                {
                    rowCount += 1;
                }
            }
            float ratio = tileMap.TileSize.x / tileMap.TileSize.y;
            float w = tileWidth;
            float h = tileWidth / ratio;
            Rect rect = GUILayoutUtility.GetRect(Screen.width - 40, 300);
            GUI.skin.scrollView = skin.scrollView;
            tileScrollPosition = GUI.BeginScrollView(rect, tileScrollPosition, new Rect(0, 0, rect.width - 20, gap + rowCount * (h + gap)));
            GUI.skin.scrollView = null;

            if (TileDataList == null || TileDataList.Count <= 0)
            {
                GUI.skin.label.normal.textColor = Color.white;
                GUI.Label(new Rect(gap, 0, rect.width, 50), "Drag and drop sprites here");
                GUI.skin.label.normal.textColor = Color.black;
            }

            HandleDragAndDrop(e);

            if (TileDataList != null)
            {
                GUI.skin = skin;
                float x = gap;
                float y = gap;

                for (int row = 0; row < rowCount; ++row)
                {
                    x = gap;
                    GUILayout.BeginHorizontal();
                    for (int col = 0; col < colCount; ++col)
                    {
                        int index = col + row * colCount;
                        if (index >= TileDataList.Count)
                        {
                            break;
                        }

                        Texture2D texture = TileDataList[index].Texture;

                        if (!texture)
                        {
                            continue;
                        }

                        GUIStyle style = GUI.skin.customStyles[0];
                        if (index == selectedTileIndex && tileSelectMode == TileSelectMode.TileSet)
                        {
                            style = GUI.skin.button;
                        }

                        Rect r = new Rect(x, y, w, h);
                        x += (w + gap);
                        //bg
                        GUI.Box(r, GUIContent.none, GUI.skin.box);
                        GUI.DrawTexture(r, texture);
                        if (GUI.Button(r, GUIContent.none, style))
                        {
                            tileSelectMode = TileSelectMode.TileSet;
                            selectedTileIndex = index;
                        }

                    }
                    GUILayout.EndHorizontal();
                    y += (h + gap);
                }

                GUI.skin = null;
            }


            GUI.EndScrollView();
        }

        private void HandleDragAndDrop(Event e)
        {
            switch (e.type)
            {
                case EventType.DragUpdated:
                case EventType.DragPerform:
                    {
                        DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                        if (e.type == EventType.DragPerform)
                        {
                            DragAndDrop.AcceptDrag();
                            foreach (var draggedObject in DragAndDrop.objectReferences)
                            {
                                Sprite sprite = draggedObject as Sprite;
                                if (sprite)
                                {
                                    if (!TileSetHasSprite(sprite))
                                    {
                                        Undo.RecordObject(tileSetManager, null);
                                        AddSpriteToTileSet(sprite);
                                    }
                                    continue;
                                }
                                Texture2D tex = draggedObject as Texture2D;
                                List<Sprite> sprites = TileEditorUtility.GetSpritesFromTexture(tex);

                                if (sprites != null)
                                {
                                    foreach (Sprite s in sprites)
                                    {
                                        if (!TileSetHasSprite(s))
                                        {
                                            Undo.RecordObject(tileSetManager, null);
                                            AddSpriteToTileSet(s);
                                        }
                                    }
                                }

                            }
                        }
                        Event.current.Use();
                        break;
                    }
            }
        }

        private bool TileSetHasSprite(Sprite sprite)
        {
            if (sprite == null)
            {
                return true;
            }

            if (TileDataList != null)
            {
                foreach (TileData data in TileDataList)
                {
                    if (data.Sprite == sprite)
                    {
                        return true;
                    }
                }
            }
            else
            {
                return false;
            }

            return false;
        }

        private void AddSpriteToTileSet(Sprite sprite)
        {
            if (sprite == null)
            {
                return;
            }
            if (TileDataList == null)
            {
                AddTileSet();
            }

            TileData data = new TileData();
            data.Sprite = sprite;
            TileDataList.Add(data);
        }

        public void AddTileSet()
        {
            Undo.RecordObject(tileSetManager, null);
            TileSet tileSet = new TileSet();
            tileSetManager.TileSets.Add(tileSet);
            selectedTileSet = tileSetManager.TileSets.Count - 1;
        }

        public void RemoveTileSet(int index)
        {
            Undo.RecordObject(tileSetManager, null);
            tileSetManager.TileSets.RemoveAt(index);
        }

        public TileData GetSelectedTileData()
        {
            if (tileSelectMode == TileSelectMode.Scene)
            {
                return tileSetManager.tileDataInScene;
            }
            else if (tileSelectMode == TileSelectMode.TileSet)
            {
                List<TileData> data = TileDataList;
                if (data == null || selectedTileIndex < 0 || selectedTileIndex >= data.Count)
                {
                    return null;
                }
                return TileDataList[selectedTileIndex];
            }
            return null;
        }
        #endregion

        #region Edit Tools Methods

        private void HandleControlClick(Event e)
        {
            if (e.control && e.button == 0 && (e.type == EventType.MouseDown || e.type == EventType.MouseDrag || e.type == EventType.MouseUp))
            {
                Vector2 mousePosition = TileEditorUtility.GetMousePosInWorld(e.mousePosition);
                if (tileMap.IsInTileMap(mousePosition))
                {
                    Vector2 gridIndex = tileMap.TransformPositionToGridIndex(mousePosition);
                    gridIndex = tileMap.ClampGridIndex(gridIndex);
                    int key = (int)(tileMap.MapSize.x * gridIndex.y + gridIndex.x);

                    if (tileMap.TileLayers != null)
                    {
                        foreach (TileLayer tileLayer in tileMap.TileLayers)
                        {
                            if (key < 0 || key > tileLayer.Tiles.Length)
                            {
                                return;
                            }
                            //get a tileData from a Tile
                            Tile tile = tileLayer.Tiles[key];
                            if (tile != null)
                            {
                                if (tileSetManager.tileDataInScene != null)
                                {
                                    if (tileSetManager.tileDataInScene.Texture != null)
                                    {
                                        DestroyImmediate(tileSetManager.tileDataInScene.Texture);
                                    }
                                }
                                tileSetManager.tileDataInScene = TileEditorUtility.GetTileDataFromTile(tile);
                                tileSelectMode = TileSelectMode.Scene;
                                //get collision info
                                colliderInfo = TileEditorUtility.GetColliderInfoFromTile(tile);
                                Repaint();
                                break;
                            }
                        }
                    }

                }
                e.Use();
            }
        }

        private void HandleEditTools(Event e)
        {

            if ((EditMode)currentEditMode == EditMode.Move)//move
            {
                HandleMoveTool(e);
                DrawMouseRect(e);
            }
            else if ((EditMode)currentEditMode == EditMode.Paint)//paint 
            {
                HandlePaintTool(e);
                DrawMouseRect(e);
            }
            else if ((EditMode)currentEditMode == EditMode.Erase)//erase
            {
                HandleDeleteTool(e);
                DrawMouseRect(e);
            }
            else if ((EditMode)currentEditMode == EditMode.Select)//select and fill
            {
                HandleSelectTool(e);
            }
        }

        private void HandlePaintTool(Event e)
        {
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
            Vector2 mousePosition = TileEditorUtility.GetMousePosInWorld(e.mousePosition);
            TileLayer tileLayer = GetSelectedTileLayer();
            TileData tileData = GetSelectedTileData();
            if (tileLayer == null || tileData == null || tileData.Sprite == null)
            {
                return;
            }

            if (e.button == 0 && (e.type == EventType.MouseDrag || e.type == EventType.MouseDown))
            {

                if (tileMap.IsInTileMap(mousePosition))
                {
                    Vector2 gridIndex = tileMap.TransformPositionToGridIndex(mousePosition);
                    gridIndex = tileMap.ClampGridIndex(gridIndex);
                    Vector2 pos = tileMap.GetGridIndexPosInWorldSpace(gridIndex);
                    int key = (int)(tileMap.MapSize.x * gridIndex.y + gridIndex.x);

                    if (key < 0 || key > tileLayer.Tiles.Length)
                    {
                        return;
                    }

                    if (tileLayer.Tiles[key] != null)
                    {
                        Tile tile = tileLayer.Tiles[key];
                        if (TileEditorUtility.IsTheSameTile(tile, tileData))
                        {
                            if (tileSelectMode == TileSelectMode.Scene)
                            {
                                if (TileEditorUtility.IsTileColliderEqualsToColliderInfo(tile, colliderInfo))
                                {
                                    return;
                                }
                            }
                            else
                            {
                                return;
                            }
                        }
                        Undo.RecordObject(tileLayer, null);
                        EditorApplication.delayCall += () => Undo.DestroyObjectImmediate(tile.gameObject);
                    }

                    {
                        GameObject go = TileEditorUtility.CreateTile(new Vector3(pos.x, pos.y, tileLayer.transform.position.z), tileData, tileLayer.Alpha / 255.0f, tileLayer.SortingLayer);
                        if (tileData.Tag != null)
                        {
                            go.tag = tileData.Tag;
                        }
                        go.layer = tileLayer.PhysicsLayer;
                        go.name = "Tile_" + gridIndex.x + "_" + gridIndex.y;
                        go.transform.parent = tileLayer.transform;
                        Tile tile = SetupTile(go, tileData);
                        if (tileSelectMode == TileSelectMode.Scene && colliderInfo != null)
                        {
                            TileEditorUtility.ApplyColliderInfoToTile(tile, colliderInfo);
                        }
                        Undo.RecordObject(tileLayer, null);
                        tileLayer.Tiles[key] = tile;
                        Undo.RegisterCreatedObjectUndo(go, "Create Tile");
                    }
                }
            }
        }

        private void HandleDeleteTool(Event e)
        {
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
            TileLayer tileLayer = GetSelectedTileLayer();
            if (tileLayer == null)
            {
                return;
            }

            if ((e.button == 0 || e.button == 2) && (e.type == EventType.MouseDrag || e.type == EventType.MouseDown))
            {

                Vector2 mousePosition = TileEditorUtility.GetMousePosInWorld(e.mousePosition);
                if (tileMap.IsInTileMap(mousePosition))
                {
                    Vector2 gridIndex = tileMap.TransformPositionToGridIndex(mousePosition);
                    gridIndex = tileMap.ClampGridIndex(gridIndex);
                    int key = (int)((int)tileMap.MapSize.x * (int)gridIndex.y + (int)gridIndex.x);
                    if (key < 0 || key > tileLayer.Tiles.Length)
                    {
                        return;
                    }
                    if (tileLayer.Tiles[key] != null)
                    {
                        GameObject go = tileLayer.Tiles[key].gameObject;
                        Undo.RecordObject(tileLayer, null);
                        EditorApplication.delayCall += () => Undo.DestroyObjectImmediate(go);
                        tileLayer.Tiles[key] = null;
                    }
                }
            }
        }

        private void HandleMoveTool(Event e)
        {
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
            Vector2 mousePosition = TileEditorUtility.GetMousePosInWorld(e.mousePosition);
            TileLayer tileLayer = GetSelectedTileLayer();
            if (tileLayer == null)
            {
                return;
            }
            if (e.type == EventType.MouseDown && e.button == 0 && tileMap.IsInTileMap(mousePosition))
            {

                Vector2 gridIndex = tileMap.TransformPositionToGridIndex(mousePosition);
                gridIndex = tileMap.ClampGridIndex(gridIndex);
                int key = (int)(tileMap.MapSize.x * gridIndex.y + gridIndex.x);
                selectedTile = null;
                if (tileLayer.Tiles[key] != null)
                {
                    selectedTile = tileLayer.Tiles[key].gameObject;
                    lastKey = key;
                }
                else
                {
                    lastKey = -1;
                }
                return;
            }
            if (e.type == EventType.MouseDrag && e.button == 0 && selectedTile != null)
            {
                Vector2 gridIndex = tileMap.TransformPositionToGridIndex(mousePosition);
                gridIndex = tileMap.ClampGridIndex(gridIndex);
                Vector2 pos = tileMap.GetGridIndexPosInWorldSpace(gridIndex);

                if (lastKey != -1 && selectedTile != null)
                {
                    Undo.RecordObject(selectedTile.GetComponent(typeof(Transform)) as Transform, "Move");
                    selectedTile.transform.position = new Vector3(pos.x, pos.y, 0);
                }
            }
            if ((e.type == EventType.MouseUp || e.rawType == EventType.MouseUp) && e.button == 0)
            {
                if (lastKey != -1 && selectedTile != null)
                {

                    Vector2 gridIndex = tileMap.TransformPositionToGridIndex(mousePosition);
                    gridIndex = tileMap.ClampGridIndex(gridIndex);
                    Vector2 pos = tileMap.GetGridIndexPosInWorldSpace(gridIndex);
                    int key = (int)((int)tileMap.MapSize.x * (int)gridIndex.y + (int)gridIndex.x);
                    if (key == lastKey)
                    {
                        return;
                    }
                    Undo.RecordObject(tileLayer, null);
                    if (tileLayer.Tiles[key] != null)
                    {
                        GameObject go = tileLayer.Tiles[key].gameObject;
                        EditorApplication.delayCall += () => Undo.DestroyObjectImmediate(go);
                    }
                    selectedTile.transform.position = new Vector3(pos.x, pos.y, 0);
                    tileLayer.Tiles[key] = tileLayer.Tiles[lastKey];
                    tileLayer.Tiles[lastKey] = null;
                }
                selectedTile = null;
                lastKey = -1;
            }
        }

        #region Selection Tool Stuff

        private class Selection
        {
            public enum SelectionToolMode
            {
                None,
                Move,
                Fill,
                Copy,
                Clear
            }
            public enum SelectionState
            {
                NotSelecting,
                Selecting,
                Selected
            }
            public SelectionState selectingState = SelectionState.NotSelecting;
            public Vector2 startPoint = Vector2.zero;
            public Vector2 endPoint = Vector2.zero;
            public bool hasSelection = false;
            public SelectionToolMode selectionToolMode = SelectionToolMode.None;
            public TileLayer savedTileLayer = null;
            public GameObject[] copyBuffer = null;
            public int bufferW = 0;
            public int bufferH = 0;
            public Selection()
            {

            }

            public void Reset()
            {
                copyBuffer = null;
                hasSelection = false;
                selectionToolMode = SelectionToolMode.None;
            }

            public Vector2 GetSelectionCenterIndex()
            {
                int x = (int)(0.5f * (GetBottomLeft().x + GetTopRight().x));
                int y = (int)(0.5f * (GetBottomLeft().y + GetTopRight().y));
                return new Vector2(x, y);
            }

            public Vector2 GetBottomLeft()
            {
                if (!hasSelection)
                {
                    return new Vector2(-1, -1);
                }
                float x = Mathf.Min(startPoint.x, endPoint.x);
                float y = Mathf.Min(startPoint.y, endPoint.y);
                return new Vector2(x, y);
            }

            public Vector2 GetTopRight()
            {
                if (!hasSelection)
                {
                    return new Vector2(-1, -1);
                }
                float x = Mathf.Max(startPoint.x, endPoint.x);
                float y = Mathf.Max(startPoint.y, endPoint.y);
                return new Vector2(x, y);
            }

            public void DeleteCopyBuffer()
            {
                if (copyBuffer == null)
                {
                    return;
                }
                for (int i = 0; i < copyBuffer.Length; ++i)
                {
                    DestroyImmediate(copyBuffer[i]);
                }
                copyBuffer = null;
            }
        }
        private delegate void HandleTile(TileLayer tileLayer, int x, int y);
        private readonly Selection selection = new Selection();

        #endregion
        private void HandleSelectTool(Event e)
        {
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
            Vector2 mousePosition = TileEditorUtility.GetMousePosInWorld(e.mousePosition);
            TileLayer tileLayer = GetSelectedTileLayer();
            TileData tileData = GetSelectedTileData();
            if (tileLayer == null || tileData == null || tileData.Sprite == null)
            {
                DrawMouseRect(e);
                return;
            }

            Vector2 origin = new Vector2(tileMap.transform.position.x, tileMap.transform.position.y);
            bool inGrid = tileMap.IsInTileMap(mousePosition);
            Vector2 minPos = Vector2.zero;
            Vector2 tileSize = tileMap.TileSize;
            Vector2 positionInGrid = tileMap.TransformPositionToGridIndex(mousePosition);
            Vector2 drawSize = tileSize;

            //selection tools 
            if (selection.hasSelection && selection.selectingState == Selection.SelectionState.Selected)
            {
                if (e.button == 0 && e.type == EventType.MouseUp)
                {
                    if (selection.selectionToolMode == Selection.SelectionToolMode.Move)
                    {
                        //apply translating
                        ApplyMovedTiles();
                        Vector2 offset = CalculateMoveOffset(e);
                        selection.startPoint += offset;
                        selection.endPoint += offset;
                        selection.selectionToolMode = Selection.SelectionToolMode.None;
                    }

                    if (selection.selectionToolMode == Selection.SelectionToolMode.Copy)
                    {
                        //apply pasting
                        ApplyCopyBuffer(e);
                        Vector2 offset = CalculateMoveOffset(e);
                        selection.startPoint += offset;
                        selection.endPoint += offset;
                        selection.selectionToolMode = Selection.SelectionToolMode.None;
                    }
                }

                if (e.type == EventType.KeyUp)
                {
                    //fill, translate, remove and copy is not allowed when copying
                    if (selection.selectionToolMode != Selection.SelectionToolMode.Copy)
                    {
                        if (e.keyCode == KeyCode.D)
                        {
                            SetupSelectedTiles(HandleSelectedTilesFilling);
                            if (selection.selectionToolMode != Selection.SelectionToolMode.Move)
                            {
                                selection.selectionToolMode = Selection.SelectionToolMode.None;
                            }
                            else
                            {
                                MoveTiles(e);
                            }

                        }
                        else if (e.keyCode == KeyCode.C)
                        {
                            SetupSelectedTiles(HandleSelectedTilesDeletion);
                            if (selection.selectionToolMode != Selection.SelectionToolMode.Move)
                            {
                                selection.selectionToolMode = Selection.SelectionToolMode.None;
                            }
                            else
                            {
                                MoveTiles(e);
                            }
                            e.Use();
                        }
                        else if (e.keyCode == KeyCode.G)
                        {
                            selection.savedTileLayer = GetSelectedTileLayer();
                            selection.selectionToolMode = Selection.SelectionToolMode.Move;
                        }
                        else if (e.keyCode == KeyCode.Space)
                        {
                            selection.selectionToolMode = Selection.SelectionToolMode.Copy;
                            CopyTiles();
                        }
                    }

                    if (e.keyCode == KeyCode.Escape)
                    {
                        if (selection.selectionToolMode == Selection.SelectionToolMode.Move)
                        {
                            ApplyMovedTiles();
                        }

                        if (selection.selectionToolMode == Selection.SelectionToolMode.Copy)
                        {
                            //delete created tiles
                            selection.DeleteCopyBuffer();
                        }

                        selection.selectionToolMode = Selection.SelectionToolMode.None;
                        selection.hasSelection = false;
                    }
                }
            }

            //Select tiles
            if (e.button == 0 && e.type == EventType.MouseDown)
            {
                if (selection.hasSelection)
                {
                    if (inGrid && selection.selectionToolMode != Selection.SelectionToolMode.Move && selection.selectionToolMode != Selection.SelectionToolMode.Copy)
                    {
                        selection.selectingState = Selection.SelectionState.Selecting;
                        selection.startPoint = positionInGrid;
                    }
                }
                else
                {
                    if (inGrid)
                    {
                        selection.hasSelection = true;
                        selection.selectingState = Selection.SelectionState.Selecting;
                        selection.startPoint = positionInGrid;
                    }
                    else
                    {
                        selection.selectingState = Selection.SelectionState.NotSelecting;
                    }
                }
            }

            //set end point
            if (selection.hasSelection && selection.selectingState == Selection.SelectionState.Selecting)
            {
                selection.endPoint = tileMap.ClampGridIndex(positionInGrid);
            }

            //finish selecting tiles
            if (e.button == 0 && e.type == EventType.MouseUp)
            {
                if (selection.hasSelection && selection.selectingState == Selection.SelectionState.Selecting)
                {
                    selection.selectingState = Selection.SelectionState.Selected;
                    selection.selectionToolMode = Selection.SelectionToolMode.None;
                }

                SceneView.RepaintAll();
            }

            //draw selection rect
            if (selection.hasSelection && (selection.selectingState == Selection.SelectionState.Selected || selection.selectingState == Selection.SelectionState.Selecting))
            {

                Vector2 offset = Vector2.zero;

                if (selection.selectionToolMode == Selection.SelectionToolMode.Move)
                {
                    MoveTiles(e);
                    offset = CalculateMoveOffset(e);
                }

                if (selection.selectionToolMode == Selection.SelectionToolMode.Copy)
                {
                    MoveCopyBuffer(e);
                    offset = CalculateMoveOffset(e);
                }

                minPos.x = Mathf.Min(selection.endPoint.x, selection.startPoint.x);
                minPos.y = Mathf.Min(selection.endPoint.y, selection.startPoint.y);
                drawSize.x = (Mathf.Max(selection.endPoint.x, selection.startPoint.x) - minPos.x + 1) * tileMap.TileSize.x;
                drawSize.y = (Mathf.Max(selection.endPoint.y, selection.startPoint.y) - minPos.y + 1) * tileMap.TileSize.y;
                minPos.x += offset.x;
                minPos.y += offset.y;
                DrawMouseCursor(new Vector2(minPos.x * tileSize.x, minPos.y * tileSize.y) + origin, drawSize);
            }
            else
            {
                if (inGrid)
                {
                    DrawMouseCursor(new Vector2(positionInGrid.x * tileSize.x, positionInGrid.y * tileSize.y) + origin, tileSize);
                }
            }

            //draw info
            if (inGrid)
            {
                DrawMouseGridInfo((int)positionInGrid.x, (int)positionInGrid.y);
            }
        }

        private void ResetSelect()
        {
            if (selection.selectionToolMode == Selection.SelectionToolMode.Move)
            {
                RevertMovingTiles();
            }

            if (selection.selectionToolMode == Selection.SelectionToolMode.Copy)
            {
                selection.DeleteCopyBuffer();
            }

            selection.Reset();
        }

        private void CopyTiles()
        {
            int _w = (int)(selection.GetTopRight().x - selection.GetBottomLeft().x + 1);
            int _h = (int)(selection.GetTopRight().y - selection.GetBottomLeft().y + 1);
            if (_w > 0 && _h > 0)
            {
                int mapWidth = (int)tileMap.MapSize.x;
                int baseX = (int)selection.GetBottomLeft().x;
                int baseY = (int)selection.GetBottomLeft().y;
                selection.copyBuffer = new GameObject[_w * _h];
                selection.bufferW = _w;
                selection.bufferH = _h;

                for (int _y = 0; _y < _h; ++_y)
                {
                    for (int _x = 0; _x < _w; ++_x)
                    {
                        TileLayer tileLayer = GetSelectedTileLayer();
                        Tile tile = tileLayer.Tiles[(baseX + _x) + (baseY + _y) * mapWidth];
                        if (tile != null)
                        {
                            GameObject go = Instantiate(tile.gameObject) as GameObject;
                            go.transform.parent = tileLayer.transform;
                            selection.copyBuffer[_y * _w + _x] = go;
                        }
                    }
                }
            }
        }

        private void RevertMovingTiles()
        {
            if (selection.selectionToolMode == Selection.SelectionToolMode.Copy)
            {
                return;
            }

            selection.selectionToolMode = Selection.SelectionToolMode.None;
            TileLayer tileLayer = selection.savedTileLayer;
            if (tileLayer == null)
            {
                return;
            }
            int startX = (int)selection.GetBottomLeft().x;
            int endX = (int)selection.GetTopRight().x;

            int startY = (int)selection.GetBottomLeft().y;
            int endY = (int)selection.GetTopRight().y;

            for (int y = startY; y <= endY; ++y)
            {
                for (int x = startX; x <= endX; ++x)
                {
                    int key = (int)((int)tileMap.MapSize.x * y + x);
                    if (key < 0 || key > tileLayer.Tiles.Length)
                    {
                        return;
                    }
                    if (tileLayer.Tiles[key] != null)
                    {
                        GameObject go = tileLayer.Tiles[key].gameObject;
                        Vector2 movedPosition = tileMap.GetGridIndexPosInWorldSpace(new Vector2(x, y));
                        go.transform.position = movedPosition;
                    }
                }
            }
        }

        private void MoveTiles(Event e)
        {
            if (!selection.hasSelection || selection.selectingState != Selection.SelectionState.Selected || selection.selectionToolMode != Selection.SelectionToolMode.Move)
            {
                return;
            }

            Vector2 movementOffset = CalculateMoveOffset(e);
            TileLayer tileLayer = GetSelectedTileLayer();
            if (tileLayer == null)
            {
                return;
            }

            int startX = (int)selection.GetBottomLeft().x;
            int endX = (int)selection.GetTopRight().x;

            int startY = (int)selection.GetBottomLeft().y;
            int endY = (int)selection.GetTopRight().y;

            for (int y = startY; y <= endY; ++y)
            {
                for (int x = startX; x <= endX; ++x)
                {
                    int key = (int)((int)tileMap.MapSize.x * y + x);
                    if (key < 0 || key > tileLayer.Tiles.Length)
                    {
                        return;
                    }
                    if (tileLayer.Tiles[key] != null)
                    {
                        GameObject go = tileLayer.Tiles[key].gameObject;
                        Vector2 movedPosition = tileMap.GetGridIndexPosInWorldSpace(new Vector2(x + movementOffset.x, y + movementOffset.y));
                        go.transform.position = movedPosition;
                    }
                }
            }
        }

        private void MoveCopyBuffer(Event e)
        {
            if (!selection.hasSelection || selection.selectingState != Selection.SelectionState.Selected || selection.selectionToolMode != Selection.SelectionToolMode.Copy)
            {
                return;
            }

            Vector2 minPos;
            minPos.x = Mathf.Min(selection.endPoint.x, selection.startPoint.x);
            minPos.y = Mathf.Min(selection.endPoint.y, selection.startPoint.y);
            Vector2 offset = CalculateMoveOffset(e);
            minPos += offset;

            for (int _y = 0; _y < selection.bufferH; ++_y)
            {
                for (int _x = 0; _x < selection.bufferW; ++_x)
                {
                    GameObject go = selection.copyBuffer[_y * selection.bufferW + _x];
                    if (go != null)
                    {
                        go.transform.position = minPos + new Vector2(_x * tileMap.TileSize.x, _y * tileMap.TileSize.y) + 0.5f * tileMap.TileSize;
                        go.transform.position += tileMap.transform.position;
                    }
                }
            }
        }

        private Vector2 CalculateMoveOffset(Event e)
        {
            if (e == null)
            {
                return Vector2.zero;
            }
            Vector2 mousePosition = TileEditorUtility.GetMousePosInWorld(e.mousePosition);
            Vector2 centerIndex = tileMap.TransformPositionToGridIndex(mousePosition);
            centerIndex = tileMap.ClampGridIndex(centerIndex);
            Vector2 selectionCenter = selection.GetSelectionCenterIndex();
            Vector2 movementOffset = centerIndex - selectionCenter;
            movementOffset.x = Mathf.Clamp(movementOffset.x, -selection.GetBottomLeft().x, tileMap.MapSize.x - 1 - selection.GetTopRight().x);
            movementOffset.y = Mathf.Clamp(movementOffset.y, -selection.GetBottomLeft().y, tileMap.MapSize.y - 1 - selection.GetTopRight().y);
            return movementOffset;
        }

        private void ApplyMovedTiles()
        {
            if (selection.hasSelection && selection.selectingState == Selection.SelectionState.Selected && selection.selectionToolMode == Selection.SelectionToolMode.Move)
            {
                TileLayer tileLayer = GetSelectedTileLayer();
                if (tileLayer == null)
                {
                    return;
                }
                int startX = (int)selection.GetBottomLeft().x;
                int endX = (int)selection.GetTopRight().x;

                int startY = (int)selection.GetBottomLeft().y;
                int endY = (int)selection.GetTopRight().y;

                int w = endX - startX + 1;
                int h = endY - startY + 1;
                Undo.IncrementCurrentGroup();

                Undo.RecordObject(tileLayer, null);

                Tile[] buffer = new Tile[w * h];
                for (int y = startY; y <= endY; ++y)
                {
                    for (int x = startX; x <= endX; ++x)
                    {
                        int key = (int)((int)tileMap.MapSize.x * y + x);
                        if (key < 0 || key > tileLayer.Tiles.Length)
                        {
                            continue;
                        }
                        buffer[(y - startY) * w + (x - startX)] = tileLayer.Tiles[key];
                        tileLayer.Tiles[key] = null;
                    }
                }
                for (int y = startY; y <= endY; ++y)
                {
                    for (int x = startX; x <= endX; ++x)
                    {

                        int key = (int)((int)tileMap.MapSize.x * y + x);
                        if (key < 0 || key > tileLayer.Tiles.Length)
                        {
                            continue;
                        }
                        Tile tile = buffer[(y - startY) * w + (x - startX)];
                        if (tile != null)
                        {
                            GameObject go = tile.gameObject;
                            Vector2 gridIndex = tileMap.TransformPositionToGridIndex(go.transform.position);
                            int newKey = (int)((int)tileMap.MapSize.x * gridIndex.y + gridIndex.x);
                            gridIndex = tileMap.ClampGridIndex(gridIndex);
                            if (newKey < 0 || newKey > tileLayer.Tiles.Length)
                            {
                                continue;
                            }
                            if (tileLayer.Tiles[newKey] != null)
                            {
                                GameObject gameObjectToDelete = tileLayer.Tiles[newKey].gameObject;
                                EditorApplication.delayCall += () => Undo.DestroyObjectImmediate(gameObjectToDelete);
                            }
                            Vector3 savedPosition = go.transform.position;
                            go.transform.position = tileMap.GetGridIndexPosInWorldSpace(new Vector2(x, y));
                            Undo.RecordObject(go.transform, null);
                            go.transform.position = savedPosition;
                            tileLayer.Tiles[newKey] = tile;
                        }
                    }
                }

                Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
            }
        }

        private void ApplyCopyBuffer(Event e)
        {
            if (selection.hasSelection && selection.selectingState == Selection.SelectionState.Selected && selection.selectionToolMode == Selection.SelectionToolMode.Copy)
            {
                TileLayer tileLayer = GetSelectedTileLayer();
                if (tileLayer == null)
                {
                    selection.DeleteCopyBuffer();
                    return;
                }
                Vector2 minPos;
                minPos.x = Mathf.Min(selection.endPoint.x, selection.startPoint.x);
                minPos.y = Mathf.Min(selection.endPoint.y, selection.startPoint.y);
                Vector2 offset = CalculateMoveOffset(e);
                minPos += offset;
                for (int _y = 0; _y < selection.bufferH; ++_y)
                {
                    for (int _x = 0; _x < selection.bufferW; ++_x)
                    {
                        GameObject go = selection.copyBuffer[_y * selection.bufferW + _x];
                        Undo.RegisterCreatedObjectUndo(go, null);
                        if (go != null)
                        {
                            go.transform.position = minPos + new Vector2(_x * tileMap.TileSize.x, _y * tileMap.TileSize.y) + 0.5f * tileMap.TileSize;
                            go.transform.position += tileMap.transform.position;

                            Vector2 gridIndex = tileMap.TransformPositionToGridIndex(go.transform.position);
                            gridIndex = tileMap.ClampGridIndex(gridIndex);
                            int key = (int)(tileMap.MapSize.x * gridIndex.y + gridIndex.x);

                            if (tileLayer.Tiles[key] != null)
                            {
                                Tile tile = tileLayer.Tiles[key];
                                Undo.RecordObject(tileLayer, null);
                                EditorApplication.delayCall += () => Undo.DestroyObjectImmediate(tile.gameObject); ;
                            }

                            {
                                //apply layer properties to game object 
                                ApplyTileLayerToCopiedTile(go, tileLayer);
                                go.layer = tileLayer.PhysicsLayer;
                                go.name = "Tile_" + gridIndex.x + "_" + gridIndex.y;
                                go.transform.position = new Vector3(go.transform.position.x, go.transform.position.y, tileLayer.transform.position.z);
                                go.transform.parent = tileLayer.transform;
                                Undo.RecordObject(tileLayer, null);
                                tileLayer.Tiles[key] = go.GetComponent<Tile>();
                            }
                        }
                    }
                }
            }
        }

        private void ApplyTileLayerToCopiedTile(GameObject gameObject, TileLayer tileLayer)
        {
            if (gameObject == null || tileLayer == null)
            {
                return;
            }

            SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
            renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, tileLayer.Alpha / 255.0f);
            renderer.sortingLayerName = tileLayer.SortingLayer;
        }

        private void SetupSelectedTiles(HandleTile handler)
        {
            if (selection.hasSelection && selection.selectingState == Selection.SelectionState.Selected)
            {
                TileLayer tileLayer = GetSelectedTileLayer();
                if (tileLayer == null)
                {
                    return;
                }
                int startX = (int)selection.GetBottomLeft().x;
                int endX = (int)selection.GetTopRight().x;

                int startY = (int)selection.GetBottomLeft().y;
                int endY = (int)selection.GetTopRight().y;

                for (int y = startY; y <= endY; ++y)
                {
                    for (int x = startX; x <= endX; ++x)
                    {
                        handler(tileLayer, x, y);
                    }
                }
            }
        }

        private void HandleSelectedTilesDeletion(TileLayer tileLayer, int x, int y)
        {
            int key = (int)((int)tileMap.MapSize.x * y + x);
            if (tileLayer == null || key < 0 || key > tileLayer.Tiles.Length)
            {
                return;
            }
            if (tileLayer.Tiles[key] != null)
            {
                GameObject go = tileLayer.Tiles[key].gameObject;

                Undo.RecordObject(tileLayer, null);
                EditorApplication.delayCall += () =>
                {
                    go.transform.position = tileMap.GetGridIndexPosInWorldSpace(new Vector2(x, y));
                    Undo.DestroyObjectImmediate(go);
                };
                tileLayer.Tiles[key] = null;
            }
        }

        private void HandleSelectedTilesFilling(TileLayer tileLayer, int x, int y)
        {
            TileData tileData = GetSelectedTileData();
            int key = (int)((int)tileMap.MapSize.x * y + x);
            if (tileLayer == null || tileData == null || key < 0 || key > tileLayer.Tiles.Length)
            {
                return;
            }

            if (tileLayer.Tiles[key] != null)
            {
                Tile tile = tileLayer.Tiles[key];
                if (TileEditorUtility.IsTheSameTile(tile, tileData))
                {
                    if (tileSelectMode == TileSelectMode.Scene)
                    {
                        if (TileEditorUtility.IsTileColliderEqualsToColliderInfo(tile, colliderInfo))
                        {
                            return;
                        }
                    }
                    else
                    {
                        return;
                    }
                }
                Undo.RecordObject(tileLayer, null);
                EditorApplication.delayCall += () => Undo.DestroyObjectImmediate(tile.gameObject);
            }
            {
                Vector2 pos = tileMap.GetGridIndexPosInWorldSpace(new Vector2(x, y));
                GameObject go = TileEditorUtility.CreateTile(new Vector3(pos.x, pos.y, tileLayer.transform.position.z), tileData, tileLayer.Alpha / 255.0f, tileLayer.SortingLayer);
                if (tileData.Tag != null)
                {
                    go.tag = tileData.Tag;
                }
                go.layer = tileLayer.PhysicsLayer;
                go.name = "Tile_" + x + "_" + y;
                go.transform.parent = tileLayer.transform;
                Tile tile = SetupTile(go, tileData);
                if (tileSelectMode == TileSelectMode.Scene && colliderInfo != null)
                {
                    TileEditorUtility.ApplyColliderInfoToTile(tile, colliderInfo);
                }
                Undo.RecordObject(tileLayer, null);
                tileLayer.Tiles[key] = tile;
                Undo.RegisterCreatedObjectUndo(go, "Fill Tiles");
            }
        }

        private Tile SetupTile(GameObject go, TileData tileData)
        {
            if (go == null || tileData == null)
            {
                return null;
            }
            go.transform.Rotate(0, 0, tileData.Rotation, Space.World);

            Tile tile = go.GetComponent(typeof(Tile)) as Tile;

            if (!tile)
            {
                tile = go.AddComponent(typeof(Tile)) as Tile;
            }

            Collider2D collider = go.GetComponent(typeof(Collider2D)) as Collider2D;
            if (collider)
            {
                DestroyImmediate(collider);
                collider = null;
            }
            tile.Collision = tileData.Collision;
            switch (tile.Collision)
            {
                case CollisionType.Box:
                    collider = go.AddComponent(typeof(BoxCollider2D)) as BoxCollider2D;
                    break;
                case CollisionType.Circle:
                    collider = go.AddComponent(typeof(CircleCollider2D)) as CircleCollider2D;
                    break;
                case CollisionType.Polygon:
                    collider = go.AddComponent(typeof(PolygonCollider2D)) as PolygonCollider2D;
                    break;
            }
            if (collider != null)
            {
                collider.sharedMaterial = tileData.PhysicsMaterial;
                collider.isTrigger = tileData.IsTrigger;
            }
            return tile;
        }

        #endregion

        #region Tile Layer Methods
        private TileLayer GetSelectedTileLayer()
        {
            return GetTileLayerAtIndex(selectedLayer.intValue);
        }

        private List<TileLayer> GetAllTileLayers()
        {
            List<TileLayer> layers = new List<TileLayer>();

            for (int i = 0; i < tileLayerList.serializedProperty.arraySize; ++i)
            {
                TileLayer layer = GetTileLayerAtIndex(i);
                if (layer == null)
                {
                    continue;
                }
                layers.Add(layer);
            }
            return layers;
        }

        private TileLayer GetTileLayerAtIndex(int index)
        {
            if (index < 0 || index >= tileLayerList.serializedProperty.arraySize)
            {
                return null;
            }
            return tileLayerList.serializedProperty.GetArrayElementAtIndex(index).objectReferenceValue as TileLayer;
        }

        #endregion

        #region Mouse Methods
        void DrawMouseRect(Event evnt)
        {
            Vector2 tileSize = tileMap.TileSize;
            Vector2 mousePosition = evnt.mousePosition;
            Vector2 origin = new Vector2(tileMap.transform.position.x, tileMap.transform.position.y);
            mousePosition = TileEditorUtility.GetMousePosInWorld(mousePosition);
            int x = -1;
            int y = -1;
            if (tileMap.IsInTileMap(mousePosition))
            {
                Vector2 positionInGrid = tileMap.TransformPositionToGridIndex(mousePosition);
                DrawMouseCursor(new Vector2(positionInGrid.x * tileSize.x, positionInGrid.y * tileSize.y) + origin, tileSize);
                x = (int)positionInGrid.x;
                y = (int)positionInGrid.y;
            }
            DrawMouseGridInfo(x, y);
        }

        private void DrawMouseGridInfo(int x, int y)
        {
            if (Camera.current == null)
            {
                return;
            }
            Handles.BeginGUI();
            float height = Camera.current.pixelHeight;
            float boxWidth = 60;
            float boxHeight = 20;
            float gap = 10;
            Rect rect1 = new Rect(20, height - boxHeight - gap, boxWidth, boxHeight);
            Rect rect2 = new Rect(boxWidth + 20 + gap, height - boxHeight - gap, boxWidth, boxHeight);

            string sX = x < 0 ? "x: " : "x: " + x;
            string sY = y < 0 ? "y: " : "y: " + y;
            GUI.Box(rect1, sX, EditorStyles.textArea);
            GUI.Box(rect2, sY, EditorStyles.textArea);
            Handles.EndGUI();
        }

        private void DrawMouseCursor(Vector2 pos, Vector2 size)
        {
            mouseRectCorner = GetRect(pos, size);
            Handles.DrawSolidRectangleWithOutline(mouseRectCorner, new Color(1, 1, 1, 0.2f), Color.white);
        }

        private Vector3[] GetRect(Vector2 center, Vector2 size)
        {
            //bottom left
            mouseRectCorner[0] = center + new Vector2(0, size.y);
            //top left
            mouseRectCorner[1] = center + new Vector2(0, 0);
            //top right
            mouseRectCorner[2] = center + new Vector2(size.x, 0);
            //bottom right
            mouseRectCorner[3] = center + new Vector2(size.x, size.y);
            return mouseRectCorner;
        }

        #endregion
    }
}
#endif