using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadbuttEffect : MonoBehaviour
{
    [SerializeField] 
    private Transform _attacker;
    [SerializeField] 
    private Transform _target;
    [SerializeField] 
    private SpriteRenderer _impactEffect;

    private Coroutine _headbuttCoroutine = null;

    private void OnEnable()
    {
        _impactEffect.gameObject.SetActive(false);

        if (_headbuttCoroutine != null)
        {
            StopCoroutine(_headbuttCoroutine);
        }
        _headbuttCoroutine = StartCoroutine(PlayHeadbuttEffect());
    }

    private void OnDisable()
    {
        if (_headbuttCoroutine != null)
        {
            StopCoroutine(_headbuttCoroutine);
        }
    }

    private IEnumerator PlayHeadbuttEffect()
    {
        Vector3 originalPosition = _attacker.position;
        Vector3 targetPosition = _target.position;
        float attackDuration = 0.2f; // ���� �ִϸ��̼��� ���� �ð�
        float impactDuration = 0.1f; // �浹 ���� �ִϸ��̼��� ���� �ð�
        float retreatDuration = 0.2f; // ���� �ִϸ��̼��� ���� �ð�
        float time = 0f;

        // ���� �ִϸ��̼� (�̵�)
        while (time < attackDuration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / attackDuration);
            _attacker.position = Vector3.Lerp(originalPosition, targetPosition, t);
            yield return null;
        }

        // �浹 �ִϸ��̼� (����Ʈ ȿ��)
        _impactEffect.transform.position = _target.position;
        _impactEffect.gameObject.SetActive(true);
        _impactEffect.transform.localScale = Vector3.zero;
        _impactEffect.color = new Color(_impactEffect.color.r, _impactEffect.color.g, _impactEffect.color.b, 1f);

        time = 0f;
        while (time < impactDuration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / impactDuration);
            _impactEffect.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one * 2, t);
            yield return null;
        }

        _impactEffect.gameObject.SetActive(false);

        // ���� �ִϸ��̼� (�̵�)
        time = 0f;
        while (time < retreatDuration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / retreatDuration);
            _attacker.position = Vector3.Lerp(targetPosition, originalPosition, t);
            yield return null;
        }

        gameObject.SetActive(false);
    }
}
