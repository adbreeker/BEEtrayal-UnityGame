using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoryManager : MonoBehaviour
{
    [Header("Images:")]
    [SerializeField] List<GameObject> _storyImages = new List<GameObject>();

    [Header("TextField")]
    [SerializeField] TextMeshProUGUI _storyTextField;
    [SerializeField, TextArea(0, 6)] List<string> _stories;

    [Header("Fade")]
    [SerializeField] RawImage _fade;

    int _globalStoryIndex = 0;
    Coroutine _storytelling = null;
    Coroutine _skipListener = null;

    private void Start()
    {
        _fade.color = Color.black;
        _storyTextField.text = "";
        _storytelling = StartCoroutine(StorytellingCoroutine(_globalStoryIndex, 0.3f));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(_skipListener == null)
            {
                _skipListener = StartCoroutine(SkipListener());
            }
            else
            {
                StopCoroutine(SkipListener());
                _skipListener = null;
                SkipStory();
            }
        }
    }

    IEnumerator StorytellingCoroutine(int storyIndex, float startDeley)
    { 
        yield return new WaitForSecondsRealtime(startDeley);

        //fade and change --------------------------------------
        Color fadeColor = _fade.color;

        while (_fade.color.a < 1f)
        {
            fadeColor = _fade.color;
            fadeColor.a += Time.deltaTime * 2f;
            _fade.color = fadeColor;
            yield return null;
        }

        fadeColor.a = 1;
        _fade.color = fadeColor;

        for (int i = 0; i < _storyImages.Count; i++)
        {
            if (i == storyIndex) { _storyImages[i].SetActive(true); }
            else { _storyImages[i].SetActive(false); }
        }

        _storyTextField.text = "";

        while (_fade.color.a > 0f)
        {
            fadeColor = _fade.color;
            fadeColor.a -= Time.deltaTime * 2f;
            _fade.color = fadeColor;
            yield return null;
        }

        fadeColor.a = 0f;
        _fade.color = fadeColor;
        //fade and change ----------------------------------------

        yield return new WaitForSecondsRealtime(1f);

        for(int i = 0; i < _stories[storyIndex].Length; i++)
        {
            _storyTextField.text += _stories[storyIndex][i];
            yield return new WaitForSecondsRealtime(0.05f);
        }

        yield return new WaitForSecondsRealtime(2f);

        _globalStoryIndex++;

        if(_globalStoryIndex < _storyImages.Count)
        {
            _storytelling = StartCoroutine(StorytellingCoroutine(_globalStoryIndex, 0));
        }
        else
        {
            PlayerPrefs.SetInt("Story", 1);
            ScenesManager.currentScenesManager.ChangeScene("Menu");
        }
    }

    IEnumerator SkipListener()
    {
        yield return new WaitForSecondsRealtime(0.3f);
        _skipListener = null;
    }

    void SkipStory()
    {
        if(_globalStoryIndex >= _storyImages.Count) 
        {
            PlayerPrefs.SetInt("Story", 1);
            ScenesManager.currentScenesManager.ChangeScene("Menu");
            if (_storytelling != null) { StopCoroutine(_storytelling); }
            return;
        }

        if(_storytelling != null ) { StopCoroutine(_storytelling); }

        for (int i = 0; i < _storyImages.Count; i++)
        {
            if (i == _globalStoryIndex) { _storyImages[i].SetActive(true); }
            else { _storyImages[i].SetActive(false); }
        }

        _storyTextField.text = _stories[_globalStoryIndex];

        _globalStoryIndex++;

        if( _globalStoryIndex < _storyImages.Count)
        {
            _storytelling = StartCoroutine(StorytellingCoroutine(_globalStoryIndex, 2.0f));
        }
        else if(_globalStoryIndex ==  _storyImages.Count) 
        {
            _storytelling = StartCoroutine(DeleyedSceneChange(2.0f));
        }
    }

    IEnumerator DeleyedSceneChange(float deley)
    {
        yield return new WaitForSecondsRealtime(deley);
        ScenesManager.currentScenesManager.ChangeScene("Menu");
    }
}
