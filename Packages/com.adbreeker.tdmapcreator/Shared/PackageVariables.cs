#if UNITY_EDITOR
using UnityEditor.PackageManager;
using UnityEngine;
using System.Linq;
using UnityEditor;
using System.Runtime.CompilerServices;

namespace adbreeker.TDMapCreator
{
    public static class PackageVariables
    {
        public static class EnvKeys
        {
            //package constants
            public const string PACKAGE_NAME = "com.adbreeker.tdmapcreator";

            //editor prefs keys
            public const string EDITORPREFS_DEFAULT_SAVE_PATH = PACKAGE_NAME + ".DEFAULT_SAVE_PATH";
            public const string EDITORPREFS_DEBUGS_MASK = PACKAGE_NAME + ".DEBUGS_MASK";

            //session state keys
            public const string SESSIONSTATE_INITIALIZED = PACKAGE_NAME + ".INITIALIZED";
            public const string SESSIONSTATE_MAP_LOAD_PATH = PACKAGE_NAME + ".MAP_LOAD_PATH";
        }

        // Default Save Path
        private static string _defaultSavePath;
        public static string DefaultSavePath
        {
            get 
            {
                _defaultSavePath ??= EditorPrefs.GetString(EnvKeys.EDITORPREFS_DEFAULT_SAVE_PATH, Application.dataPath);
                return _defaultSavePath;
            }

            set 
            {
                if (_defaultSavePath != value)
                {
                    _defaultSavePath = value;
                    EditorPrefs.SetString(EnvKeys.EDITORPREFS_DEFAULT_SAVE_PATH, _defaultSavePath);
                }
            }
        }

        // Debug Prints Mask
        private static int _debugsMask = -1;
        public static int DebugsMask
        {
            get 
            {
                if (_debugsMask == -1) { _debugsMask = EditorPrefs.GetInt(EnvKeys.EDITORPREFS_DEBUGS_MASK, ~0); }
                return _debugsMask;
            }
            set 
            {
                if (_debugsMask != value)
                {
                    _debugsMask = value;
                    EditorPrefs.SetInt(EnvKeys.EDITORPREFS_DEBUGS_MASK, _debugsMask);
                }
            }
        }

        public static string GetPackageAbsolutePath()
        {
            var listRequest = Client.List(true); // true = include dependencies
            while (!listRequest.IsCompleted) { } // wait for the request to finish

            if (listRequest.Status == StatusCode.Failure)
            {
                PackageUtilis.PrintDebug(LogType.Error, "Failed to list packages: " + listRequest.Error.message);
                return null;
            }

            var package = listRequest.Result.FirstOrDefault(p => p.name == EnvKeys.PACKAGE_NAME);
            if (package != null)
            {
                return package.resolvedPath; // absolute path on disk
            }

            PackageUtilis.PrintDebug(LogType.Error, "Package \"TDMapCreator\" not found");
            return null;
        }

        public static string GetPackageRelativePath()
        {
            return "Packages/" + EnvKeys.PACKAGE_NAME;
        }
    }
}
#endif