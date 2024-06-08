using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DeckSettingBtn : MonoBehaviour
{
    private CharacterData _characterData;

    private Image _profileImage;
    private Image _typeImage;
    private Image _check;
    private TextMeshProUGUI _levelData;

    private bool _isSet = false;

    private void Awake()
    {

        _levelData = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        _typeImage = transform.GetChild(2).GetComponent<Image>();
        _check = transform.GetChild(3).GetComponent<Image>();
        _profileImage = GetComponent<Image>();
        
    }

    public void SetData(CharacterData characterData)
    {
        _profileImage.sprite = characterData.profileImage;
      //  _levelData.text = characterData.Level.ToString();
        _typeImage.sprite = characterData.typeImage;
    }

    public void OnClick()
    {
        _isSet = !_isSet;

        if(_isSet )
        {
            _check.gameObject.SetActive(true);
            _profileImage.sprite.GetComponent<Image>().color = Color.gray;
        }
    }

}
