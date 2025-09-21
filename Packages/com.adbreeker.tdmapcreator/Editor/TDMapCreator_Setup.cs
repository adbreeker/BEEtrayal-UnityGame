#if UNITY_EDITOR
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;


namespace adbreeker.TDMapCreator
{
    [InitializeOnLoad]
    public class TDMapCreator_Setup
    {
        public const string PACKAGE_NAME = "com.adbreeker.tdmapcreator";

        static TDMapCreator_Setup()
        {
            if (SessionState.GetBool("TDMapCreator_Initialized", false))
                return;

            SessionState.SetBool("TDMapCreator_Initialized", true);


            TDMapCreatorUtilis.PrintDebug(LogType.Log, "Initialized");
        }

        /// <summary>
        /// Returns the full folder path of a package given its name, e.g. "com.myplugin"
        /// </summary>
        public static string GetPackageAbsolutePath()
        {
            var listRequest = Client.List(true); // true = include dependencies
            while (!listRequest.IsCompleted) { } // wait for the request to finish

            if (listRequest.Status == StatusCode.Failure)
            {
                TDMapCreatorUtilis.PrintDebug(LogType.Error, "Failed to list packages: " + listRequest.Error.message);
                return null;
            }

            var package = listRequest.Result.FirstOrDefault(p => p.name == PACKAGE_NAME);
            if (package != null)
            {
                return package.resolvedPath; // absolute path on disk
            }

            TDMapCreatorUtilis.PrintDebug(LogType.Error, "Package \"TDMapCreator\" not found");
            return null;
        }

        public static string GetPackageRelativePath()
        {
            return "Packages/" + PACKAGE_NAME;
        }
    }
}
#endif
