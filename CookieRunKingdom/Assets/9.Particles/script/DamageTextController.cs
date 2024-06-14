/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageTextController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _damageTextPrefab;

    private Queue<TextMeshProUGUI> _textQueue;
    private Camera _camera;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        _camera = Camera.main;
        _textQueue = new Queue<TextMeshProUGUI>();

        for (int i = 0; i < 30; i++)
        {
            TextMeshProUGUI text = Instantiate(_damageTextPrefab, transform);
            _textQueue.Enqueue(text);
            ObjectPoolingObject objectPoolingObject = text.gameObject.AddComponent<ObjectPoolingObject>();
            objectPoolingObject.onDisable += (() => _textQueue.Enqueue(text));
            text.gameObject.SetActive(false);
        }
    }

    public TextMeshProUGUI GetDamageText(int value, bool isCritical = false, bool isHeal = false)
    {
        if(_textQueue.Count == 0)
        {
            Debug.Log("ÅÖÅÖ ºñ¾î¹ö¸°~");
            return null;
        }

        if (value == 0)
            return null;

        TextMeshProUGUI text = _textQueue.Dequeue();
        text.alpha = 1;
        string result = "";
        value = Mathf.Abs(value);
        string valueToString = value.ToString();

        for(int i = 0; i < valueToString.Length; i++)
        {
            if(isHeal)
                result += string.Format("<sprite={0}>", "1" + valueToString[i]);
            else
                result += string.Format("<sprite={0}>", valueToString[i]);
        }

        text.text = result;
        text.gameObject.SetActive(true);

        return text;
    }
}
*/