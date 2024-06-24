using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeckSettingManager : MonoBehaviour
{//�� ���� �г� ���� ��ũ��Ʈ
    static public DeckSettingManager Instance;

    [SerializeField]
    private Transform _deckBtnContent; //��ũ�Ѻ� ���� ī�� ���
    private CharacterData _characterData;
    private GameObject _teamPower; //������ ǥ�� ������Ʈ
    private TextMeshProUGUI _teamPowerText;//������ �ؽ�Ʈ
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
    {//ī�� �ʱ�ȭ �Լ�: �� ���� �г� Ȱ��ȭ �� �ʱ�ȭ ����
        foreach(KeyValuePair<int, GameObject> deckBtn in _deckBtns)
        {
            Destroy(deckBtn.Value.gameObject);
        }

        _deckBtns.Clear();
    }

    private void SetDeckBtn()
    {//�� ���� �г� ���� �� ��ũ�Ѻ信 ���� ���� ī�� Ȱ��ȭ
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
    //ī�� ��ȯ �Լ�
      
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
