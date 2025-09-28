#if UNITY_EDITOR
using adbreeker.TDMapCreator;
using UnityEngine;
using UnityEngine.UI;

namespace adbreeker.TDMapCreator
{
    public class UI_MapSetupPanel : MonoBehaviour
    {
        [Header("Main panel elements:")]
        [SerializeField] InputField _inputResWidth;
        [SerializeField] InputField _inputResHeight;
        [SerializeField] RawImage _backgroundPreview;

        [Header("Other elements:")]
        [SerializeField] UI_ChooseMapBackgroundPanel _chooseMapBackgroundPanel;

        public void Button_StartCreating()
        {
            int resWidth = 1920;
            int resHeight = 1080;

            if (!string.IsNullOrWhiteSpace(_inputResWidth.text) && int.TryParse(_inputResWidth.text, out var w) && w > 0) { resWidth = w; }
            if (!string.IsNullOrWhiteSpace(_inputResHeight.text) && int.TryParse(_inputResHeight.text, out var h) && h > 0) { resHeight = h; }


            TDMapCreatorManager.Instance.StartCreating(resWidth, resHeight, _backgroundPreview.texture);
        }

        public void Button_OpenChangeMapBackgroundPanel()
        {
            _chooseMapBackgroundPanel.SetupPanel(ChangeMapBackground);
            _chooseMapBackgroundPanel.gameObject.SetActive(true);
        }

        public void ChangeMapBackground(string backgroundPath)
        {
            PackageUtilis.PrintDebug(LogType.Log, $"Changing map background to image at path: {backgroundPath}");
            Texture2D texture = UnityEditor.AssetDatabase.LoadAssetAtPath<Texture2D>(backgroundPath);
            _backgroundPreview.texture = texture;
            _chooseMapBackgroundPanel.gameObject.SetActive(false);  
        }
    }													
}
#endif