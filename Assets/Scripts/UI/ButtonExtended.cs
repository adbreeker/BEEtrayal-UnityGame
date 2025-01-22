using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEditor;

[AddComponentMenu("UI/Button Extended", 31)]
public class ButtonExtended : Button
{
    [Header("Other graphic elements:")]
    public List<Graphic> otherGraphicElements;

    //button hold
    [SerializeField] float _holdTime = 0.5f;
    [SerializeField] UnityEvent OnHold = new UnityEvent();
    [SerializeField] UnityEvent OnUnhold = new UnityEvent();

    //button mutliclick
    [SerializeField] float _multiclickTime = 0.5f;
    [SerializeField] int _multiclickCount = 2;
    [SerializeField] UnityEvent OnMulticlick = new UnityEvent();

    //coroutines checking for event activators
    Coroutine _buttonHoldCoroutine;
    bool _buttonHolding = false;

    Coroutine _buttonMulticlickCoroutine;
    int _currentClicks = 0;

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);

        if(OnHold.GetPersistentEventCount() > 0)
        {
            if (_buttonHoldCoroutine == null)
            {
                _buttonHoldCoroutine = StartCoroutine(HoldingCoroutine());
            }
        }
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);

        if (_buttonHoldCoroutine != null)
        {
            StopCoroutine(_buttonHoldCoroutine);
            _buttonHoldCoroutine = null;
        }

        if (_buttonHolding)
        {
            _buttonHolding = false;
            OnUnhold.Invoke();
        }
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);

        if(OnMulticlick.GetPersistentEventCount() > 0)
        {
            _currentClicks++;
            if (_buttonMulticlickCoroutine == null)
            {
                _buttonMulticlickCoroutine = StartCoroutine(MulticlickCoroutine());
            }
            else
            {
                StopCoroutine(_buttonMulticlickCoroutine);
                _buttonMulticlickCoroutine = StartCoroutine(MulticlickCoroutine());
            }
        }
    }

    IEnumerator HoldingCoroutine()
    {
        float timeElapsed = 0;
        while(timeElapsed < _holdTime)
        {
            if(!GameParams.IsPointerOverUIObject(gameObject)) { OnPointerUp(null); }

            timeElapsed += Time.unscaledDeltaTime;

            yield return null;
        }
        _buttonHolding = true;
        while(_buttonHolding)
        {
            if (!GameParams.IsPointerOverUIObject(gameObject)) { OnPointerUp(null); }
            yield return null;
            OnHold.Invoke();
        }
    }

    IEnumerator MulticlickCoroutine()
    {
        float timeElapsed = 0;
        while(timeElapsed < _multiclickTime)
        {
            timeElapsed += Time.unscaledDeltaTime;

            if (_currentClicks >= _multiclickCount)
            {
                _currentClicks = 0;
                OnMulticlick.Invoke();
                _buttonMulticlickCoroutine = null;
                yield break;
            }
            yield return null;
        }
        _currentClicks = 0;
        _buttonMulticlickCoroutine = null;
    }

    protected override void DoStateTransition(SelectionState state, bool instant)
    {
        if (!gameObject.activeInHierarchy)
            return;

        Color tintColor;
        Sprite transitionSprite;
        string triggerName;

        switch (state)
        {
            case SelectionState.Normal:
                tintColor = colors.normalColor;
                transitionSprite = null;
                triggerName = animationTriggers.normalTrigger;
                break;
            case SelectionState.Highlighted:
                tintColor = colors.highlightedColor;
                transitionSprite = spriteState.highlightedSprite;
                triggerName = animationTriggers.highlightedTrigger;
                break;
            case SelectionState.Pressed:
                tintColor = colors.pressedColor;
                transitionSprite = spriteState.pressedSprite;
                triggerName = animationTriggers.pressedTrigger;
                break;
            case SelectionState.Selected:
                tintColor = colors.selectedColor;
                transitionSprite = spriteState.selectedSprite;
                triggerName = animationTriggers.selectedTrigger;
                break;
            case SelectionState.Disabled:
                tintColor = colors.disabledColor;
                transitionSprite = spriteState.disabledSprite;
                triggerName = animationTriggers.disabledTrigger;
                break;
            default:
                tintColor = Color.black;
                transitionSprite = null;
                triggerName = string.Empty;
                break;
        }

        switch (transition)
        {
            case Transition.ColorTint:
                StartColorTween(tintColor * colors.colorMultiplier, instant);
                break;
            case Transition.SpriteSwap:
                DoSpriteSwap(transitionSprite);
                break;
            case Transition.Animation:
                TriggerAnimation(triggerName);
                break;
        }
    }

    void StartColorTween(Color targetColor, bool instant)
    {
        if (targetGraphic != null)
        {
            targetGraphic.CrossFadeColor(targetColor, instant ? 0f : colors.fadeDuration, true, true);
        }
        foreach(Graphic otherGraphic in otherGraphicElements)
        {
            if(otherGraphic != null)
            {
                otherGraphic.CrossFadeColor(targetColor, instant ? 0f : colors.fadeDuration, true, true);
            }
        }
    }

    void DoSpriteSwap(Sprite newSprite)
    {
        if (image == null)
            return;

        image.overrideSprite = newSprite;
    }

    void TriggerAnimation(string triggername)
    {
#if PACKAGE_ANIMATION
            if (transition != Transition.Animation || animator == null || !animator.isActiveAndEnabled || !animator.hasBoundPlayables || string.IsNullOrEmpty(triggername))
                return;

            animator.ResetTrigger(m_AnimationTriggers.normalTrigger);
            animator.ResetTrigger(m_AnimationTriggers.highlightedTrigger);
            animator.ResetTrigger(m_AnimationTriggers.pressedTrigger);
            animator.ResetTrigger(m_AnimationTriggers.selectedTrigger);
            animator.ResetTrigger(m_AnimationTriggers.disabledTrigger);

            animator.SetTrigger(triggername);
#endif
    }
}

[CustomEditor(typeof(ButtonExtended), true)]
[CanEditMultipleObjects]
public class ButtonExtendedEditor : UnityEditor.UI.SelectableEditor
{
    SerializedProperty _otherGraphicElementsProperty;

    SerializedProperty m_OnClickProperty;

    SerializedProperty _holdTimeProperty;
    SerializedProperty _onHoldProperty;
    SerializedProperty _onUnholdProperty;
    SerializedProperty _multiclickTimeProperty;
    SerializedProperty _multiclickCountProperty;
    SerializedProperty _onMulticlickProperty;

    protected override void OnEnable()
    {
        base.OnEnable();
        _otherGraphicElementsProperty = serializedObject.FindProperty("otherGraphicElements");

        m_OnClickProperty = serializedObject.FindProperty("m_OnClick");

        _holdTimeProperty = serializedObject.FindProperty("_holdTime");
        _onHoldProperty = serializedObject.FindProperty("OnHold");
        _onUnholdProperty = serializedObject.FindProperty("OnUnhold");
        _multiclickTimeProperty = serializedObject.FindProperty("_multiclickTime");
        _multiclickCountProperty = serializedObject.FindProperty("_multiclickCount");
        _onMulticlickProperty = serializedObject.FindProperty("OnMulticlick");
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();
        EditorGUILayout.PropertyField(_otherGraphicElementsProperty);

        EditorGUILayout.Space(10f);
        EditorGUILayout.LabelField("Button Click", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(m_OnClickProperty);

        EditorGUILayout.Space(10f);
        EditorGUILayout.LabelField("Button Hold", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(_holdTimeProperty);
        EditorGUILayout.PropertyField(_onHoldProperty);
        EditorGUILayout.PropertyField(_onUnholdProperty);

        EditorGUILayout.Space(10f);
        EditorGUILayout.LabelField("Button Multiclick", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(_multiclickTimeProperty);
        EditorGUILayout.PropertyField(_multiclickCountProperty);
        EditorGUILayout.PropertyField(_onMulticlickProperty);

        serializedObject.ApplyModifiedProperties();
    }
}
