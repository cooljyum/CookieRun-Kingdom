using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TyphoonEffect : MonoBehaviour
{
    [SerializeField] 
    private SpriteRenderer _typhoonSprite;

    private Coroutine _typhoonCoroutine = null;

    private void OnEnable()
    {
        _typhoonSprite.transform.localScale = Vector3.zero;
        _typhoonSprite.color = new Color(_typhoonSprite.color.r, _typhoonSprite.color.g, _typhoonSprite.color.b, 1f);

        if (_typhoonCoroutine != null)
        {
            StopCoroutine(_typhoonCoroutine);
        }
        _typhoonCoroutine = StartCoroutine(PlayTyphoonEffect());
    }

    private void OnDisable()
    {
        if (_typhoonCoroutine != null)
        {
            StopCoroutine(_typhoonCoroutine);
        }
    }

    private IEnumerator PlayTyphoonEffect()
    {
        float duration = 1f; // 효과의 전체 지속 시간
        float growDuration = 1f; // 스케일이 증가하는 시간
        float rotateSpeed = 360f; // 회전 속도 (degrees per second)
        float fadeDuration = 2f; // 페이드 아웃 시간
        float time = 0f;

        Vector3 initialScale = Vector3.zero;
        Vector3 targetScale = Vector3.one * 3; // 태풍의 목표 스케일
        Color initialColor = _typhoonSprite.color;
        Color targetColor = new Color(initialColor.r, initialColor.g, initialColor.b, 0f);

        while (time < duration)
        {
            time += Time.deltaTime;

            // 스케일 증가
            if (time <= growDuration)
            {
                float t = Mathf.Clamp01(time / growDuration);
                _typhoonSprite.transform.localScale = Vector3.Lerp(initialScale, targetScale, t);
            }

            // 회전
            _typhoonSprite.transform.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime);

            // 페이드 아웃
            if (time > growDuration)
            {
                float fadeTime = time - growDuration;
                float t = Mathf.Clamp01(fadeTime / fadeDuration);
                _typhoonSprite.color = Color.Lerp(initialColor, targetColor, t);
            }

            yield return null;
        }

        gameObject.SetActive(false);
    }
}
