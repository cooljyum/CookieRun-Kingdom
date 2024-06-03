using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class JellyInfoUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _jellyText;
    [SerializeField] private TextMeshProUGUI _jellTimeText;

    private Coroutine _coUpdate;

    private void OnDisable()
    {
        if(GameManager.Instance != null)
            GameManager.Game.prevJellyTime = System.DateTime.Now;

        if (_coUpdate != null)
            StopCoroutine(_coUpdate);
    }

    private void OnEnable()
    {
        GameManager.Game.UpdateJellyTime();
        OnChangeJelly();
    }


    public void OnChangeJelly()
    {
        if (!transform.CheckParentActive(true))
            return;

        _jellTimeText.gameObject.SetActive(false);

        if (GameManager.Game.Jelly < GameManager.Game.MaxJelly)
        {
            if (_coUpdate != null)
                StopCoroutine(_coUpdate);
            _coUpdate = StartCoroutine(CoUpdate());
        }
    }


    private IEnumerator CoUpdate()
    {
        _jellTimeText.gameObject.SetActive(true);
        WaitForSeconds wait = new WaitForSeconds(1f);

        while(true)
        {
            _jellTimeText.text = Utils.GetTimeText(GameManager.Game.jellyTime, false);
            yield return wait;
            GameManager.Game.jellyTime--;

            if (GameManager.Game.jellyTime <= 0)
            {
                GameManager.Game.Jelly++;
                GameManager.Game.jellyTime = Utils.JellyTime;
                if (GameManager.Game.Jelly >= GameManager.Game.MaxJelly)
                {
                    _jellTimeText.gameObject.SetActive(false);
                    break;
                }
            }

            GameManager.Game.prevJellyTime = System.DateTime.Now;
        }
    }
}
