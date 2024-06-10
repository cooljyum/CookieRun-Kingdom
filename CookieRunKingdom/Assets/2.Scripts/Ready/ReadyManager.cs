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

    private Dictionary<int, GameObject> _selectedCharacters = new Dictionary<int, GameObject>();

    private void Awake()
    {
        Instance = this;

        _characterPrefab = Resources.Load<GameObject>("Prefabs/Character");

        //Transform[] children = GetComponentsInChildren<Transform>();
        //
        //foreach (Transform child in children)
        //{
        //    if(child.name == "BattleReadyPanel")
        //    {
        //        readyPanel = child.gameObject;
        //    }
        //    else if(child.name == "DeckSettingPanel")
        //    {
        //        deckPanel = child.gameObject;
        //    }
        //}

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
            //抗寇贸府
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
            //抗寇贸府
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

     public void Clear()
    {
        
            
    }
}
