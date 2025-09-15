using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;

#if UNITY_EDITOR
public class MapCreator_Setup
{
    const string PACKAGE_NAME = "com.adbreeker.tdmapcreator";

    /// <summary>
    /// Returns the full folder path of a package given its name, e.g. "com.myplugin"
    /// </summary>
    public static string GetPackageAbsolutePath()
    {
        var listRequest = Client.List(true); // true = include dependencies
        while (!listRequest.IsCompleted) { } // wait for the request to finish

        if (listRequest.Status == StatusCode.Failure)
        {
            Debug.LogError("Failed to list packages: " + listRequest.Error.message);
            return null;
        }

        var package = listRequest.Result.FirstOrDefault(p => p.name == PACKAGE_NAME);
        if (package != null)
        {
            return package.resolvedPath; // absolute path on disk
        }

        Debug.LogError($"Package \"TD Map Creator\" not found");
        return null;
    }

    public static string GetPackageRelativePath()
    {
        return "Packages/" + PACKAGE_NAME;
    }
}
#endif
