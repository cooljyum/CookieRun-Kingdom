using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeckSettingManager : MonoBehaviour
{//팀 설정 패널 관리 스크립트
    [SerializeField]
    private Transform _deckBtnContent; //스크롤뷰 삽입 카드 목록
    private CharacterData _characterData;
    private GameObject _teamPower; //전투력 표시 오브젝트
    private TextMeshProUGUI _teamPowerText;//전투력 텍스트

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
    
    private void ClearCard()
    {//카드 초기화 함수: 팀 설정 패널 활성화 시 초기화 실행
        foreach(KeyValuePair<int, DeckSettingBtn> deckBtn in _deckBtns)
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
            CharacterData data = DataManager.Instance.GetCharacterData(myCard);

            GameObject btnObj = Instantiate(btnPrefab, _deckBtnContent);
            DeckSettingBtn btn = btnObj.GetComponent<DeckSettingBtn>();
            btn.SetData(data);

            _deckBtns[myCard] = btn;
        }
    }

    public DeckSettingBtn GetDeckBtn(int key) { return _deckBtns[key]; }
    //카드 반환 함수
}
