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
        float duration = 1f; // ȿ���� ��ü ���� �ð�
        float growDuration = 1f; // �������� �����ϴ� �ð�
        float rotateSpeed = 360f; // ȸ�� �ӵ� (degrees per second)
        float fadeDuration = 2f; // ���̵� �ƿ� �ð�
        float time = 0f;

        Vector3 initialScale = Vector3.zero;
        Vector3 targetScale = Vector3.one * 3; // ��ǳ�� ��ǥ ������
        Color initialColor = _typhoonSprite.color;
        Color targetColor = new Color(initialColor.r, initialColor.g, initialColor.b, 0f);

        while (time < duration)
        {
            time += Time.deltaTime;

            // ������ ����
            if (time <= growDuration)
            {
                float t = Mathf.Clamp01(time / growDuration);
                _typhoonSprite.transform.localScale = Vector3.Lerp(initialScale, targetScale, t);
            }

            // ȸ��
            _typhoonSprite.transform.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime);

            // ���̵� �ƿ�
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
