using Spine.Unity;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class ResultUIController : MonoBehaviour
{
    [Header("Victory")]
    [SerializeField]
    private GameObject _battleVictoryUI;

    [Header("Star")]
    private int _star;
    [SerializeField]
    private GameObject _resultStars;

    [Header("ItemSlot")]
    private GameObject _itemSlotPrefab;
    [SerializeField]
    private Transform _itemSlotParentTransform;
    private List<StageItem> _stageItemList;

    [Header("----------------------------")]
    [Header("Defeat")]
    [SerializeField]
    private GameObject _battleDefeatUI;

    [Header("----------------------------")]
    [Header("Cookie")]
    [SerializeField]
    private GameObject _awardCookiesParent;

    [Header("Btn")]
    [SerializeField]
    private GameObject _resultBtnParent;
    [SerializeField]
    private Button _exitBtn;
    [SerializeField]
    private Button _goKingdomBtn;

    private void Awake()
    {
        _itemSlotPrefab = Resources.Load<GameObject>("Prefabs/Battle/ItemSlot");
        _exitBtn = _resultBtnParent.transform.GetChild(0).GetComponent<Button>();
        _goKingdomBtn = _resultBtnParent.transform.GetChild(1).GetComponent<Button>();
    }
    private void Start()
    {
        _exitBtn.onClick.AddListener(() => LoadingManager.Instance.LoadScene("KingdomScene"));
        _exitBtn.onClick.AddListener(() => SoundManager.Instance.PlayFX("BtnClick"));
        _goKingdomBtn.onClick.AddListener(() => LoadingManager.Instance.LoadScene("KingdomScene"));
        _goKingdomBtn.onClick.AddListener(() => SoundManager.Instance.PlayFX("BtnClick"));
    }

    public void Init()
    {
        _battleVictoryUI.SetActive(false);
        _battleDefeatUI.SetActive(false);
        _resultBtnParent.SetActive(false);
        _awardCookiesParent.SetActive(false);

        ClearChildObjects(_itemSlotParentTransform);
        SetItem();
        SetAwardCookies();
    }

    public void SetResultUI(bool isWin)
    {
        if (isWin)
        {
            SoundManager.Instance.PlayBG("VictoryBgm");
            SoundManager.Instance.PlayFX("Battle_VictoryEffect");
            SoundManager.Instance.PlayFX("Battle_VictoryVoiceEffect");

            SetResult();

            SaveItems();
            SaveCurStage();

            GameManager.Instance.SavePlayerData();

            _battleVictoryUI.SetActive(true);
        }
        else 
        {
            SoundManager.Instance.PlayBG("DefeatBgm");
            SoundManager.Instance.PlayFX("Battle_DefeatEffect");
            SoundManager.Instance.PlayFX("Battle_DefeatVoiceEffect");

            _battleDefeatUI.SetActive(true);
        }

        _resultBtnParent.SetActive(true);
        _awardCookiesParent.SetActive(true);
      
        SetAwardCookiesAni(isWin);
    }

    private void SetResult() 
    {
        SetStar();
    }

    private void SetStar() 
    {
        _star = 1; //기본 1개

        // 죽은 쿠키가 1이하 -> 스타 추가
        if (BattleManager.Instance.KilledCookies <= 1) 
        {
            _star++;
        }

        // 남은시간이 40초 이상 -> 스타 추가
        if (BattleUIManager.Instance.BattleTime > 40f)
        {
            _star++;
        }

        SetStarUI();
    }

    private void SetStarUI() 
    {
        for (int i = 0; i < _resultStars.transform.childCount; i++)
        {
            if(i<_star)
                _resultStars.transform.GetChild(i).gameObject.SetActive(true);
            else
                _resultStars.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void SetItem() 
    {
        _stageItemList = new List<StageItem>();

        List<StageItem> itemDatas = BattleManager.Instance.StageData.StageItemList;
        for (int i = 0;i < itemDatas.Count;i++) 
        {
            StageItem item = itemDatas[i];
            if (item != null)
            {
                GameObject itemSlot = Instantiate(_itemSlotPrefab, _itemSlotParentTransform);
                itemSlot.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite = DataManager.Instance.GetItemData(item.Key).Sprite;
                itemSlot.transform.GetChild(0).GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = item.Value.ToString();

                _stageItemList.Add(item);
            }
        }
    }

    void ClearChildObjects(Transform objTransform)
    {
        foreach (Transform child in objTransform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    void SetAwardCookies() 
    {
        List<List<int>> battleCookies =  BattleObjectSpawnManager.Instance.BattleCookieKeys;

        foreach (Transform child in _awardCookiesParent.transform)
        {
            child.gameObject.SetActive(false);
        }

        int cnt = 0;
        foreach (List<int> cookieList in battleCookies)
        {
            foreach (int cookieKey in cookieList)
            {
                GameObject awardCookie = _awardCookiesParent.transform.GetChild(cnt).gameObject;
                if (awardCookie != null)
                {
                    awardCookie.SetActive(true);
                    awardCookie.transform.GetChild(0).GetComponent<SkeletonGraphic>().skeletonDataAsset = DataManager.Instance.GetCharacterData(cookieKey).SkeletonDataAsset;
                    awardCookie.transform.GetChild(0).GetComponent<SkeletonGraphic>().Initialize(true);
                    cnt++;
                }
            }
        }
    }

    void SetAwardCookiesAni(bool isWin) 
    {
        for (int i = 0; i < BattleManager.Instance.CntCurCookies; i++)
        {
            string aniName;
            if (isWin)
                aniName = "joy";
            else 
                aniName = "lose";

            _awardCookiesParent.transform.GetChild(i).GetChild(0).GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(0, aniName, true);
        }
    }

    void SaveItems() 
    {
        foreach (var itemData in _stageItemList)
        {
            int key = itemData.Key;
            int value = (int)itemData.Value;

            switch (key)
            {
                case 2: //Exp
                    GameManager.Instance.CurPlayerData.Exp += value;
                    break;
                case 3: //Gold
                    GameManager.Instance.CurPlayerData.Coin += value;
                    break;
            }
        }
    }

    void SaveCurStage() 
    {
        GameManager.Instance.CurPlayerData.CurStage++;
    }
}
