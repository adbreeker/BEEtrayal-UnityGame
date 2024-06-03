using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave 1", menuName = "Waves")]
public class InsectsWave : ScriptableObject
{
    public List<GameObject> insectsInWave = new List<GameObject>();
}
