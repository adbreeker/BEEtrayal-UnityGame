using System.IO;
using UnityEngine;
using System;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class TDMapSaveSO : ScriptableObject
{
    [field: SerializeField] public GameObject MapPrefab { get; private set; }
    private string _mapPrefabPath;
    [field: SerializeField] public Texture2D MapTexture { get; private set; }
    private string _mapTexturePath;
    [field: SerializeField] public Sprite MapSprite { get; private set; }

    public void Init(GameObject mapPrefab, Texture2D mapTexture)
    {
        this.MapPrefab = mapPrefab;
        this.MapTexture = mapTexture;
    }

#if UNITY_EDITOR
    public void SaveAsAsset(string path)
    {
        string targetPath = "Assets" + path.Replace(Application.dataPath, "") + ".asset";
        try
        {
            AssetDatabase.CreateAsset(this, targetPath);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to save TDMapSaveSO asset at {targetPath}: {e.Message}");
            return;
        }
    }
#endif
}
