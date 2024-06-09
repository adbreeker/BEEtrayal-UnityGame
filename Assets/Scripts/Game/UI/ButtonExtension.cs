using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class ButtonExtension : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    [Header("Button hold")]
    [SerializeField] float _holdTime = 0.5f;
    [SerializeField] UnityEvent OnHold = new UnityEvent();
    [SerializeField] UnityEvent OnUnhold = new UnityEvent();

    [Header("Button multiclick")]
    [SerializeField] float _multiclickTime = 0.5f;
    [SerializeField] int _multiclickCount = 2;
    [SerializeField] UnityEvent OnMulticlick = new UnityEvent();

    //coroutines checking for event activators
    Coroutine _buttonHoldCoroutine;
    bool _buttonHolding = false;

    Coroutine _buttonMulticlickCoroutine;
    int _currentClicks = 0;

    public void OnPointerDown(PointerEventData eventData)
    {
        if(OnHold.GetPersistentEventCount() > 0)
        {
            if (_buttonHoldCoroutine == null)
            {
                _buttonHoldCoroutine = StartCoroutine(HoldingCoroutine());
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
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

    public void OnPointerClick(PointerEventData eventData)
    {
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
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        _buttonHolding = true;
        while(_buttonHolding)
        {
            yield return null;
            OnHold.Invoke();
        }
    }

    IEnumerator MulticlickCoroutine()
    {
        float timeElapsed = 0;
        while(timeElapsed < _multiclickTime)
        {
            timeElapsed += Time.deltaTime;
            if(_currentClicks >= _multiclickCount)
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
}
