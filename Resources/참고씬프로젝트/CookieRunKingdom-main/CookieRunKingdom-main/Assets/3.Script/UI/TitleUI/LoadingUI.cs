using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadingUI : MonoBehaviour
{
    [Header("Á¤º¸")]
    [SerializeField] private Sprite[] _loadingSprites;
    [SerializeField] private string[] _tipContents;

    [Header("UI")]
    [SerializeField] private Image _loadingImage;
    [SerializeField] private TextMeshProUGUI _loadingText;

    public void StartLoading()
    {
        _loadingImage.gameObject.SetActive(true);
        _loadingImage.sprite = _loadingSprites[Random.Range(0, _loadingSprites.Length)];
        _loadingText.text = _tipContents[Random.Range(0, _tipContents.Length)];
    }

    public void EndLoading()
    {
        _loadingImage.gameObject.SetActive(false);
    }
}
