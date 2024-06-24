using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeckSettingManager : MonoBehaviour
{//팀 설정 패널 관리 스크립트
    static public DeckSettingManager Instance;

    [SerializeField]
    private Transform _deckBtnContent; //스크롤뷰 삽입 카드 목록
    private CharacterData _characterData;
    private GameObject _teamPower; //전투력 표시 오브젝트
    private TextMeshProUGUI _teamPowerText;//전투력 텍스트
    private float _power;

    private Dictionary<int, GameObject> _deckBtns = new();

    private void OnEnable()
    {
        ClearCard();
        SetDeckBtn();
        SetTeamPower();
    }

    private void Awake()
    {
        Instance = this;        
        _teamPower = GameObject.Find("TeamPower");
        _teamPowerText = _teamPower.GetComponent<TextMeshProUGUI>();
    }
        
    private void ClearCard()
    {//카드 초기화 함수: 팀 설정 패널 활성화 시 초기화 실행
        foreach(KeyValuePair<int, GameObject> deckBtn in _deckBtns)
        {
            Destroy(deckBtn.Value.gameObject);
        }

        _deckBtns.Clear();
    }

    private void SetDeckBtn()
    {//팀 설정 패널 진입 시 스크롤뷰에 소유 중인 카드 활성화
        GameObject btnPrefab = Resources.Load<GameObject>("Prefabs/BattleReady/DeckBtn");

        foreach(int myCard in GameManager.Instance.CurPlayerData.MyCardsLists )
        {
            _characterData = DataManager.Instance.GetCharacterData(myCard);

            GameObject btnObj = Instantiate(btnPrefab, _deckBtnContent);
            
            btnObj.GetComponent<DeckSettingBtn>().SetData(_characterData);

            _deckBtns[myCard] = btnObj;
        }
    }

    public GameObject GetDeckBtn(int key) { return _deckBtns[key]; }
    //카드 반환 함수
      
    public void RemoveCharacter(int key)
    {
        _deckBtns[key].gameObject.GetComponent<DeckSettingBtn>().RemoveCharacter();

    }

    public void ClearCharacters()
    {
        SoundManager.Instance.PlayFX("BtnClick");
        foreach (KeyValuePair<int, GameObject> deckBtn in _deckBtns)
        {            
            if(deckBtn.Value.GetComponent<DeckSettingBtn>().IsSet() == true)
            {
                deckBtn.Value.GetComponent<DeckSettingBtn>().RemoveCharacter();
            }
        }

    }

    public void SetTeamPower()
    {
        _power = 0.0f;

        foreach (KeyValuePair<int, GameObject> deckBtn in _deckBtns)
        {
            if (deckBtn.Value.GetComponent<DeckSettingBtn>().IsSet() == true)
            {
                _power += deckBtn.Value.GetComponent<DeckSettingBtn>().CharacterData.Attack * 2;
                _power += deckBtn.Value.GetComponent<DeckSettingBtn>().CharacterData.Hp * 0.5f;
                _power += deckBtn.Value.GetComponent<DeckSettingBtn>().CharacterData.Critical * 3.0f;
                _power += deckBtn.Value.GetComponent<DeckSettingBtn>().CharacterData.Defence;
            }
        }

        _teamPowerText.text = _power.ToString();
    }

}
