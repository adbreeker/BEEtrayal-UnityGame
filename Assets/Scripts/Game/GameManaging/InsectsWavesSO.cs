using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "InsectsWaves", menuName = "Insects Waves SO")]
public class InsectsWavesSO : ScriptableObject
{
    [Serializable]
    public class InsectsWave
    {
        public List<GameObject> insectsInWave = new List<GameObject>();
    }

    public List<InsectsWave> insectsWaves = new List<InsectsWave>();

    public int InsectsInWavesCount()
    {
        int count = 0;
        foreach(InsectsWave wave in insectsWaves) 
        {
            foreach(GameObject insect in wave.insectsInWave)
            {
                if(insect != null)
                {
                    count++;
                }
            }
        }
        return count;
    }

    public int InsectsInWavesValueNetto()
    {
        int value = 0;
        foreach (InsectsWave wave in insectsWaves)
        {
            foreach (GameObject insect in wave.insectsInWave)
            {
                if(insect != null)
                {
                    value += insect.GetComponent<InsectController>().value;
                }
            }
        }
        return value;
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(InsectsWavesSO))]
[CanEditMultipleObjects]
public class InsectsWavesSOEditor : Editor
{
    InsectsWavesSO _script;

    SerializedProperty _insectsWavesProperty;

    public void OnEnable()
    {
        _script = (InsectsWavesSO)target;
        _insectsWavesProperty = serializedObject.FindProperty("insectsWaves");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Calculate counts
        int insectCount = _script.InsectsInWavesCount();
        int insectsValue = _script.InsectsInWavesValueNetto();

        // Draw insect count and total value
        EditorGUILayout.Space(10f);
        EditorGUILayout.LabelField("Current Waves Summary", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Total Insects in Waves:", insectCount.ToString());
        EditorGUILayout.LabelField("Total Value (Netto):", insectsValue.ToString());

        EditorGUILayout.Space(20f);
        EditorGUILayout.LabelField("Waves:", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(_insectsWavesProperty);

        serializedObject.ApplyModifiedProperties();
    }
}

//drawer of InsectsWave - changing label in list from Element to Wave
[CustomPropertyDrawer(typeof(InsectsWavesSO.InsectsWave))]
public class InsectsWaveDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Change the label to "Wave X" based on the property's index in the parent array
        SerializedProperty parent = property.serializedObject.FindProperty("insectsWaves");
        int index = GetIndex(property, parent);
        label.text = $"Wave {index + 1}";

        // Draw the default property field with the updated label
        EditorGUI.PropertyField(position, property, label, true);
    }

    private int GetIndex(SerializedProperty property, SerializedProperty parent)
    {
        for (int i = 0; i < parent.arraySize; i++)
        {
            if (SerializedProperty.EqualContents(parent.GetArrayElementAtIndex(i), property))
                return i;
        }
        return -1; // Should not reach here
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, true);
    }
}
#endif
