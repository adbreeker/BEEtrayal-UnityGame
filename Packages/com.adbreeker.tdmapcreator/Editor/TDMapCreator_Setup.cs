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
            if (SessionState.GetBool(PackageVariables.SESSIONSTATE_INITIALIZED, false))
                return;

            SessionState.SetBool(PackageVariables.SESSIONSTATE_INITIALIZED, true);


            PackageUtilis.PrintDebug(LogType.Log, "Initialized");
        }
    }
}
#endif
