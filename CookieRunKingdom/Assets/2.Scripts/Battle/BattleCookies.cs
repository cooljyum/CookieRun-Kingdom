using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCookies : MonoBehaviour
{
    [SerializeField]
    private List<List<GameObject>> _battleCookies = new List<List<GameObject>>(); // ��Ʋ ��Ű ����Ʈ

    public List<List<GameObject>> BattleCookieList => _battleCookies;

    public void CreateBattleCookies(List<List<int>> cookieKeys)
    {
        GameObject battleCookiesObject = GameObject.Find("BattleCookies");
        if (battleCookiesObject == null)
        {
            Debug.LogError("BattleCookies object not found!");
            return;
        }

        GameObject battleCookiePrefab = Resources.Load<GameObject>("Prefabs/Battle/BattleCookie");
        if (battleCookiePrefab == null)
        {
            Debug.LogError("BattleCookie prefab not found!");
            return;
        }

        // �� ��ġ�� ���� �̸�
        string[] positions = { "Front", "Middle", "Back" };

        for (int i = 0; i < cookieKeys.Count; i++)
        {
            // �ش� ��ġ ������Ʈ ã��
            Transform positionTransform = battleCookiesObject.transform.GetChild(i);
            if (positionTransform == null)
            {
                Debug.LogError($"{positions[i]} object not found!");
                continue;
            }

            // Solo�� Duo ������Ʈ ã��
            GameObject soloObject = positionTransform.GetChild(0).gameObject; // Solo Object
            GameObject duoObject = positionTransform.GetChild(1).gameObject; // Duo Object

            _battleCookies.Add(new List<GameObject>()); 
            // ����Ʈ�� ���̿� ���� Solo�� Duo Ȱ��ȭ ���� ����
            if (i < cookieKeys.Count)
            {
                List<int> cookies = cookieKeys[i];
                if (cookies.Count == 1) // Solo
                {
                    soloObject.SetActive(true);
                    duoObject.SetActive(false);
                    AddCookiesToObject(soloObject.transform, battleCookiePrefab, cookies, i);
                }
                else if (cookies.Count == 2) // Duo
                {
                    soloObject.SetActive(false);
                    duoObject.SetActive(true);
                    AddCookiesToObject(duoObject.transform, battleCookiePrefab, cookies, i);
                }
            }
            else
            {
                // �����Ͱ� ���� ��ġ�� Solo�� Duo ��� ��Ȱ��ȭ
                soloObject.SetActive(false);
                duoObject.SetActive(false);
            }
        }
    }

    void AddCookiesToObject(Transform parent, GameObject prefab, List<int> cookies, int positionIndex)
    {
        int childCount = parent.childCount;

        for (int i = 0; i < cookies.Count; i++)
        {
            if (i < childCount)
            {
                // ������ �ڽ� ������Ʈ�� ���� �������� �߰�
                Transform child = parent.GetChild(i);
                GameObject newCookiePrefab = Instantiate(prefab, child);
                newCookiePrefab.transform.localPosition = Vector3.zero;


                //key�� ���� ĳ���� ����
                CharacterData characterData = DataManager.Instance.GetCharacterData(cookies[i]);
                SkeletonAnimation skeletonAnimation = newCookiePrefab.GetComponentInChildren<SkeletonAnimation>();
                if (skeletonAnimation != null && characterData.SkeletonDataAsset != null)
                {
                    skeletonAnimation.skeletonDataAsset = characterData.SkeletonDataAsset;
                    skeletonAnimation.Initialize(true); // ���ο� SkeletonDataAsset�� �����ϱ� ���� �ʱ�ȭ
                }

                // _battleCookies ����Ʈ�� �߰�
                _battleCookies[positionIndex].Add(newCookiePrefab);
            }
            else
            {
                Debug.LogWarning("Not enough child objects to assign prefabs to");
                break;
            }
        }
    }
}
