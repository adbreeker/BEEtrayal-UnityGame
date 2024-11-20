using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingBar : MonoBehaviour
{
    [SerializeField] RectTransform _loadingMask;
    [SerializeField] RectTransform _loadingBar;

    public void SetLoading(float loadingStatus)
    {
        loadingStatus = Mathf.Clamp01(loadingStatus);
        float lenghtOfMask = _loadingMask.rect.width;

        Vector2 newSize = _loadingBar.sizeDelta;
        newSize.x = loadingStatus * lenghtOfMask;

        _loadingBar.sizeDelta = newSize;
    }
}
