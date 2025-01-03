using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using static UnityEngine.GraphicsBuffer;
using UnityEditor;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    public static SoundManager soundManager;

    [System.Serializable]
    public class Sound
    {
        public AudioClip soundClip;
        public string soundName;
        public SoundEnum soundEnum;
    }

    [SerializeField] GameObject _audioPrefab;
    [SerializeField, HideInInspector] List<Sound> _sounds = new List<Sound>();

    List<AudioSource> _activeAudios = new List<AudioSource>();

    void Awake()
    {
        if (soundManager == null)
        {
            soundManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (soundManager != this)
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(SoundEnum soundToPlay)
    {
        AudioSource audioSource = Instantiate(_audioPrefab).GetComponent<AudioSource>();
        audioSource.clip = _sounds[(int)soundToPlay].soundClip;
        audioSource.Play();
        Destroy(audioSource.gameObject, audioSource.clip.length);
    }

    public void ChangeSoundsMute(bool mute)
    {
        GetComponent<AudioSource>().mute = mute;
        foreach(AudioSource audioSource in _activeAudios)
        {
            audioSource.mute = mute;
        }
    }

    //Dynamic updates of sounds enums --------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
    private void OnValidate()
    {
        if(!Application.isPlaying)
        {
            UnityEditor.EditorApplication.delayCall -= UpdateEnum;
            UnityEditor.EditorApplication.delayCall += UpdateEnum;
        }
    }

    private void UpdateEnum()
    {
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

        string[] guids = UnityEditor.AssetDatabase.FindAssets("t:Script SoundManager");
        if (guids.Length > 0)
        {
            string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[0]);
            path = System.IO.Path.GetDirectoryName(path) + "/SoundEnum.cs";
            System.IO.File.WriteAllText(path, sb.ToString());
            UnityEditor.AssetDatabase.Refresh();
        }

        for(int i = 0; i < _sounds.Count; i++)
        {
            _sounds[i].soundEnum = (SoundEnum)i;
        }
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
        }
        else
        {
            EditorGUILayout.HelpBox("Sounds editing is allowed only in the prefab editor.", MessageType.Info);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif