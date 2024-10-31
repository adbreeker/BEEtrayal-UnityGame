using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager currentScenesManager;

    [SerializeField] RawImage _fadeImage;
    [SerializeField] GraphicRaycaster _raycaster;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        currentScenesManager = this;
    }

    public void ChangeScene(string scene)
    {
        StartCoroutine(ChangeSceneCoroutine(scene));
    }

    IEnumerator ChangeSceneCoroutine(string scene)
    {
        _raycaster.enabled = true;
        while (_fadeImage.color.a != 1f)
        {
            Color updateColor = _fadeImage.color;

            updateColor.a = Mathf.MoveTowards(updateColor.a, 1f, 0.05f);

            _fadeImage.color = updateColor;
            yield return new WaitForFixedUpdate();
        }

        SceneManager.LoadScene(scene);
        yield return new WaitForSecondsRealtime(0.5f);

        while (_fadeImage.color.a != 0f)
        {
            Color updateColor = _fadeImage.color;

            updateColor.a = Mathf.MoveTowards(updateColor.a, 0f, 0.02f);

            _fadeImage.color = updateColor;
            yield return new WaitForFixedUpdate();
        }
        _raycaster.enabled = false;
    }
}
