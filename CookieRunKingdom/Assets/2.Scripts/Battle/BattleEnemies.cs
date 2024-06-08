using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BattleEnemies : MonoBehaviour
{
    private List<List<List<int>>> _battleEnemiesKeys = new List<List<List<int>>>();

    [SerializeField]
    private List<List<GameObject>> _battleCookie = new List<List<GameObject>>(); // 배틀 쿠키 리스트

    public List<List<List<int>>> BattleEnemiesKeysList => _battleEnemiesKeys;

    public void CreateBattleEnemiesGroup(int stage)
    {
        StageData stageData = DataManager.Instance.GetStageData(stage);
        Vector2 basePosition = new Vector2(0, 0); // Starting position
        Vector2 offset = new Vector2(20, 10); // Offset for each team

        for (int i = 0; i < stageData.MonsterGroupList.Count; i++)
        {
            MonsterGroupData monsterGroup = DataManager.Instance.GetMonsterGroupData(stageData.MonsterGroupList[i]);
            List<List<int>> monsterGroupList = new List<List<int>>();
            monsterGroupList.Add(new List<int>());
            for (int j = 0; j < monsterGroup.FrontMonsterKeyList.Count; j++)
            {
                monsterGroupList[0].Add(monsterGroup.FrontMonsterKeyList[j]);
            }
            monsterGroupList.Add(new List<int>());
            for (int j = 0; j < monsterGroup.MiddleMonsterKeyList.Count; j++)
            {
                monsterGroupList[1].Add(monsterGroup.MiddleMonsterKeyList[j]);
            }
            monsterGroupList.Add(new List<int>());
            for (int j = 0; j < monsterGroup.BackMonsterKeyList.Count; j++)
            {
                monsterGroupList[2].Add(monsterGroup.BackMonsterKeyList[j]);
            }
            _battleEnemiesKeys.Add(monsterGroupList);

            // Calculate position for this team
            Vector2 teamPosition = basePosition + (offset * i);
            CreateBattleEnemies(monsterGroupList, teamPosition);
        }
    }

    public void CreateBattleEnemies(List<List<int>> enemyKeys, Vector2 position)
    {
        GameObject battleEnemiesObject = GameObject.Find("BattleEnemies");
        if (battleEnemiesObject == null)
        {
            Debug.LogError("BattleEnemies object not found!");
            return;
        }

        GameObject teamPrefab = Resources.Load<GameObject>("Prefabs/Battle/Team");
        GameObject battleEnemiesPrefab = Resources.Load<GameObject>("Prefabs/Battle/BattleEnemy");
        if (teamPrefab == null)
        {
            Debug.LogError("Team prefab not found!");
            return;
        }

        GameObject teamPrefabObject = Instantiate(teamPrefab, battleEnemiesObject.transform);
        teamPrefabObject.transform.localPosition = position; // Set position

        // 각 위치에 대한 이름
        string[] positions = { "Front", "Middle", "Back" };

        for (int i = 0; i < enemyKeys.Count; i++)
        {
            // 해당 위치 오브젝트 찾기
            Transform positionTransform = teamPrefabObject.transform.GetChild(i);
            if (positionTransform == null)
            {
                Debug.LogError($"{positions[i]} object not found!");
                continue;
            }

            // Solo와 Duo 오브젝트 찾기
            GameObject soloObject = positionTransform.GetChild(0).gameObject; // Solo Object
            GameObject duoObject = positionTransform.GetChild(1).gameObject; // Duo Object
            GameObject squadObject = positionTransform.GetChild(2).gameObject; // Squad Object

            // 리스트의 길이에 따라 Solo와 Duo 활성화 상태 설정
            if (i < enemyKeys.Count)
            {
                List<int> cookies = enemyKeys[i];
                if (cookies.Count == 1) // Solo
                {
                    soloObject.SetActive(true);
                    duoObject.SetActive(false);
                    squadObject.SetActive(false);
                    AddCookiesToObject(soloObject.transform, battleEnemiesPrefab, cookies, i);
                }
                else if (cookies.Count == 2) // Duo
                {
                    soloObject.SetActive(false);
                    duoObject.SetActive(true);
                    squadObject.SetActive(false);
                    AddCookiesToObject(duoObject.transform, battleEnemiesPrefab, cookies, i);
                }
                else if (cookies.Count == 3) // Squad
                {
                    soloObject.SetActive(false);
                    duoObject.SetActive(false);
                    squadObject.SetActive(true);
                    AddCookiesToObject(squadObject.transform, battleEnemiesPrefab, cookies, i);
                }
            }
            else
            {
                // 데이터가 없는 위치는 모두 비활성화
                soloObject.SetActive(false);
                duoObject.SetActive(false);
                squadObject.SetActive(false);
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

                // key에 따라 캐릭터 설정
/*                MonsterData monsterData = DataManager.Instance.GetMonsterData(cookies[i]);
                SkeletonAnimation skeletonAnimation = newCookiePrefab.GetComponentInChildren<SkeletonAnimation>();
                if (skeletonAnimation != null && monsterData.SkeletonDataAsset != null)
                {
                    skeletonAnimation.skeletonDataAsset = monsterData.SkeletonDataAsset;
                    skeletonAnimation.Initialize(true); // 새로운 SkeletonDataAsset을 적용하기 위해 초기화
                }*/
            }
            else
            {
                Debug.LogWarning("Not enough child objects to assign prefabs to");
                break;
            }
        }
    }
}
