using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class ReadyManager : MonoBehaviour
{
    static public ReadyManager Instance;

    [SerializeField]
    private GameObject readyPanel;
    [SerializeField]
    private GameObject deckPanel;

    [SerializeField]
    private List<ReadySort> _readySorts;
    private GameObject _characterPrefab;

    private DeckSettingManager _deckSettingManager;

    private Dictionary<int, GameObject> _selectedCharacters = new Dictionary<int, GameObject>();

    private void Awake()
    {
        Instance = this;

        _characterPrefab = Resources.Load<GameObject>("Prefabs/Character");
        _deckSettingManager = GetComponentInChildren<DeckSettingManager>();
    }

    private void Start()
    {
        LoadDeck();
    }

    public void LoadDeck()
    {
        List<int> deckLists = GameManager.Instance.CurPlayerData.DeckKeyLists;

        foreach (int character in deckLists)
        {
            _deckSettingManager.GetDeckBtn(character).AddCharacter();
        }
    }

    public void Exit()
    {
        SaveDeck();

    }

    public void SaveDeck()
    {
        GameManager.Instance.CurPlayerData.DeckKeyLists.Clear();

        foreach(KeyValuePair<int, GameObject> selectCharacter in _selectedCharacters)
        {
            GameManager.Instance.CurPlayerData.DeckKeyLists.Add(selectCharacter.Key);
        }


    }
    public void OnClickDeckSetting()
    {
        readyPanel.SetActive(false);
        deckPanel.SetActive(true);
    }    

    public bool Add(CharacterData characterData)
    {
        if(_selectedCharacters.Count == 5)
        {
            //����ó��
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
        if (_readySorts[index].GetSize() == 2)
        {
            //����ó��
            return false;
        }

        GameObject character = Instantiate(_characterPrefab, null);
        character.GetComponent<StandingCharacter>().SetData(characterData);

        _selectedCharacters.Add(characterData.Key, character);
        _readySorts[index].Add(character);

        return true;        
    }

    public void Remove(CharacterData characterData)
    {
        foreach (ReadySort sort in _readySorts)
            sort.Remove(_selectedCharacters[characterData.Key]);

        Destroy(_selectedCharacters[characterData.Key]);

        _selectedCharacters.Remove(characterData.Key);
    }

     public int GetPos(CharacterData characterData)
    {
        //�˸��� �� ��ȯ�ϵ��� �����ʿ�
        return _readySorts[characterData.Key].GetSize();
    }

}
