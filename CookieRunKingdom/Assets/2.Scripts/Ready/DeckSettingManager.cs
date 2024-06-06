using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckSettingManager : MonoBehaviour
{
    [SerializeField]
    private Transform _deckBtnContent;

    private CharacterData _characterData;


   

    private void Start()
    {
        SetDeckBtn();
    }

    private void Update()
    {
        SetStandingCharacters();        
    }

    private void SetStandingCharacters()
    {

    }
    private void SetDeckBtn()
    {
        GameObject btnPrefab = Resources.Load<GameObject>("Prefabs/BattleReady/DeckBtn");

        foreach(int myCard in GameManager.Instance.myCards)
        {
            CharacterData data = DataManager.Instance.GetCharacterData(myCard);

            GameObject btnObj = Instantiate(btnPrefab, _deckBtnContent);
            btnObj.GetComponent<DeckSettingBtn>().SetData(data);
        }
    }
}
