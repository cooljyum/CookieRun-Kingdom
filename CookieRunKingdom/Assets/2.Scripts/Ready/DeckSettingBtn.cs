using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DeckSettingBtn : MonoBehaviour
{
    static public DeckSettingBtn Instance;

    private CharacterData _characterData;

    private Image _profileImage;
    private Image _typeImage;
    private Image _check;
    private TextMeshProUGUI _levelData;

    public float BattlePower;

    private bool _isSet = false;

    private void Awake()
    {    
        _levelData = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        _typeImage = transform.GetChild(2).GetComponent<Image>();
        _check = transform.GetChild(3).GetComponent<Image>();
        _check.gameObject.SetActive(false);
        _profileImage = GetComponent<Image>(); 
        
    }
    private void Start()
    {
        foreach (int key in GameManager.Instance.CurPlayerData.DeckKeyLists)
        {
            if (key == _characterData.Key)
                ButtonOn();
        }
    }

    public void SetData(CharacterData characterData)
    {
        _characterData = characterData;
        _profileImage.sprite = characterData.profileImage;
        _typeImage.sprite = characterData.typeImage;
    }

    public void OnClick()
    {
        if (!_isSet)
        {
            if(ReadyManager.Instance.Add(_characterData))
            {
                ButtonOn();
            }

            GameManager.Instance.CurPlayerData.DeckKeyLists.Add(_characterData.Key); //Ű�� ����

            BattlePower += _characterData.Attack;
        }
        else
        {
            ReadyManager.Instance.Remove(_characterData);
            ButtonOff();

            GameManager.Instance.CurPlayerData.DeckKeyLists.Remove(_characterData.Key); //Ű�� ����

            BattlePower -= _characterData.Attack;
        }
    }

    private void ButtonOn()
    {
        _isSet = true;
        _check.gameObject.SetActive(true);
        _profileImage.color = Color.gray;
    }

    private void ButtonOff()
    {
        _isSet = false;        
        _check.gameObject.SetActive(false);
        _profileImage.color = Color.white;
    }
}
