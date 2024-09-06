using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataInitialization : MonoBehaviour
{
    private void Awake()
    {
        if(!PlayerPrefs.HasKey("Honey"))
        {
            PlayerPrefs.SetInt("Honey", 0);
        }
    }
}
