using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("----------------------------")]
    [Header("Defeat")]
    [SerializeField]
    private GameObject _battleDefeatUI;

    [Header("----------------------------")]
    [Header("Cookie")]
    [SerializeField]
    private GameObject _awardCookiesTransform;



    private void Awake()
    {
        _itemSlotPrefab = Resources.Load<GameObject>("Prefabs/Battle/ItemSlot");
    }

    public void Init()
    {
        _battleVictoryUI.SetActive(false);
        _battleDefeatUI.SetActive(false);
        _awardCookiesTransform.SetActive(false);

        ClearChildObjects(_itemSlotParentTransform);
        SetItem();
        SetAwardCookies();
    }

    public void SetResultUI(bool isWin)
    {
        if (isWin)
        {
            SetResult();
            _battleVictoryUI.SetActive(true);
        }
        else 
        {
            _battleDefeatUI.SetActive(true);
        }

        _awardCookiesTransform.SetActive(true);
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
        List<StageItem> itemDatas = BattleManager.Instance.StageData.StageItemList;
        for (int i = 0;i < itemDatas.Count;i++) 
        {
            StageItem item = itemDatas[i];
            if (item != null)
            {
                GameObject itemSlot = Instantiate(_itemSlotPrefab, _itemSlotParentTransform);
                itemSlot.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite = DataManager.Instance.GetItemData(item.Key).Sprite;
                itemSlot.transform.GetChild(0).GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = item.Value.ToString();
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

        foreach (Transform child in _awardCookiesTransform.transform)
        {
            child.gameObject.SetActive(false);
        }

        int cnt = 0;
        foreach (List<int> cookieList in battleCookies)
        {
            foreach (int cookieKey in cookieList)
            {
                GameObject awardCookie = _awardCookiesTransform.transform.GetChild(cnt).gameObject;
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

            _awardCookiesTransform.transform.GetChild(i).GetChild(0).GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(0, aniName, true);
           //_awardCookiesTransform.transform.GetChild(i).transform.GetChild(0).GetComponent<SkeletonGraphic>().Initialize(true);
        }
    }
}
