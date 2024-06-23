using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SkillEffectUIController : MonoBehaviour
{
    private bool _isActivated = false;
    private float activationTime;
    private Image _childImg;
    private const float _fadeDuration = 0.3f; // 페이드 인/아웃 지속 시간

    void OnEnable()
    {
        // 오브젝트가 활성화되면 타이머 시작
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
                // 2초가 지나면 서서히 비활성화
                _isActivated = false; // 상태 초기화
                StartCoroutine(FadeOut());
            }
        }
    }

    public void ChangeSkillImgTex(Texture newTexture)
    {
        // 자식이 있는지 확인
        if (transform.childCount > 0)
        {
            // 첫 번째 자식을 가져옴
            Transform firstChild = transform.GetChild(0);

            // 자식 오브젝트에 Image 컴포넌트가 있는지 확인
            _childImg = firstChild.GetComponent<Image>();

            // 이미지 텍스처를 변경

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

            // 완전히 사라지면 오브젝트 비활성화
            gameObject.SetActive(false);
            BattleUIManager.Instance.IsSkillEffect = false;
        }
    }
}
