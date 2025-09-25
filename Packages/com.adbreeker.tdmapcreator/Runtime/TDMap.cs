using NUnit.Framework;
using UnityEngine;

public class TDMap : MonoBehaviour
{
    [Header("Map holders:")]
    [field: SerializeField] public SpriteRenderer BackgroundRenderer { get; private set; }
    [field: SerializeField] public Transform DecorationsHolder { get; private set; }
    [field: SerializeField] public Transform PathsHolder { get; private set; }
}
