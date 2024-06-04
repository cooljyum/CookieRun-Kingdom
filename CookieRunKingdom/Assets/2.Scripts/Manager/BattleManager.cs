using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : Singleton<BattleManager>
{
    [Header ("BattleCookie")]
    [SerializeField]
    private List<int> _battleCookieKeys = new List<int>(); // 배틀 쿠키 키 리스트

    [SerializeField]
    private List<GameObject> _battleCookies = new List<GameObject>(); // 배틀 쿠키 리스트

    
    private void Start()
    {
        CreateBattle(); // 배틀 오브젝트 생성 함수 호출
        Init(); // 초기화 함수 호출
        TestGame();
        StartBattle(); // 배틀 시작 함수 호출
    }

    private void CreateBattle() 
    {
        GameObject battleCookiesObject = GameObject.Find("BattleCookies");
        if (battleCookiesObject == null)
        {
            Debug.LogError("BattleCookies object not found!");
            return;
        }

        GameObject battleCookiePrefab = Resources.Load<GameObject>("Prefabs/BattleCookie");

        foreach (Transform child in battleCookiesObject.transform)
        {
            _battleCookies.Add(child.gameObject);

            // child.gameObject의 자식으로 battleCookiePrefab을 추가
            GameObject newCookiePrefab = Instantiate(battleCookiePrefab, child);
            newCookiePrefab.transform.localPosition = Vector3.zero; // 위치를 조정할 필요가 있을 경우
        }
    }

    private void Init()
    {
        _battleCookieKeys.Clear(); // 배틀 쿠키 키 리스트 초기화
    }

    private void TestGame() 
    {
        _battleCookieKeys = new List<int> { 1,2,3,4,5 };
    }

    private void StartBattle()
    {
        for (int i = 0; i < _battleCookieKeys.Count; i++)
        {
            int key = _battleCookieKeys[i];

            _battleCookies[i].SetActive(true); // 각 배틀 쿠키 활성화
            CharacterData characterData = DataManager.Instance.GetCharacterData(key);
            SkeletonAnimation skeletonAnimation = _battleCookies[i].GetComponentInChildren<SkeletonAnimation>();
            if (skeletonAnimation != null && characterData.SkeletonDataAsset != null)
            {
                skeletonAnimation.skeletonDataAsset = characterData.SkeletonDataAsset;
                skeletonAnimation.Initialize(true); // 새로운 SkeletonDataAsset을 적용하기 위해 초기화
            }
        }
    }
}
