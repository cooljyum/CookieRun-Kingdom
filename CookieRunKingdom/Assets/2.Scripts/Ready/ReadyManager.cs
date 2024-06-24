using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class ReadyManager : MonoBehaviour
{//전투 준비 창 관리 스크립트
    static public ReadyManager Instance;

    [SerializeField]
    private GameObject readyPanel;
    [SerializeField]
    private GameObject deckPanel;

    [SerializeField]
    private List<ReadySort> _readySorts;
    private GameObject _characterPrefab;
    [SerializeField]
    private DeckSettingManager _deckSettingManager;

    private Dictionary<int, GameObject> _selectedCharacters = new Dictionary<int, GameObject>();
    
    public enum Type
    {
        Assault,
        Defence,
        Penetrate,
        Magic,
        Heal,
        Shoot,
        Buff,
        Bomb
    }
    private void Awake()
    {
        Instance = this;

        _characterPrefab = Resources.Load<GameObject>("Prefabs/Character");
    }

    private void Start()
    {
        LoadReady();
    }

    public void LoadDeck()
    {     
        List<int> deckLists = GameManager.Instance.CurPlayerData.DeckKeyLists;
        
        foreach (int character in deckLists)
        {
            _deckSettingManager.GetDeckBtn(character).GetComponent<DeckSettingBtn>().AddCharacter();
        }
    }

    public void LoadReady()
    {
        List<int> deckLists = GameManager.Instance.CurPlayerData.DeckKeyLists;

        foreach (int character in deckLists)
        {
            Add(DataManager.Instance.GetCharacterData(character));
        }
    }

    public void Exit()
    {
        SaveDeck();
        SoundManager.Instance.PlayFX("BtnClick2");
        EnterReadyPanel();
    }
    public void EnterDeck()
    { 
        SoundManager.Instance.PlayFX("BtnClick2");
        EnterDeckSettingPanel();
    }

    public void SaveDeck()
    {
        GameManager.Instance.CurPlayerData.DeckKeyLists.Clear();
        List<int> deckCntList = new List<int>();
        for (int i = 0; i < _readySorts.Count; i++)
        {
            deckCntList.Add(_readySorts[i].GetSize());

            for (int j = 0; j < _readySorts[i].GetSize(); j++)
            {
                GameManager.Instance.CurPlayerData.DeckKeyLists.Add(_readySorts[i].Characters[j].GetComponent<StandingCharacter>().CharacterData.Key);
            }
        }
        GameManager.Instance.CurPlayerData.BattlePosCntLists = deckCntList;

        foreach (KeyValuePair<int, GameObject> selectCharacter in _selectedCharacters)
        {
            Destroy(selectCharacter.Value);
        }

        _selectedCharacters.Clear();
        foreach (ReadySort readySort in _readySorts)
        {
            readySort.Clear();
        }
        GameManager.Instance.SavePlayerData();
    }

    public void EnterReadyPanel()
    {
        readyPanel.SetActive(true);
        deckPanel.SetActive(false);
        LoadReady();

    }
    public void EnterDeckSettingPanel()
    {  
        readyPanel.SetActive(false);
        deckPanel.SetActive(true);
        LoadDeck();        

    }    
    
    public bool Add(CharacterData characterData)
    {
        if(_selectedCharacters.Count == 5)
        {//기존 선택된 캐릭터가 5에 도달할 시 Add호출 예외 처리
            return false;
        }

        switch ((Type)characterData.Type)
        {//CharacterData의 Type 정보에 따라 
            case Type.Assault:
            case Type.Defence:
                return PushSort(0, characterData);
            case Type.Penetrate:
            case Type.Magic:
            case Type.Bomb:
                return PushSort(1, characterData);
            case Type.Shoot:
            case Type.Heal:
            case Type.Buff:
                return PushSort(2, characterData);
        }

        return false;
    }

    private bool PushSort(int index, CharacterData characterData)
    {
        if (_selectedCharacters.ContainsKey(characterData.Key)) return false;

        for (int i = index; i < _readySorts.Count; i++)
        {
            if (_readySorts[i].GetSize() < 2)//해당 위치에 배치된 캐릭터가 2개를 초과할 시
            {
                // 비어있는 ReadySort에 캐릭터를 추가
                GameObject character = Instantiate(_characterPrefab, null);
                character.GetComponent<StandingCharacter>().SetData(characterData);

                _selectedCharacters.Add(characterData.Key, character);
                _readySorts[i].Add(character);

                return true;
            }
        }

        return false;
    }
    public void Remove(CharacterData characterData)
    {
        foreach (ReadySort sort in _readySorts)
        {
            if (sort.Contains(_selectedCharacters[characterData.Key]))
            { 
                sort.Remove(_selectedCharacters[characterData.Key]);
                break;
            }
        }

        Destroy(_selectedCharacters[characterData.Key]);

        _selectedCharacters.Remove(characterData.Key);
    }

}
