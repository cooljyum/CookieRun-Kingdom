using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SkillEffectUIController : MonoBehaviour
{
    private bool _isActivated = false;
    private float activationTime;
    private Image _childImg;
    private const float _fadeDuration = 0.3f; // ���̵� ��/�ƿ� ���� �ð�

    void OnEnable()
    {
        // ������Ʈ�� Ȱ��ȭ�Ǹ� Ÿ�̸� ����
        activationTime = Time.time;
        _isActivated = true;
        StartCoroutine(FadeIn());
    }

    void Update()
    {
        if (_isActivated)
        {
            if (Time.time - activationTime >= 2f|| !BattleManager.Instance.IsOnBattle )
            {
                // 2�ʰ� ������ ������ ��Ȱ��ȭ
                _isActivated = false; // ���� �ʱ�ȭ
                StartCoroutine(FadeOut());
            }
        }
    }

    public void ChangeSkillImgTex(Texture newTexture)
    {
        // �ڽ��� �ִ��� Ȯ��
        if (transform.childCount > 0)
        {
            // ù ��° �ڽ��� ������
            Transform firstChild = transform.GetChild(0);

            // �ڽ� ������Ʈ�� Image ������Ʈ�� �ִ��� Ȯ��
            _childImg = firstChild.GetComponent<Image>();

            // �̹��� �ؽ�ó�� ����

            _childImg.material.SetTexture("_mainTex", newTexture);
        }
    }

    private IEnumerator FadeIn()
    {
        if (_childImg != null)
        {
            float elapsedTime = 0f;
            Color color = _childImg.color;

            while (elapsedTime < _fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                color.a = Mathf.Clamp01(elapsedTime / _fadeDuration);
                _childImg.color = color;
                yield return null;
            }
        }
    }

    private IEnumerator FadeOut()
    {
        if (_childImg != null)
        {
            float elapsedTime = 0f;
            Color color = _childImg.color;

            while (elapsedTime < _fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                color.a = 1f - Mathf.Clamp01(elapsedTime / _fadeDuration);
                _childImg.color = color;
                yield return null;
            }

            // ������ ������� ������Ʈ ��Ȱ��ȭ
            gameObject.SetActive(false);
            BattleUIManager.Instance.IsSkillEffect = false;
        }
    }
}
