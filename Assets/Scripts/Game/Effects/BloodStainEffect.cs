using System.Collections.Generic;
using UnityEngine;

public class BloodStainEffect : MonoBehaviour
{
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] List<Sprite> _bloodSprites = new List<Sprite>();

    private void Awake()
    {
        _spriteRenderer.sprite = _bloodSprites[Random.Range(0, _bloodSprites.Count)];
        transform.localScale = transform.localScale * Random.Range(0.9f, 1.1f);
        transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
    }

    private void FixedUpdate()
    {
        Color newAlpha = _spriteRenderer.color;
        newAlpha.a -= 2f * Time.deltaTime;
        _spriteRenderer.color = newAlpha;

        if(_spriteRenderer.color.a <= 0)
        {
            Destroy(gameObject);
        }
    }
}
