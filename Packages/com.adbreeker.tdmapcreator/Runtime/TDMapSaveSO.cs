using System.IO;
using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;


#if UNITY_EDITOR
using UnityEditor;
#endif


namespace adbreeker.TDMapCreator
{
    public class TDMapSaveSO : ScriptableObject
    {
        [field: SerializeField] public GameObject MapPrefab { get; private set; }
        private string _mapPrefabPath;
        [field: SerializeField] public Texture2D MapTexture { get; private set; }
        private string _mapTexturePath;
        [field: SerializeField] public Sprite MapSprite { get; private set; }


#if UNITY_EDITOR
        public TDMapSaveSO Init(GameObject mapPrefab, Texture2D mapTexture)
        {
            this.MapPrefab = mapPrefab;
            this.MapTexture = mapTexture;
            this.MapSprite = PackageUtilis.GetSpritesFromTextureAsset(mapTexture).FirstOrDefault();
            return this;
        }

        public static void SaveTDMap(string absolutePath, GameObject mapPrefab, Texture2D mapTexture)
        {
            string relativePath = PackageUtilis.AbsoluteToRelativeAssetsPath(absolutePath);
            string soPath =  Path.Combine(relativePath, "TDMapSave.asset");
            string prefabPath = Path.Combine(relativePath, "MapPrefab.prefab");
            string texturePath = Path.Combine(relativePath, "MapImage.png");

            List<string> errors = new List<string>();

            // Save Map Image as PNG
            try
            {
                var data = mapTexture.EncodeToPNG();
                Directory.CreateDirectory(Path.GetDirectoryName(texturePath));
                File.WriteAllBytes(texturePath, data);

                AssetDatabase.Refresh();
                TextureImporter importer = AssetImporter.GetAtPath(texturePath) as TextureImporter;
                if (importer != null)
                {
                    importer.spriteImportMode = SpriteImportMode.Single;
                    importer.SaveAndReimport();
                }
            }
            catch (Exception e)
            {
                string message = "Error saving map image: " + e;
                errors.Add(message);
                PackageUtilis.PrintDebug(LogType.Error, message);
            }

            //Save Map Object as Prefab
            // If _mapRoot is a prefab instance, unpack it completely IN-PLACE so there are no prefab links.
            try
            {
                if (PrefabUtility.IsPartOfPrefabInstance(mapPrefab))
                {
                    PrefabUtility.UnpackPrefabInstance(mapPrefab, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
                }

                PrefabUtility.SaveAsPrefabAsset(mapPrefab, prefabPath);
            }
            catch (Exception e)
            {
                string message = "Error saving map prefab: " + e;
                errors.Add(message);
                PackageUtilis.PrintDebug(LogType.Error, message);
            }


            AssetDatabase.Refresh();

            TDMapSaveSO saveSO = ScriptableObject.CreateInstance<TDMapSaveSO>().Init(
                AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath),
                AssetDatabase.LoadAssetAtPath<Texture2D>(texturePath)
            );
            try
            {
                AssetDatabase.CreateAsset(saveSO, soPath);
            }
            catch (Exception e)
            {
                string message = "Error creating map save asset: " + e;
                errors.Add(message);
                PackageUtilis.PrintDebug(LogType.Error, message);
            }

            if (errors.Count == 0)
            {
                EditorUtility.DisplayDialog("TDMapCreator", $"Map saved successfully to {absolutePath}", "OK");
            }
            else
            {
                string errorsList = string.Join("\n", errors.Select(err => "- " + err));
                EditorUtility.DisplayDialog(
                    "TDMapCreator", 
                    $"Errors listed below appeared during saving:\n{errorsList}\nPart or whole save might be corupted on {absolutePath}", 
                    "OK");
            }
        }

        public static void SaveMapDialogue()
        {
            string savePath;
            if (Directory.Exists(PackageVariables.DefaultSavePath))
            {
                savePath = EditorUtility.SaveFilePanel("Save Map Asset", PackageVariables.DefaultSavePath, "Map" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss"), "");
            }
            else
            {
                savePath = EditorUtility.SaveFilePanel("Save Map Asset (default save directory could not be found)", Application.dataPath, "Map" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss"), "");
            }

            if (!string.IsNullOrWhiteSpace(savePath))
            {
                Directory.CreateDirectory(savePath);
                var mapImage = TDMapCreatorManager.Instance?.GetMapImage();
                var mapObject = TDMapCreatorManager.Instance?.GetMapObject();

                TDMapSaveSO.SaveTDMap(savePath, mapObject, mapImage);

                AssetDatabase.Refresh();
                PackageUtilis.PrintDebug(LogType.Log, $"Map saved to: {savePath}");
            }
        }
#endif
    }
}
