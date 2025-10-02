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
        [MenuItem("Tools/TDMapCreator/Map Creator Window")]
        public static void ShowWindow()
        {
            GetWindow<TDMapCreator_EditorWindow>("TDMapCreator");
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
                string loadPath = EditorUtility.OpenFolderPanel("Select Map Folder", PackageVariables.DefaultSavePath, "");
                if (PackageUtilis.IsPathPartOfAssets(loadPath))
                {
                    string relativePath = PackageUtilis.AbsoluteToRelativeAssetsPath(loadPath);
                    string[] guids = AssetDatabase.FindAssets("t:TDMapSaveSO", new[] { relativePath });

                    if (guids.Length == 1)
                    {
                        string assetPath = AssetDatabase.GUIDToAssetPath(guids[0]);

                        SessionState.SetString(PackageVariables.EnvKeys.SESSIONSTATE_MAP_LOAD_PATH, assetPath);

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
                TDMapSaveSO.SaveMapDialogue();
            }

            GUILayout.Space(5f);

            GUILayout.BeginHorizontal();
            GUILayout.Space(20f);
            if (GUILayout.Button("QUIT CREATOR", new GUIStyle(GUI.skin.button) { fontSize = 14 }, GUILayout.Height(20f)))
            {
                int saveBefore = EditorUtility.DisplayDialogComplex("TD Map Creator", "Do you want to save your map before quitting?", "Save and Quit", "Cancel", "Quit Without Saving");
                if (saveBefore != 1) 
                {
                    if (saveBefore == 0) { TDMapSaveSO.SaveMapDialogue(); }
                    EditorApplication.isPlaying = false;
                }
            }
            GUILayout.Space(20f);
            GUILayout.EndHorizontal();
        }

        private void DrawSettingsSection()
        {
            GUILayout.Label("Paths:", new GUIStyle("BoldLabel") { fontSize = 12 }); // Paths section -------------------------
            // Default Save Folder
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Default Save Folder:", GUILayout.Width(120));
            using (new EditorGUI.DisabledScope(true))
            {
                EditorGUILayout.TextField(PackageVariables.DefaultSavePath);
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Browse...", GUILayout.Width(90)))
            {
                string startDir = Application.dataPath;
                string selectedPath = EditorUtility.SaveFolderPanel("Select Default Save Folder", startDir, "");
                if (PackageUtilis.IsPathPartOfAssets(selectedPath))
                {
                    PackageVariables.DefaultSavePath = selectedPath;
                }
                else
                {
                    EditorUtility.DisplayDialog("TD Map Creator", "Please select a folder inside the Assets directory of this project.", "OK");
                }
            }
            if (GUILayout.Button("Open", GUILayout.Width(60)))
            {
                if (Directory.Exists(PackageVariables.DefaultSavePath)) { EditorUtility.RevealInFinder(PackageVariables.DefaultSavePath + "/"); }
                else { EditorUtility.DisplayDialog("TD Map Creator", $"Directory {PackageVariables.DefaultSavePath} no longer exists in this project.", "OK"); }
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(10f);
            GUILayout.Label("Other:", new GUIStyle("BoldLabel") { fontSize = 12 }); // Other section -------------------------
            // Debugs Mask
            string[] options = new string[] { "Logs", "Warnings", "Errors", "Asserts", "Exceptions" };
            PackageVariables.DebugsMask = EditorGUILayout.MaskField("Allowed Debugs:", PackageVariables.DebugsMask , options);
        }
    }
}
#endif
