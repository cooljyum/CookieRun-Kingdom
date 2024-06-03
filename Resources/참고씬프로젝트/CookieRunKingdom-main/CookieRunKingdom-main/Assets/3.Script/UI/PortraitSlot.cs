using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PortraitSlot : MonoBehaviour
{
    public System.Action onClickButton;

    [SerializeField] private Button _button;
    [SerializeField] private Image _portraitImage;
    [SerializeField] private GameObject _selectedImage;

    public void Init(Sprite sprite)
    {
        _button.onClick.AddListener(OnClickButton);

        _portraitImage.sprite = sprite;
        _selectedImage.SetActive(false);
    }

    public void SetActive(bool active)
    {
        _selectedImage.SetActive(active);
    }

    private void OnClickButton()
    {
        onClickButton?.Invoke();
    }
}
