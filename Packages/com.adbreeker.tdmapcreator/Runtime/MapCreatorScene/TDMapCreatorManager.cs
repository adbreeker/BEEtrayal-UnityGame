#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;


namespace adbreeker.TDMapCreator
{
    public class TDMapCreatorManager : MonoBehaviour
    {
        public static TDMapCreatorManager Instance { get; private set; }

        [SerializeField] PathDrawer _pathDrawer;

        [Header("Map Elements:")]
        [SerializeField] Transform _mapRoot;
        [SerializeField] SpriteRenderer _background;
        [SerializeField] Transform _decorationsRoot;
        [SerializeField] Transform _pathsRoot;

        [Header("UI Elements:")]
        [SerializeField] UI_MapSetupPanel _panelNewMapSetup;
        int _currentResWidth = 1920;
        int _currentResHeight = 1080;


        void Awake()
        {
            EditorSceneManager.playModeStartScene = null;
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
                return;
            }
            Instance = this;

            string loadPath = SessionState.GetString(PackageVariables.EnvKeys.SESSIONSTATE_MAP_LOAD_PATH, "");
            if (loadPath != "")
            {
                SessionState.SetString(PackageVariables.EnvKeys.SESSIONSTATE_MAP_LOAD_PATH, "");
                LoadMap(loadPath);
            }
        }

        public void StartCreating(int resolutionWidth, int resolutionHeight, Texture backgroundImage)
        {
            _currentResWidth = resolutionWidth;
            _currentResHeight = resolutionHeight;

            PackageUtilis.PrintDebug(LogType.Log, $"Starting new map with resolution {_currentResWidth}x{_currentResHeight}");

            // Assign background sprite
            var sprite = PackageUtilis.GetSpritesFromTextureAsset(backgroundImage).FirstOrDefault();
            _background.sprite = sprite;

            // Scale sprite to match desired resolution (in pixels)
            if (sprite != null)
            {
                var rect = sprite.rect; // pixel rect
                float spritePxWidth = rect.width;
                float spritePxHeight = rect.height;

                if (spritePxWidth > 0f && spritePxHeight > 0f)
                {
                    float scaleX = _currentResWidth / spritePxWidth;
                    float scaleY = _currentResHeight / spritePxHeight;
                    _background.transform.localScale = new Vector3(scaleX, scaleY, 1f);
                }
            }
            else
            {
                PackageUtilis.PrintDebug(LogType.Warning, "No sprite found for background preview texture.");
            }

            _panelNewMapSetup.gameObject.SetActive(false);
        }

        void LoadMap(string loadPath)
        {
            TDMapSaveSO saveSO = AssetDatabase.LoadAssetAtPath<TDMapSaveSO>(loadPath);

            TDMap loadedMapPrefab = Instantiate(saveSO.MapPrefab).GetComponent<TDMap>();
            loadedMapPrefab.gameObject.name = _mapRoot.name;
            Destroy(_mapRoot.gameObject);

            //setting references - TDMapCreatorManager
            _mapRoot = loadedMapPrefab.transform;
            _background = loadedMapPrefab.BackgroundRenderer;
            _decorationsRoot = loadedMapPrefab.DecorationsHolder;
            _pathsRoot = loadedMapPrefab.PathsHolder;

            //setting references - PathDrawer
            _pathDrawer.pathsHolder = _pathsRoot;

            _panelNewMapSetup.gameObject.SetActive(false);
        }

        public Texture2D GetMapImage()
        {
            if (_mapRoot == null || _background == null || _background.sprite == null)
            {
                PackageUtilis.PrintDebug(LogType.Warning, "ExportMapImage: Map root or background sprite is not set.");
                return null;
            }

            Texture2D tex = PackageUtilis.CaptureMapAlignedToBackground(
                _mapRoot,
                _background,
                _currentResWidth,
                _currentResHeight
            );

            if (tex == null)
            {
                PackageUtilis.PrintDebug(LogType.Warning, "ExportMapImage: capture failed.");
                return null;
            }

            return tex;
        }

        public GameObject GetMapObject()
        {
            if (_mapRoot == null)
            {
                PackageUtilis.PrintDebug(LogType.Warning, "ExportMapPrefab: Map root is not set.");
                return null;
            }
            else
            {
                return _mapRoot.gameObject;
            }
        }
    }
}
#endif