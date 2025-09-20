using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using System.IO;
using System;
using System.Linq;
using System.Collections.Generic;

public class MapCreator_EditorWindow : EditorWindow
{
    private string _defaultSavePath;

    //editor prefs keys
    const string _prefDefaultSavePath = MapCreator_Setup.PACKAGE_NAME + ".DEFAULT_SAVE_PATH";

    [MenuItem("Tools/Map Creator/Map Creator Window")]
    public static void ShowWindow()
    {
        GetWindow<MapCreator_EditorWindow>("Map Creator");
    }

    private void OnEnable()
    {
        _defaultSavePath = EditorPrefs.GetString(_prefDefaultSavePath, Application.dataPath);
    }

    private void OnDisable()
    {
        EditorPrefs.SetString(_prefDefaultSavePath, _defaultSavePath);
    }

    private void OnGUI()
    {
        GUILayout.Label("Map Creator", new GUIStyle("BoldLabel") { fontSize = 24, padding = new RectOffset(0, 0, 10, 10), alignment = TextAnchor.MiddleCenter });
        GUILayout.Space(10f);

        CreateOrLoadMapSection();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Space(10f);

        GUILayout.Label("Settings:", new GUIStyle("BoldLabel") { fontSize = 16, padding = new RectOffset(0, 0, 10, 10), alignment = TextAnchor.MiddleCenter });
        SettingsSection();
    }

    private void CreateOrLoadMapSection()
    {
        if (!EditorApplication.isPlaying)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("NEW MAP", new GUIStyle(GUI.skin.button) { fontSize = 16 }, GUILayout.Height(24f)))
            {
                string scenePath = Path.Combine(MapCreator_Setup.GetPackageRelativePath(), "Resources", "Scenes", "_MapCreator.unity");
                MapCreatorUtilis.LaunchTemporalScene(scenePath);
            }
            if (GUILayout.Button("LOAD MAP", new GUIStyle(GUI.skin.button) { fontSize = 16 }, GUILayout.Height(24f)))
            {
                string loadPath = EditorUtility.OpenFilePanel("Select Map Folder", _defaultSavePath, ".tdmap");

                
            }
            GUILayout.EndHorizontal();
        }
        else
        {
            if (GUILayout.Button("SAVE MAP", new GUIStyle(GUI.skin.button) { fontSize = 16 }, GUILayout.Height(24f)))
            {
                string savePath = EditorUtility.SaveFilePanel("Save Map Asset", _defaultSavePath , "Map" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss"), "");
                if (!string.IsNullOrWhiteSpace(savePath)) 
                { 
                    Directory.CreateDirectory(savePath);
                    var mapImage = MapCreatorManager.Instance?.ExportMapImage(Path.Combine(savePath, "MapImage"));
                    MapCreatorManager.Instance?.ExportMapPrefab(Path.Combine(savePath, "MapPrefab"));

                    var saveSO = ScriptableObject.CreateInstance<TDMapSaveSO>();
                    saveSO.Init(null, mapImage);
                    saveSO.SaveAsAsset(Path.Combine(savePath, "MapSave.tdmap"));

                    AssetDatabase.Refresh();
                    Debug.Log($"Map saved to: {savePath}");
                }
            }
        }
    }

    private void SettingsSection()
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
            string selected = EditorUtility.SaveFolderPanel("Select Default Save Folder", startDir, "");
            if (!string.IsNullOrEmpty(selected))
            {
                _defaultSavePath = selected;
                EditorPrefs.SetString(_prefDefaultSavePath, _defaultSavePath);
            }
        }
        if (GUILayout.Button("Open", GUILayout.Width(60)))
        {
            if (Directory.Exists(_defaultSavePath))
                EditorUtility.RevealInFinder(_defaultSavePath + "/");
        }
        EditorGUILayout.EndHorizontal();

        //
    }

    
}
