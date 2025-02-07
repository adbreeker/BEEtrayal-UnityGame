using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEditor;
using System;
using System.Linq;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    public static SoundManager soundManager;

    [Serializable]
    public class Sound
    {
        public AudioClip soundClip;
        public string soundName;
        public SoundEnum soundEnum;
    }

    public int managerPriority = 0;
    [SerializeField] GameObject _audioPrefab;
    [SerializeField] List<Sound> _sounds = new List<Sound>();

    List<AudioSourceController> _activeAudios = new List<AudioSourceController>();
    bool _isMuted = false;

    void Awake()
    {
        if (soundManager == null)
        {
            soundManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (soundManager != this)
        {
            if(soundManager.managerPriority >= this.managerPriority) 
            {
                Destroy(gameObject); 
            }
            else
            {
                DestroyImmediate(soundManager.gameObject);
                soundManager = this;
                DontDestroyOnLoad(gameObject);
            }
        }
    }

    public void PlaySound(SoundEnum soundToPlay, bool randomize = false)
    {
        AudioSourceController acc = Instantiate(_audioPrefab).GetComponent<AudioSourceController>();
        _activeAudios.Add(acc);

        if (randomize) { acc.SetPitch(UnityEngine.Random.Range(0.9f, 1.1f)); }

        acc.SetSpatial(0f);
        acc.SetMute(_isMuted);
        acc.PlayAndDestroy(_sounds[(int)soundToPlay].soundClip);
    }

    public void PlaySound3D(SoundEnum soundToPlay, Vector3 position, bool randomize = false)
    {
        AudioSourceController acc = Instantiate(_audioPrefab, position, Quaternion.identity).GetComponent<AudioSourceController>();
        _activeAudios.Add(acc);

        if (randomize) { acc.SetPitch(UnityEngine.Random.Range(0.9f, 1.1f)); }

        acc.SetSpatial(1f);
        acc.SetMute(_isMuted);
        acc.PlayAndDestroy(_sounds[(int)soundToPlay].soundClip);
    }

    public void ChangeSoundsMute(bool mute)
    {
        _isMuted = mute;
        GetComponent<AudioSource>().mute = mute;
        foreach(AudioSourceController acc in _activeAudios)
        {
            if(acc != null)
            {
                acc.SetMute(mute);
            }
        }
    }

    public void DestroyAudioSource(AudioSourceController audioSourceController)
    {
        _activeAudios.Remove(audioSourceController);
        Destroy(audioSourceController.gameObject);
    }

#if UNITY_EDITOR
    private void UpdateSoundsCollection()
    {
        //ordering collection alphabetically
        _sounds = _sounds.OrderBy(sound => sound.soundName).ToList();

        //updating enums list
        Debug.Log("Updating sound enums");

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("public enum SoundEnum");
        sb.AppendLine("{");

        for(int i = 0; i < _sounds.Count; i++) 
        {
            if(i != _sounds.Count - 1)
            {
                if (_sounds[i].soundName == "" || _sounds[i].soundClip == null)
                {
                    sb.AppendLine($"    Null{i},");
                }
                else
                {
                    sb.AppendLine($"    {_sounds[i].soundName.Replace(" ", "_")},");
                }
            }
            else
            {
                if (_sounds[i].soundName == "" || _sounds[i].soundClip == null)
                {
                    sb.AppendLine($"    Null{i}");
                }
                else
                {
                    sb.AppendLine($"    {_sounds[i].soundName.Replace(" ", "_")}");
                }
            }
        }

        sb.AppendLine("}");

        string[] guids = AssetDatabase.FindAssets("t:Script SoundManager");
        if (guids.Length > 0)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[0]);
            path = System.IO.Path.GetDirectoryName(path) + "/SoundEnum.cs";
            System.IO.File.WriteAllText(path, sb.ToString());
            AssetDatabase.Refresh();
        }

        //applying right enum to every sound in collection
        for(int i = 0; i < _sounds.Count; i++)
        {
            _sounds[i].soundEnum = (SoundEnum)i;
        }

        EditorUtility.SetDirty(gameObject);
        AssetDatabase.SaveAssetIfDirty(gameObject);
        AssetDatabase.Refresh();
    }
#endif
}

#if UNITY_EDITOR
[CustomEditor(typeof(SoundManager))]
public class SoundManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Draw all properties except _sounds
        DrawPropertiesExcluding(serializedObject, "_sounds");

        // Reference to target SoundManager
        SoundManager soundManager = (SoundManager)target;

        // Allow _sounds editing only in prefab asset mode
        if (PrefabUtility.IsPartOfPrefabAsset(soundManager.gameObject))
        {
            SerializedProperty soundsProperty = serializedObject.FindProperty("_sounds");
            EditorGUILayout.PropertyField(soundsProperty, true);

            EditorGUILayout.Space(10f);
            
            if(GUILayout.Button("Update sounds"))
            {
                var method = typeof(SoundManager).GetMethod("UpdateSoundsCollection", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                method?.Invoke(soundManager, null);
            }
        }
        else
        {
            EditorGUILayout.HelpBox("Sounds editing is allowed only in the prefab editor.", MessageType.Info);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif