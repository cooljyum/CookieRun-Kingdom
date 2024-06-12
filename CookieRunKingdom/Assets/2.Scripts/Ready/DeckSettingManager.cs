using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeckSettingManager : MonoBehaviour
{
    [SerializeField]
    private Transform _deckBtnContent;

    private CharacterData _characterData;
    private GameObject _teamPower;
    private TextMeshProUGUI _teamPowerText;

    private Dictionary<int, DeckSettingBtn> _deckBtns = new();

    private void OnEnable()
    {
        ClearCard();
        SetDeckBtn();
    }

    private void Start()
    {        
        _teamPower = GameObject.Find("TeamPower");
        _teamPowerText = _teamPower.GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        SetStandingCharacters();        
        
    }

    private void SetStandingCharacters()
    {

    }

    private void ClearCard()
    {
        foreach(KeyValuePair<int, DeckSettingBtn> deckBtn in _deckBtns)
        {
            Destroy(deckBtn.Value.gameObject);
        }

        _deckBtns.Clear();
    }

    private void SetDeckBtn()
    {
        GameObject btnPrefab = Resources.Load<GameObject>("Prefabs/BattleReady/DeckBtn");

        foreach(int myCard in GameManager.Instance.CurPlayerData.MyCardsLists )
        {
            CharacterData data = DataManager.Instance.GetCharacterData(myCard);

            GameObject btnObj = Instantiate(btnPrefab, _deckBtnContent);
            DeckSettingBtn btn = btnObj.GetComponent<DeckSettingBtn>();
            btn.SetData(data);

            _deckBtns[myCard] = btn;
        }
    }

    public DeckSettingBtn GetDeckBtn(int key) { return _deckBtns[key]; }
}
