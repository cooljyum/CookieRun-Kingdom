using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCookies : MonoBehaviour
{
    [SerializeField]
    private List<List<GameObject>> _battleCookies = new List<List<GameObject>>(); // 배틀 쿠키 리스트
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

        // 각 위치에 대한 이름
        string[] positions = { "Front", "Middle", "Back" };

        for (int i = 0; i < cookieKeys.Count; i++)
        {
            // 해당 위치 오브젝트 찾기
            Transform positionTransform = battleCookiesObject.transform.GetChild(i);
            if (positionTransform == null)
            {
                Debug.LogError($"{positions[i]} object not found!");
                continue;
            }

            // Solo와 Duo 오브젝트 찾기
            GameObject soloObject = positionTransform.GetChild(0).gameObject; // Solo Object
            GameObject duoObject = positionTransform.GetChild(1).gameObject; // Duo Object

            _battleCookies.Add(new List<GameObject>()); 
            // 리스트의 길이에 따라 Solo와 Duo 활성화 상태 설정
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
                // 데이터가 없는 위치는 Solo와 Duo 모두 비활성화
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
                // 각각의 자식 오브젝트에 대해 프리팹을 추가
                Transform child = parent.GetChild(i);
                GameObject newCookiePrefab = Instantiate(prefab, child);
                newCookiePrefab.transform.localPosition = Vector3.zero;


                //key에 따라 캐릭터 설정
                CharacterData characterData = DataManager.Instance.GetCharacterData(cookies[i]);
                SkeletonAnimation skeletonAnimation = newCookiePrefab.GetComponentInChildren<SkeletonAnimation>();
                if (skeletonAnimation != null && characterData.SkeletonDataAsset != null)
                {
                    skeletonAnimation.skeletonDataAsset = characterData.SkeletonDataAsset;
                    skeletonAnimation.Initialize(true); // 새로운 SkeletonDataAsset을 적용하기 위해 초기화
                }

                // _battleCookies 리스트에 추가
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
