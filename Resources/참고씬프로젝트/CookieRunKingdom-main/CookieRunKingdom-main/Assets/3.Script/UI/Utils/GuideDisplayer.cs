using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class GuideDisplayer : MonoBehaviour
{
    private static GuideDisplayer instance = null;
    public static GuideDisplayer Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GuideDisplayer>();
                if (instance == null)
                {
                    GameObject go = new GameObject();
                    go.name = "SingletonController";
                    instance = go.AddComponent<GuideDisplayer>();

                    instance.Init();
                }
            }
            return instance;
        }
    }

    [SerializeField] private TextMeshProUGUI _guideTextPrefab;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Init();
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private Queue<TextMeshProUGUI> _textPool;

    private void Init()
    {
        _textPool = new Queue<TextMeshProUGUI>();

        for (int i = 0; i < 15; i++)
        {
            TextMeshProUGUI text = Instantiate(_guideTextPrefab, transform);
            text.transform.localPosition = Vector3.zero;
            text.gameObject.AddComponent<ObjectPoolingObject>().onDisable = (() =>
            {
                _textPool.Enqueue(text);
            });
            _textPool.Enqueue(text);
            text.gameObject.SetActive(false);
        }
    }


    public void ShowGuide(string content)
    {
        if(_textPool.Count == 0)
        {
            Debug.Log("다 소모했습니다");
            return;
        }

        TextMeshProUGUI text = _textPool.Dequeue();
        text.text = content;

        Sequence seq = DOTween.Sequence();

        float yPos = text.transform.position.y;

        text.transform.localPosition = Vector3.up * -(yPos / 5f);
        text.transform.localScale = Vector3.one * 0.75f;
        text.color = Color.white;


        text.gameObject.SetActive(true);
        seq.Append(text.transform.DOLocalMoveY(0, 0.4f))
            .Join(text.transform.DOScale(Vector3.one, 0.4f))
            .AppendInterval(1f)
            .Append(text.transform.DOLocalMoveY((yPos / 5f), 0.5f))
            .Join(text.DOColor(Color.black, 0.5f))
            .Join(text.DOFade(0, 0.5f))
            .OnComplete(() => text.gameObject.SetActive(false));
    }
}
