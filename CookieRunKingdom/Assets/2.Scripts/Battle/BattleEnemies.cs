using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BattleEnemies : MonoBehaviour
{
    private List<List<List<int>>> _battleEnemiesKeys = new List<List<List<int>>>();

    [SerializeField]
    private List<List<GameObject>> _battleCookie = new List<List<GameObject>>(); // ��Ʋ ��Ű ����Ʈ

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

        // �� ��ġ�� ���� �̸�
        string[] positions = { "Front", "Middle", "Back" };

        for (int i = 0; i < enemyKeys.Count; i++)
        {
            // �ش� ��ġ ������Ʈ ã��
            Transform positionTransform = teamPrefabObject.transform.GetChild(i);
            if (positionTransform == null)
            {
                Debug.LogError($"{positions[i]} object not found!");
                continue;
            }

            // Solo�� Duo ������Ʈ ã��
            GameObject soloObject = positionTransform.GetChild(0).gameObject; // Solo Object
            GameObject duoObject = positionTransform.GetChild(1).gameObject; // Duo Object
            GameObject squadObject = positionTransform.GetChild(2).gameObject; // Squad Object

            // ����Ʈ�� ���̿� ���� Solo�� Duo Ȱ��ȭ ���� ����
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
                // �����Ͱ� ���� ��ġ�� ��� ��Ȱ��ȭ
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
                // ������ �ڽ� ������Ʈ�� ���� �������� �߰�
                Transform child = parent.GetChild(i);
                GameObject newCookiePrefab = Instantiate(prefab, child);
                newCookiePrefab.transform.localPosition = Vector3.zero;

                // key�� ���� ĳ���� ����
                MonsterData monsterData = DataManager.Instance.GetMonsterData(cookies[i]);
                SkeletonAnimation skeletonAnimation = newCookiePrefab.GetComponentInChildren<SkeletonAnimation>();
                if (skeletonAnimation != null && monsterData.SkeletonDataAsset != null)
                {
                    skeletonAnimation.skeletonDataAsset = monsterData.SkeletonDataAsset;
                    skeletonAnimation.Initialize(true); // ���ο� SkeletonDataAsset�� �����ϱ� ���� �ʱ�ȭ
                }
            }
            else
            {
                Debug.LogWarning("Not enough child objects to assign prefabs to");
                break;
            }
        }
    }
}
