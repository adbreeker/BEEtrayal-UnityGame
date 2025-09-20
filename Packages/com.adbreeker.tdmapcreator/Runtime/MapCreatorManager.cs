using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
public class MapCreatorManager : MonoBehaviour
{
    public static MapCreatorManager Instance;

    [Header("Map Elements:")]
    [SerializeField] Transform _mapRoot;
    [SerializeField] SpriteRenderer _background;

    [Header("New Map Settup UI:")]
    [SerializeField] GameObject _panelNewMapSettup;
    [SerializeField] InputField _inputResWidth;
    int _currentResWidth = 1920;
    [SerializeField] InputField _inputResHeight;
    int _currentResHeight = 1080;
    [SerializeField] RawImage _backgroundPreview;

    void Awake()
    {
        EditorSceneManager.playModeStartScene = null;
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }

    public void Button_ChangeBackgroundPreview()
    {

    }

    public void Button_StartCreating()
    {
        // Parse resolution
        if (!string.IsNullOrWhiteSpace(_inputResWidth.text) && int.TryParse(_inputResWidth.text, out var w) && w > 0)
            _currentResWidth = w;
        if (!string.IsNullOrWhiteSpace(_inputResHeight.text) && int.TryParse(_inputResHeight.text, out var h) && h > 0)
            _currentResHeight = h;

        Debug.Log($"Starting new map with resolution {_currentResWidth}x{_currentResHeight}");

        // Assign background sprite
        var sprite = MapCreatorUtilis.GetSpritesFromTextureAsset(_backgroundPreview.texture).FirstOrDefault();
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

            Debug.Log($"Background sprite size: {spritePxWidth}x{spritePxHeight} px; applied scale to fit {_currentResWidth}x{_currentResHeight}");
        }
        else
        {
            Debug.LogWarning("No sprite found for background preview texture.");
        }

        _panelNewMapSettup.SetActive(false);
    }

    public Texture2D ExportMapImage(string absolutePath)
    {
        if (_mapRoot == null || _background == null || _background.sprite == null)
        {
            Debug.LogWarning("ExportMapImage: Map root or background sprite is not set.");
            return null;
        }

        var tex = MapCreatorUtilis.CaptureMapAlignedToBackground(
            _mapRoot,
            _background,
            _currentResWidth,
            _currentResHeight
        );

        if (tex == null)
        {
            Debug.LogWarning("ExportMapImage: capture failed.");
            return null;
        }

        string targetPath = absolutePath + ".png";
        var data = tex.EncodeToPNG();
        Directory.CreateDirectory(Path.GetDirectoryName(targetPath));
        File.WriteAllBytes(targetPath, data);

        return tex;
    }

    public void ExportMapPrefab(string absolutePath)
    {
        if (_mapRoot == null)
        {
            Debug.LogWarning("ExportMapPrefab: Map root is not set.");
            return;
        }

        var targetPath = absolutePath + ".tdmap.prefab";

        // If _mapRoot is a prefab instance, unpack it completely IN-PLACE so there are no prefab links.
        if (PrefabUtility.IsPartOfPrefabInstance(_mapRoot.gameObject))
        {
            PrefabUtility.UnpackPrefabInstance(_mapRoot.gameObject, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
        }

        PrefabUtility.SaveAsPrefabAsset(_mapRoot.gameObject, targetPath);
    }
}
#endif