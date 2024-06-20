using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCircleEffect : MonoBehaviour
{
    [SerializeField] 
    private SpriteRenderer _magicCircle;

    private Coroutine _magicCoroutine = null;

    private void OnEnable()
    {
        _magicCircle.transform.localScale = Vector3.zero;
        _magicCircle.color = new Color(_magicCircle.color.r, _magicCircle.color.g, _magicCircle.color.b, 1f);

        if (_magicCoroutine != null)
        {
            StopCoroutine(_magicCoroutine);
        }
        _magicCoroutine = StartCoroutine(PlayMagicCircleEffect());
    }

    private void OnDisable()
    {
        if (_magicCoroutine != null)
        {
            StopCoroutine(_magicCoroutine);
        }
    }

    private IEnumerator PlayMagicCircleEffect()
    {
        float duration = 2f; // 효과의 전체 지속 시간
        float growDuration = 0.5f; // 스케일이 증가하는 시간
        float fadeDuration = 1.5f; // 페이드 아웃 시간
        float time = 0f;

        Vector3 initialScale = Vector3.zero;
        Vector3 targetScale = Vector3.one * 3; // 마법진의 목표 스케일
        Color initialColor = _magicCircle.color;
        Color targetColor = new Color(initialColor.r, initialColor.g, initialColor.b, 0f);

        while (time < duration)
        {
            time += Time.deltaTime;

            if (time <= growDuration)
            {
                float t = Mathf.Clamp01(time / growDuration);
                _magicCircle.transform.localScale = Vector3.Lerp(initialScale, targetScale, t);
            }

            if (time > growDuration)
            {
                float fadeTime = time - growDuration;
                float t = Mathf.Clamp01(fadeTime / fadeDuration);
                _magicCircle.color = Color.Lerp(initialColor, targetColor, t);
            }

            yield return null;
        }

        gameObject.SetActive(false);
    }
}