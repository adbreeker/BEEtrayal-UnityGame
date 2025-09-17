using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MapCreatorManager : MonoBehaviour
{
    [Header("Map Elements:")]
    [SerializeField] SpriteRenderer _background;

    [Header("New Map Settup UI:")]
    [SerializeField] GameObject _panelNewMapSettup;
    [SerializeField] InputField _inputResWidth;
    [SerializeField] InputField _inputResHeight;
    [SerializeField] RawImage _backgroundPreview;

    public void Button_ChangeBackgroundPreview()
    {

    }

    public void Button_StartCreating()
    {
        // Parse resolution
        int resWidth = 1920;
        int resHeight = 1080;
        if (!string.IsNullOrWhiteSpace(_inputResWidth.text) && int.TryParse(_inputResWidth.text, out var w) && w > 0)
            resWidth = w;
        if (!string.IsNullOrWhiteSpace(_inputResHeight.text) && int.TryParse(_inputResHeight.text, out var h) && h > 0)
            resHeight = h;

        Debug.Log($"Starting new map with resolution {resWidth}x{resHeight}");

        // Assign background sprite
        var sprite = GetSpritesFromTextureAsset(_backgroundPreview.texture).FirstOrDefault();
        _background.sprite = sprite;

        // Scale sprite to match desired resolution (in pixels)
        if (sprite != null)
        {
            var rect = sprite.rect; // pixel rect
            float spritePxWidth = rect.width;
            float spritePxHeight = rect.height;

            if (spritePxWidth > 0f && spritePxHeight > 0f)
            {
                float scaleX = resWidth / spritePxWidth;
                float scaleY = resHeight / spritePxHeight;
                _background.transform.localScale = new Vector3(scaleX, scaleY, 1f);
            }

            Debug.Log($"Background sprite size: {spritePxWidth}x{spritePxHeight} px; applied scale to fit {resWidth}x{resHeight}");
        }
        else
        {
            Debug.LogWarning("No sprite found for background preview texture.");
        }

        _panelNewMapSettup.SetActive(false);
    }

    public static Sprite[] GetSpritesFromTextureAsset(Object textureAsset)
    {
        if (textureAsset == null) return new Sprite[0];
        string path = AssetDatabase.GetAssetPath(textureAsset);
        if (string.IsNullOrEmpty(path)) return new Sprite[0];

        // For Multiple sprites, this returns each Sprite sub-asset; for Single, it returns one.
        return AssetDatabase.LoadAllAssetRepresentationsAtPath(path)
            .OfType<Sprite>()
            .DefaultIfEmpty(AssetDatabase.LoadAssetAtPath<Sprite>(path))
            .Where(s => s != null)
            .ToArray();
    }
}
