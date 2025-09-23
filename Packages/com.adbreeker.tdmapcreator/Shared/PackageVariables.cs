using UnityEditor.PackageManager;
using UnityEngine;
using System.Linq;
using adbreeker.TDMapCreator;

namespace adbreeker.TDMapCreator
{
    public static class PackageVariables
    {
        //package constants
        public const string PACKAGE_NAME = "com.adbreeker.tdmapcreator";

        //editor prefs keys
        public const string EDITORPREFS_DEFAULT_SAVE_PATH = PACKAGE_NAME + ".DEFAULT_SAVE_PATH";

        //session state keys
        public const string SESSIONSTATE_INITIALIZED = PACKAGE_NAME + ".INITIALIZED";
        public const string SESSIONSTATE_MAP_LOAD_PATH = PACKAGE_NAME + ".MAP_LOAD_PATH";

        public static string GetPackageAbsolutePath()
        {
            var listRequest = Client.List(true); // true = include dependencies
            while (!listRequest.IsCompleted) { } // wait for the request to finish

            if (listRequest.Status == StatusCode.Failure)
            {
                PackageUtilis.PrintDebug(LogType.Error, "Failed to list packages: " + listRequest.Error.message);
                return null;
            }

            var package = listRequest.Result.FirstOrDefault(p => p.name == PACKAGE_NAME);
            if (package != null)
            {
                return package.resolvedPath; // absolute path on disk
            }

            PackageUtilis.PrintDebug(LogType.Error, "Package \"TDMapCreator\" not found");
            return null;
        }

        public static string GetPackageRelativePath()
        {
            return "Packages/" + PACKAGE_NAME;
        }
    }
}
