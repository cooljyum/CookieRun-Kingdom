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
        EnterReadyPanel();
    }
    public void EnterDeck()
    {
        //SaveDeck();
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
        {
            //예외처리
            return false;
        }

        switch (characterData.Type)
        {
            case 0:
            case 1:
                return PushSort(0, characterData);
            case 2:
            case 3:
            case 7:
                return PushSort(1, characterData);
            case 4:
            case 5:
            case 6:
                return PushSort(2, characterData);
        }

        return false;
    }

    private bool PushSort(int index, CharacterData characterData)
    {
        if (_selectedCharacters.ContainsKey(characterData.Key)) return false;

        for (int i = index; i < _readySorts.Count; i++)
        {
            if (_readySorts[i].GetSize() < 2)
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
