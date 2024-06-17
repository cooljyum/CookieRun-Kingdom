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
    public CharacterData CharacterData {  get { return _characterData; } }

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
        _profileImage.sprite = characterData.ProfileImage;
        _typeImage.sprite = characterData.TypeImage;
    }

    public void OnClick()
    {
        if (!_isSet)
        {
            AddCharacter();
        }
        else
        {
            RemoveCharacter();
        }
   
    }

    public void AddCharacter()
    {
        if (_isSet) return;

        if (ReadyManager.Instance.Add(_characterData))
        {
            ButtonOn();            
        }        
    }

    public void RemoveCharacter()
    {
        if(!_isSet) return;

        ReadyManager.Instance.Remove(_characterData);
        ButtonOff();

        
    }

    private void ButtonOn()
    {
        _isSet = true;
        _check.gameObject.SetActive(true);
        _profileImage.color = Color.gray;
        DeckSettingManager.Instance.SetTeamPower();
    }

    private void ButtonOff()
    {
        _isSet = false;        
        _check.gameObject.SetActive(false);
        _profileImage.color = Color.white;
        DeckSettingManager.Instance.SetTeamPower();
    }

    public bool IsSet() { return _isSet; }
}
