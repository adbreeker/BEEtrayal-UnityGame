using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartManager : MonoBehaviour
{
    [SerializeField] LoadingBar _loading;

    void Start()
    {
        StartCoroutine(LoadingCoroutine());
    }

    IEnumerator LoadingCoroutine()
    {
        float loadingStatus = 0f;

        while(loadingStatus < 1f)
        {
            loadingStatus += Time.unscaledDeltaTime / 1.5f;
            _loading.SetLoading(loadingStatus);

            yield return null;
        }

        if(PlayerPrefs.GetInt("Story") == 0)
        {
            ScenesManager.currentScenesManager.ChangeScene("Story");
        }
        else
        {
            ScenesManager.currentScenesManager.ChangeScene("Menu");
        }
    }
}
