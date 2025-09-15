using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using System.IO;

public class MapCreator_EditorWindow : EditorWindow
{
    private static bool _launchedFromWindow;

    [MenuItem("Tools/Map Creator/Map Creator Window")]
    public static void ShowWindow()
    {
        GetWindow<MapCreator_EditorWindow>("Map Creator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Map Creator", new GUIStyle("BoldLabel") { fontSize = 24, padding = new RectOffset(0, 0, 10, 10), alignment = TextAnchor.MiddleCenter });
        GUILayout.Space(10f);

        CreateOrLoadMap();
    }

    private void CreateOrLoadMap()
    {
        if (GUILayout.Button("Create New Map"))
        {
            // Build the Assets-relative path to the scene and load as SceneAsset
            string scenePath = Path.Combine(MapCreator_Setup.GetPackageRelativePath(), "Resources", "Scenes", "_MapCreator.unity");
            var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);

            if (sceneAsset == null)
            {
                EditorUtility.DisplayDialog("Map Creator", $"Cannot find scene at:\n{scenePath}", "OK");
                return;
            }

            // Use Play Mode Start Scene so Unity restores the previous scene automatically afterward
            EditorSceneManager.playModeStartScene = sceneAsset;

            // Ensure we clear the start scene when exiting play mode
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            _launchedFromWindow = true;

            // Enter play mode without opening/saving the _MapCreator scene in the editor
            EditorApplication.isPlaying = true;
        }
    }

    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (!_launchedFromWindow) return;

        if (state == PlayModeStateChange.EnteredEditMode)
        {
            // Reset so future play sessions use the currently open scene again
            EditorSceneManager.playModeStartScene = null;
            _launchedFromWindow = false;
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        }
    }
}
