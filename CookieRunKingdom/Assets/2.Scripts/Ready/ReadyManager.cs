using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class ReadyManager : MonoBehaviour
{//���� �غ� â ���� ��ũ��Ʈ
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
        {//���� ���õ� ĳ���Ͱ� 5�� ������ �� Addȣ�� ���� ó��
            return false;
        }

        switch ((Type)characterData.Type)
        {//CharacterData�� Type ������ ���� 
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
            if (_readySorts[i].GetSize() < 2)//�ش� ��ġ�� ��ġ�� ĳ���Ͱ� 2���� �ʰ��� ��
            {
                // ����ִ� ReadySort�� ĳ���͸� �߰�
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
