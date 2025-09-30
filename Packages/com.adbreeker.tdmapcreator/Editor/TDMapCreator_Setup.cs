#if UNITY_EDITOR
using adbreeker.TDMapCreator;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.SceneManagement;
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

            if (SessionState.GetBool(PackageVariables.EnvKeys.SESSIONSTATE_INITIALIZED, false)) { return; }
            SessionState.SetBool(PackageVariables.EnvKeys.SESSIONSTATE_INITIALIZED, true);

            //Session initialization
            PackageUtilis.PrintDebug(LogType.Log, "Initialized");
        }

        static void PlaymodeChange(PlayModeStateChange state)
        {
            PackageUtilis.PrintDebug(LogType.Log, $"Playmode state changed: {state}");

            if(state == PlayModeStateChange.ExitingEditMode)
            {

            }
            else if (state == PlayModeStateChange.EnteredPlayMode)
            {

            }
            else if (state == PlayModeStateChange.ExitingPlayMode)
            {
                EditorSceneManager.playModeStartScene = null;
            }
            else if (state == PlayModeStateChange.EnteredEditMode)
            {

            }
        }
    }
}
#endif
