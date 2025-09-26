#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using System.IO;
using System;
using System.Linq;
using System.Collections.Generic;
using adbreeker.TDMapCreator;


namespace adbreeker.TDMapCreator
{
    public class TDMapCreator_EditorWindow : EditorWindow
    {
        private string _defaultSavePath;


        [MenuItem("Tools/TDMapCreator/Map Creator Window")]
        public static void ShowWindow()
        {
            GetWindow<TDMapCreator_EditorWindow>("TDMapCreator");
        }

        private void OnEnable()
        {
            _defaultSavePath = EditorPrefs.GetString(PackageVariables.EDITORPREFS_DEFAULT_SAVE_PATH, Application.dataPath);
        }

        private void OnDisable()
        {
            EditorPrefs.SetString(PackageVariables.EDITORPREFS_DEFAULT_SAVE_PATH, _defaultSavePath);
        }

        private void OnGUI()
        {
            GUILayout.Label("TDMapCreator", new GUIStyle("BoldLabel") { fontSize = 24, padding = new RectOffset(0, 0, 10, 10), alignment = TextAnchor.MiddleCenter });
            GUILayout.Space(10f);

            if (!EditorApplication.isPlaying) { DrawCreateOrLoadMapSection(); }
            else { DrawSaveQuitSection(); }
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Space(10f);

            GUILayout.Label("Settings:", new GUIStyle("BoldLabel") { fontSize = 16, padding = new RectOffset(0, 0, 10, 10), alignment = TextAnchor.MiddleCenter });
            DrawSettingsSection();
        }

        private void DrawCreateOrLoadMapSection()
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("NEW MAP", new GUIStyle(GUI.skin.button) { fontSize = 16 }, GUILayout.Height(24f)))
            {
                string scenePath = Path.Combine(PackageVariables.GetPackageRelativePath(), "Resources", "Scenes", "_MapCreator.unity");
                PackageUtilis.LaunchTemporalScene(scenePath);
            }
            if (GUILayout.Button("LOAD MAP", new GUIStyle(GUI.skin.button) { fontSize = 16 }, GUILayout.Height(24f)))
            {
                string loadPath = EditorUtility.OpenFolderPanel("Select Map Folder", _defaultSavePath, "");
                if (PackageUtilis.IsPathPartOfAssets(loadPath))
                {
                    string relativePath = PackageUtilis.AbsoluteToRelativeAssetsPath(loadPath);
                    string[] guids = AssetDatabase.FindAssets("t:TDMapSaveSO", new[] { relativePath });

                    if (guids.Length == 1)
                    {
                        string assetPath = AssetDatabase.GUIDToAssetPath(guids[0]);

                        SessionState.SetString(PackageVariables.SESSIONSTATE_MAP_LOAD_PATH, assetPath);

                        string scenePath = Path.Combine(PackageVariables.GetPackageRelativePath(), "Resources", "Scenes", "_MapCreator.unity");
                        PackageUtilis.LaunchTemporalScene(scenePath);
                    }

                    // Invalid cases
                    else if (guids.Length == 0)
                    {
                        EditorUtility.DisplayDialog("TD Map Creator", "No TDMapSave asset found in the selected folder.", "OK");
                        return;
                    }
                    else if (guids.Length > 1)
                    {
                        EditorUtility.DisplayDialog("TD Map Creator", "Multiple TDMapSave assets found in the selected folder. Please ensure only one exists.", "OK");
                        return;
                    }
                }
                else
                {
                    EditorUtility.DisplayDialog("TD Map Creator", "Please select a folder inside the Assets directory of this project.", "OK");
                }
            }
            GUILayout.EndHorizontal();
        }

        private void DrawSaveQuitSection()
        {
            if (GUILayout.Button("SAVE MAP", new GUIStyle(GUI.skin.button) { fontSize = 16 }, GUILayout.Height(24f)))
            {
                MapSaveDialogue();
            }

            GUILayout.Space(5f);

            GUILayout.BeginHorizontal();
            GUILayout.Space(20f);
            if (GUILayout.Button("QUIT CREATOR", new GUIStyle(GUI.skin.button) { fontSize = 14 }, GUILayout.Height(20f)))
            {
                int saveBefore = EditorUtility.DisplayDialogComplex("TD Map Creator", "Do you want to save your map before quitting?", "Save and Quit", "Cancel", "Quit Without Saving");
                if (saveBefore != 1) 
                {
                    if (saveBefore == 0) { MapSaveDialogue(); }
                    EditorApplication.isPlaying = false;
                }
            }
            GUILayout.Space(20f);
            GUILayout.EndHorizontal();
        }

        private void DrawSettingsSection()
        {
            // Default Save Folder
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Default Save Folder:", GUILayout.Width(120));
            using (new EditorGUI.DisabledScope(true))
            {
                EditorGUILayout.TextField(_defaultSavePath);
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Browse...", GUILayout.Width(90)))
            {
                string startDir = Directory.Exists(_defaultSavePath) ? _defaultSavePath : Application.dataPath;
                string selectedPath = EditorUtility.SaveFolderPanel("Select Default Save Folder", startDir, "");
                if (PackageUtilis.IsPathPartOfAssets(selectedPath))
                {
                    _defaultSavePath = selectedPath;
                    EditorPrefs.SetString(PackageVariables.EDITORPREFS_DEFAULT_SAVE_PATH, _defaultSavePath);
                }
                else
                {
                    EditorUtility.DisplayDialog("TD Map Creator", "Please select a folder inside the Assets directory of this project.", "OK");
                }
            }
            if (GUILayout.Button("Open", GUILayout.Width(60)))
            {
                if (Directory.Exists(_defaultSavePath)) { EditorUtility.RevealInFinder(_defaultSavePath + "/"); }
                else { EditorUtility.DisplayDialog("TD Map Creator", $"Directory {_defaultSavePath} no longer exists in this project.", "OK"); }
            }
            EditorGUILayout.EndHorizontal();
        }


        // Utilis methods
        private void MapSaveDialogue()
        {
            string savePath;
            if (Directory.Exists(_defaultSavePath))
            {
                savePath = EditorUtility.SaveFilePanel("Save Map Asset", _defaultSavePath, "Map" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss"), "");
            }
            else
            {
                savePath = EditorUtility.SaveFilePanel("Save Map Asset (default save directory could not be found)", Application.dataPath, "Map" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss"), "");
            }

            if (!string.IsNullOrWhiteSpace(savePath))
            {
                Directory.CreateDirectory(savePath);
                var mapImage = TDMapCreatorManager.Instance?.GetMapImage();
                var mapObject = TDMapCreatorManager.Instance?.GetMapObject();

                TDMapSaveSO.SaveTDMap(savePath, mapObject, mapImage);

                AssetDatabase.Refresh();
                PackageUtilis.PrintDebug(LogType.Log, $"Map saved to: {savePath}");
            }
        }

    }
}
#endif
