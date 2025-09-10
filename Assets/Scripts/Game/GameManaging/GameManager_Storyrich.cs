using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager_Storyrich : GameManager
{
    [Space(20f), Header("Story properties:")]
    [SerializeField] GameObject _storyPanel;
    [SerializeField] TextMeshProUGUI _storyText;
    [SerializeField] Button _infoPanelButton;

    [SerializeField] List<StorySegment> _storySegments;

    [System.Serializable]
    public class StorySegment
    {
        [Header("Segment fields:")]
        public string segmentName;
        [Space(5f),TextArea(1, 10)]
        public string monologue;
        [Space(10f)]
        public GameObject linkedObject;
    }

    bool _isStoryOngoing = true;
    int _globalStoryIndex = 0;
    Coroutine _storyCoroutine = null;
    Coroutine _skipListener = null;

    private void Start()
    {
        _globalStoryIndex = 0;
        _isStoryOngoing = true;
        Time.timeScale = 0;

        _storyText.text = "";
        _storyCoroutine = StartCoroutine(StoryCoroutine(_globalStoryIndex, 1f));
    }

    protected override void Update()
    {
        if(_isStoryOngoing)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (_skipListener == null)
                {
                    _skipListener = StartCoroutine(SkipListener());
                }
                else
                {
                    StopCoroutine(SkipListener());
                    _skipListener = null;
                    SkipStorySegment();
                }
            }
        }
        else
        {
            base.Update();
        }
    }

    IEnumerator SkipListener()
    {
        yield return new WaitForSecondsRealtime(0.3f);
        _skipListener = null;
    }

    IEnumerator StoryCoroutine(int storyIndex, float startDeley)
    {
        yield return new WaitForSecondsRealtime(startDeley);

        _storyText.text = "";
        for(int i = 0; i < _storySegments.Count; i++)
        {
            if (_storySegments[i].linkedObject != null) { _storySegments[i].linkedObject.SetActive(false); }
        }
        yield return new WaitForSecondsRealtime(0.5f);

        if(_storySegments[storyIndex].linkedObject != null)
        {
            _storySegments[storyIndex].linkedObject.SetActive(true);
        }
        foreach (string word in _storySegments[storyIndex].monologue.Split(" "))
        {
            SoundManager.soundManager.PlaySound(SoundEnum.EFFECT_BEEBUZZ1, true);
            _storyText.text += word + " ";
            yield return new WaitForSecondsRealtime(Mathf.Min(word.Length * 0.07f, 0.5f));
        }

        yield return new WaitForSecondsRealtime(2f);

        _storyText.text = "";
        if(_storySegments[storyIndex].linkedObject != null){ _storySegments[storyIndex].linkedObject.SetActive(false); }
        _globalStoryIndex++;

        if (_globalStoryIndex < _storySegments.Count)
        {
            _storyCoroutine = StartCoroutine(StoryCoroutine(_globalStoryIndex, 0));
        }
        else
        {
            StoryFinish();
        }
    }

    void SkipStorySegment()
    {
        if (_globalStoryIndex >= _storySegments.Count)
        {
            if (_storyCoroutine != null) { StopCoroutine(_storyCoroutine); }
            StoryFinish();
            return;
        }

        if (_storyCoroutine != null) { StopCoroutine(_storyCoroutine); }

        for (int i = 0; i < _storySegments.Count; i++)
        {
            if (i == _globalStoryIndex && _storySegments[i].linkedObject != null) { _storySegments[i].linkedObject.SetActive(true); }
            else if (_storySegments[i].linkedObject != null) { _storySegments[i].linkedObject.SetActive(false); }
        }
        _storyText.text = _storySegments[_globalStoryIndex].monologue;

        _globalStoryIndex++;

        if (_globalStoryIndex < _storySegments.Count)
        {
            _storyCoroutine = StartCoroutine(StoryCoroutine(_globalStoryIndex, 2.0f));
        }
        else if (_globalStoryIndex == _storySegments.Count)
        {
            StartCoroutine(DelayedStoryFinish(3.0f));
        }
    }

    IEnumerator DelayedStoryFinish(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        if(_isStoryOngoing) { StoryFinish(); }
    }

    void StoryFinish()
    {
        _storyCoroutine = null;
        _isStoryOngoing = false;
        _infoPanelButton.interactable = true;
        Time.timeScale = 1;
        _storyPanel.SetActive(false);
    }
}
