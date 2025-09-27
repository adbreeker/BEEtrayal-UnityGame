#if UNITY_EDITOR
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;
using adbreeker.TDMapCreator;


namespace adbreeker.TDMapCreator
{
    [InitializeOnLoad]
    public class TDMapCreator_Setup
    {
        static TDMapCreator_Setup()
        {
            //Every load initialization

            if (SessionState.GetBool(PackageVariables.EnvKeys.SESSIONSTATE_INITIALIZED, false)) { return; }
            SessionState.SetBool(PackageVariables.EnvKeys.SESSIONSTATE_INITIALIZED, true);

            //Session initialization
            PackageUtilis.PrintDebug(LogType.Log, "Initialized");
        }
    }
}
#endif
