using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    public float explosionSize = 1.0f;
    [SerializeField] float _explosionSpeed = 1.0f;

    SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(ExplosionAnimation());
    }

    IEnumerator ExplosionAnimation()
    {
        float scale = explosionSize / 2;
        Color color = _spriteRenderer.color;
        color.a = 0.1f;

        _spriteRenderer.color = color;
        transform.localScale = new Vector3(scale, scale, 1);

        while (_spriteRenderer.color.a < 1)
        {
            yield return new WaitForFixedUpdate();

            color.a += 0.1f * _explosionSpeed;
            scale = (explosionSize / 2) + color.a * (explosionSize / 2);

            _spriteRenderer.color = color;
            transform.localScale = new Vector3(scale, scale, 1);
        }

        yield return new WaitForSeconds(0.2f/_explosionSpeed);

        while (_spriteRenderer.color.a > 0)
        {
            yield return new WaitForFixedUpdate();

            color.a -= 0.05f * _explosionSpeed;
            scale = (explosionSize / 2) + color.a * (explosionSize / 2);

            _spriteRenderer.color = color;
            transform.localScale = new Vector3(scale, scale, 1);
        }

        Destroy(gameObject);
    }
}
