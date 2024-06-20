using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealEffect : MonoBehaviour
{
    [SerializeField] 
    private SpriteRenderer _healCircle;

    private Coroutine _healCoroutine = null;

    private void OnEnable()
    {
        _healCircle.transform.localScale = Vector3.zero;
        _healCircle.color = new Color(_healCircle.color.r, _healCircle.color.g, _healCircle.color.b, 1f);

        if (_healCoroutine != null)
        {
            StopCoroutine(_healCoroutine);
        }
        _healCoroutine = StartCoroutine(PlayHealEffect());
    }

    private void OnDisable()
    {
        if (_healCoroutine != null)
        {
            StopCoroutine(_healCoroutine);
        }
    }

    private IEnumerator PlayHealEffect()
    {
        float duration = 1.5f;
        float halfDuration = duration / 2f;
        float time = 0f;

        Vector3 initialScale = Vector3.zero;
        Vector3 targetScale = Vector3.one * 2;
        Color initialColor = _healCircle.color;
        Color targetColor = new Color(initialColor.r, initialColor.g, initialColor.b, 0f);

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / halfDuration);

            if (time <= halfDuration)
            {
                _healCircle.transform.localScale = Vector3.Lerp(initialScale, targetScale, t);
            }
            _healCircle.color = Color.Lerp(initialColor, targetColor, t);

            yield return null;
        }

        gameObject.SetActive(false);
    }
}
