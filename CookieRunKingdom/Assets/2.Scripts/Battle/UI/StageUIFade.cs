using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageUIFade : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup _stagePanel;

    private float _fadeDuration = 1f;
    private float _displayDuration = 2f;

    void Start()
    {
        if (_stagePanel == null)
        {
            Debug.LogError("Stage Panel is not assigned.");
            return;
        }

        StartCoroutine(FadeOutPanel());
    }

    IEnumerator FadeOutPanel()
    {
        yield return new WaitForSeconds(_displayDuration);

        float timer = 0f;
        while (timer < _fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timer / _fadeDuration);
            _stagePanel.alpha = alpha;
            yield return null;
        }

        _stagePanel.gameObject.SetActive(false);
    }
}
