#if UNITY_EDITOR
using UnityEngine;
using adbreeker.TDMapCreator;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Events;
using UnityEngine.UI;

namespace adbreeker.TDMapCreator
{
    public class UI_ChooseMapBackgroundPanel : MonoBehaviour
    {
        [SerializeField] GameObject _fieldPrefab;
        [SerializeField] Transform _fieldsHolder;

        public void SetupPanel(UnityAction<string> onBackgroundChooseMethod)
        {
            foreach (Transform child in _fieldsHolder)
            {
                Destroy(child.gameObject);
            }

            List<string> maps = GetBackgroundsPathsFromPath(Path.Combine(PackageVariables.GetPackageRelativePath(), "Resources", "Images", "Backgrounds"));
            foreach (string mapPath in maps)
            {
                GameObject fieldObj = Instantiate(_fieldPrefab, _fieldsHolder);
                UI_MapBackgroundField field = fieldObj.GetComponent<UI_MapBackgroundField>();
                field.Initialize(mapPath, () => onBackgroundChooseMethod(mapPath)); // Pass lambda that calls our method
            }

            // Adjust _fieldsHolder height based on grid layout and number of fields
            AdjustFieldsHolderHeight(maps.Count);
        }

        private void AdjustFieldsHolderHeight(int fieldCount)
        {
            RectTransform fieldsHolderRect = _fieldsHolder as RectTransform;
            if (fieldsHolderRect == null) return;

            // Get the GridLayoutGroup component
            GridLayoutGroup gridLayout = _fieldsHolder.GetComponent<GridLayoutGroup>();
            if (gridLayout == null) return;

            // Calculate required height
            int columnsPerRow = gridLayout.constraintCount > 0 ? gridLayout.constraintCount : 1;
            if (gridLayout.constraint == GridLayoutGroup.Constraint.FixedRowCount)
            {
                columnsPerRow = Mathf.CeilToInt((float)fieldCount / gridLayout.constraintCount);
            }

            int rows = Mathf.CeilToInt((float)fieldCount / columnsPerRow);
            
            float cellHeight = gridLayout.cellSize.y;
            float spacingY = gridLayout.spacing.y;
            float paddingTop = gridLayout.padding.top;
            float paddingBottom = gridLayout.padding.bottom;

            float totalHeight = paddingTop + paddingBottom + (rows * (cellHeight + spacingY)) - (0.5f * spacingY);

            // Set the new height
            fieldsHolderRect.sizeDelta = new Vector2(fieldsHolderRect.sizeDelta.x, totalHeight);
        }
       
        List<string> GetBackgroundsPathsFromPath(string path)
        {
            List<string> backgroundsPaths = new List<string>();

            try
            {
                // For packages directory, use AssetDatabase directly with the packages path
                if (Directory.Exists(path))
                {
                    string[] assetGuids = AssetDatabase.FindAssets("t:Texture2D", new[] { path });

                    foreach (string guid in assetGuids)
                    {
                        string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                        backgroundsPaths.Add(assetPath);
                    }

                }
                else
                {
                    PackageUtilis.PrintDebug(LogType.Warning, $"Directory does not exist: {path}");
                }
            }
            catch (System.Exception ex)
            {
                PackageUtilis.PrintDebug(LogType.Error, $"Error retrieving backgrounds from path '{path}': {ex.Message}");
            }

            PackageUtilis.PrintDebug(LogType.Log, $"Found {backgroundsPaths.Count} image files in '{path}'");
            return backgroundsPaths;
        }
    }
}
#endif