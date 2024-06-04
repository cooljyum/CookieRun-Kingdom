using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : Singleton<BattleManager>
{
    [Header ("BattleCookie")]
    [SerializeField]
    private List<int> _battleCookieKeys = new List<int>(); // ��Ʋ ��Ű Ű ����Ʈ

    [SerializeField]
    private List<GameObject> _battleCookies = new List<GameObject>(); // ��Ʋ ��Ű ����Ʈ

    
    private void Start()
    {
        CreateBattle(); // ��Ʋ ������Ʈ ���� �Լ� ȣ��
        Init(); // �ʱ�ȭ �Լ� ȣ��
        TestGame();
        StartBattle(); // ��Ʋ ���� �Լ� ȣ��
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

            // child.gameObject�� �ڽ����� battleCookiePrefab�� �߰�
            GameObject newCookiePrefab = Instantiate(battleCookiePrefab, child);
            newCookiePrefab.transform.localPosition = Vector3.zero; // ��ġ�� ������ �ʿ䰡 ���� ���
        }
    }

    private void Init()
    {
        _battleCookieKeys.Clear(); // ��Ʋ ��Ű Ű ����Ʈ �ʱ�ȭ
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

            _battleCookies[i].SetActive(true); // �� ��Ʋ ��Ű Ȱ��ȭ
            CharacterData characterData = DataManager.Instance.GetCharacterData(key);
            SkeletonAnimation skeletonAnimation = _battleCookies[i].GetComponentInChildren<SkeletonAnimation>();
            if (skeletonAnimation != null && characterData.SkeletonDataAsset != null)
            {
                skeletonAnimation.skeletonDataAsset = characterData.SkeletonDataAsset;
                skeletonAnimation.Initialize(true); // ���ο� SkeletonDataAsset�� �����ϱ� ���� �ʱ�ȭ
            }
        }
    }
}
