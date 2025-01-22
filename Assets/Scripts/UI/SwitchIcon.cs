using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchIcon : MonoBehaviour
{
    [Header("Icons to switch between:")]
    [SerializeField] List<Graphic> icons = new List<Graphic>();
    int _iconIndex = 0;

    public void Switch()
    {
        icons[_iconIndex].enabled = false;
        _iconIndex = (_iconIndex + 1) % icons.Count;
        icons[_iconIndex].enabled = true;
    }
}
