#if UNITY_EDITOR
using adbreeker.TDMapCreator;
using Codice.Utils;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.SceneManagement;
using UnityEditor.ShortcutManagement;
using UnityEngine;


namespace adbreeker.TDMapCreator
{
    [InitializeOnLoad]
    public class TDMapCreator_Setup
    {
        static TDMapCreator_Setup()
        {
            //Every load initialization
            EditorApplication.playModeStateChanged -= PlaymodeChange;
            EditorApplication.playModeStateChanged += PlaymodeChange;

            EditorApplication.quitting -= OnEditorQuit;
            EditorApplication.quitting += OnEditorQuit;

            if (SessionState.GetBool(PackageVariables.EnvKeys.SESSIONSTATE_INITIALIZED, false)) { return; }
            SessionState.SetBool(PackageVariables.EnvKeys.SESSIONSTATE_INITIALIZED, true);

            //Session initialization
            PackageUtilis.PrintDebug(LogType.Log, "Initialized");
        }

        static void PlaymodeChange(PlayModeStateChange state)
        {
            PackageUtilis.PrintDebug(LogType.Log, $"Playmode state changed: {state}");

            if (state == PlayModeStateChange.ExitingEditMode)
            {
                OverrideShortcuts();
            }
            else if (state == PlayModeStateChange.EnteredPlayMode)
            {

            }
            else if (state == PlayModeStateChange.ExitingPlayMode)
            {
                EditorSceneManager.playModeStartScene = null;
                RestoreShortcuts();
            }
            else if (state == PlayModeStateChange.EnteredEditMode)
            {

            }
        }

        static void OnEditorQuit()
        {
            RestoreShortcuts();
        }

        // Override Unity Shortcuts
        private static void OverrideShortcuts()
        {
            var manager = ShortcutManager.instance;
            string tempProfileId = "TDMapCreator-TempProfile";
            manager.CreateProfile(tempProfileId);
            manager.activeProfileId = tempProfileId;

            // Disable the shortcut
            manager.RebindShortcut("Main Menu/File/Save", ShortcutBinding.empty);
        }

        private static void RestoreShortcuts()
        {
            var manager = ShortcutManager.instance;
            string tempProfileId = "TDMapCreator-TempProfile";

            if (manager.GetAvailableProfileIds().Contains(tempProfileId))
            {
                manager.DeleteProfile(tempProfileId);
            }
        }
    }
}
#endif
