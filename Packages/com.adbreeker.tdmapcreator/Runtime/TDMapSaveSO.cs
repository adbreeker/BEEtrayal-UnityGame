using System.IO;
using UnityEngine;
using System;
using System.Linq;


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
            this.MapSprite = TDMapCreatorUtilis.GetSpritesFromTextureAsset(mapTexture).FirstOrDefault();
            return this;
        }

        public static void SaveTDMap(string absolutePath, GameObject mapPrefab, Texture2D mapTexture)
        {
            string relativePath = "Assets" + absolutePath.Replace(Application.dataPath, "");
            string soPath =  Path.Combine(relativePath, "TDMapSave.asset");
            string prefabPath = Path.Combine(relativePath, "MapPrefab.prefab");
            string texturePath = Path.Combine(relativePath, "MapImage.png");

            // Save Map Image as PNG
            try
            {
                var data = mapTexture.EncodeToPNG();
                Directory.CreateDirectory(Path.GetDirectoryName(texturePath));
                File.WriteAllBytes(texturePath, data);
            }
            catch (Exception e)
            {
                Debug.LogError("[TDMapCreator] Error saving map image: " + e);
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
                Debug.LogError("[TDMapCreator] Error saving map prefab: " + e);
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
                Debug.LogError("[TDMapCreator] Error saving map save file: " + e);
                return;
            }
        }
#endif
    }
}
