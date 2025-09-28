#if UNITY_EDITOR
using UnityEngine;
using adbreeker.TDMapCreator;
using UnityEngine.UI;
using UnityEditor;
using System.IO;
using System;
using UnityEngine.Events;

namespace adbreeker.TDMapCreator
{
    public class UI_MapBackgroundField : MonoBehaviour
    {
        [SerializeField] Button _selectBackgroundButton;
        [SerializeField] RawImage _backgroundPreviewImage;
        [SerializeField] Text _fieldLabel;

        public void Initialize(string imagePath, Action buttonMethod)
        {
            Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(imagePath);
            _backgroundPreviewImage.texture = texture;
            _fieldLabel.text = Path.GetFileNameWithoutExtension(imagePath).ToUpper() + $" ({texture.width}:{texture.height})";

            _selectBackgroundButton.onClick.AddListener(buttonMethod.Invoke);
        }
    }													
}
#endif