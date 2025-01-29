using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager currentScenesManager;

    [Header("Doors speed")]
    public float doorSpeed;

    [Header("Fade elements:")]
    [SerializeField] GameObject _fadeBorder;
    [SerializeField] RectTransform _fadeUp;
    [SerializeField] RectTransform _fadeDown;
    [SerializeField] GraphicRaycaster _raycaster;

    private void Awake()
    {
        if (currentScenesManager != null) { Destroy(gameObject); }
        else
        {
            DontDestroyOnLoad(gameObject);
            currentScenesManager = this;
        }
    }

    public void ChangeScene(string scene)
    {
        StartCoroutine(ChangeSceneCoroutine(scene));
    }

    IEnumerator ChangeSceneCoroutine(string scene)
    {
        float height = ((RectTransform)transform).rect.height;

        _fadeUp.sizeDelta = new Vector2(_fadeUp.sizeDelta.x, height / 2f);
        _fadeDown.sizeDelta = new Vector2(_fadeDown.sizeDelta.x, height / 2f);

        _fadeUp.anchoredPosition = new Vector2(_fadeUp.anchoredPosition.x, (height / 4f + 5f));
        _fadeDown.anchoredPosition = new Vector2(_fadeDown.anchoredPosition.x, -(height / 4f + 5f));

        _raycaster.enabled = true;

        _fadeBorder.SetActive(true);
        _fadeUp.gameObject.SetActive(true);
        _fadeDown.gameObject.SetActive(true);

        Vector2 destinedUp = _fadeUp.anchoredPosition;
        destinedUp.y = -(height / 4f + 5f);
        Vector2 destinedDown = _fadeDown.anchoredPosition;
        destinedDown.y = (height / 4f + 5f);

        SoundManager.soundManager.PlaySound(SoundEnum.EFFECT_SLIDE_CLOSE);
        while(_fadeUp.anchoredPosition != destinedUp || _fadeDown.anchoredPosition != destinedDown)
        {
            _fadeUp.anchoredPosition = Vector2.MoveTowards(_fadeUp.anchoredPosition, destinedUp, doorSpeed * Time.unscaledDeltaTime);
            _fadeDown.anchoredPosition = Vector2.MoveTowards(_fadeDown.anchoredPosition, destinedDown, doorSpeed * Time.unscaledDeltaTime);
            yield return null;
        }

        SceneManager.LoadScene(scene);
        yield return new WaitForSecondsRealtime(0.5f);

        destinedUp.y = (height / 4f + 5f);
        destinedDown.y = -(height / 4f + 5f);

        SoundManager.soundManager.PlaySound(SoundEnum.EFFECT_SLIDE_OPEN);
        while (_fadeUp.anchoredPosition != destinedUp || _fadeDown.anchoredPosition != destinedDown)
        {
            _fadeUp.anchoredPosition = Vector2.MoveTowards(_fadeUp.anchoredPosition, destinedUp, doorSpeed * Time.unscaledDeltaTime);
            _fadeDown.anchoredPosition = Vector2.MoveTowards(_fadeDown.anchoredPosition, destinedDown, doorSpeed * Time.unscaledDeltaTime);
            yield return null;
        }

        _fadeUp.gameObject.SetActive(true);
        _fadeDown.gameObject.SetActive(true);
        _fadeBorder.SetActive(false);

        _raycaster.enabled = false;
    }
}
