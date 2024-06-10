using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BattleEntitiesBase : MonoBehaviour
{
    protected List<List<GameObject>> _battleEntities = new List<List<GameObject>>();
    public List<List<GameObject>> BattleEntitiesList => _battleEntities;

    protected void CreateBattleEntities(List<List<int>> entityKeys, string parentObjectName, string prefabPath, bool isSpawnTeam = false, Vector2 position = default, bool hasSquad = false, bool isEnemy = false)
    {
        //예외처리
        GameObject battleEntitiesObject = GameObject.Find(parentObjectName);
        if (battleEntitiesObject == null)
        {
            Debug.LogError($"{parentObjectName} object not found!");
            return;
        }

        GameObject entityPrefab = Resources.Load<GameObject>(prefabPath);
        if (entityPrefab == null)
        {
            Debug.LogError($"{prefabPath} prefab not found!");
            return;
        }

        //Team Prefeb
        if (isSpawnTeam)
        {
            GameObject teamPrefab = Resources.Load<GameObject>("Prefabs/Battle/Team");
            GameObject teamPrefabObject = Instantiate(teamPrefab, battleEntitiesObject.transform);
            battleEntitiesObject = teamPrefabObject;
            if (position != Vector2.zero)
            {
                teamPrefabObject.transform.localPosition = position; // 위치 설정
            }
        }

        string[] positions = { "Front", "Middle", "Back" };

        _battleEntities.Clear();

        for (int i = 0; i < entityKeys.Count; i++)
        {
            Transform positionTransform = battleEntitiesObject.transform.GetChild(i);
            if (positionTransform == null)
            {
                Debug.LogError($"{positions[i]} object not found!");
                continue;
            }

            GameObject soloObject = positionTransform.GetChild(0).gameObject;
            GameObject duoObject = positionTransform.GetChild(1).gameObject;
            GameObject squadObject = hasSquad ? positionTransform.GetChild(2).gameObject : null;

            _battleEntities.Add(new List<GameObject>());
            List<int> entities = entityKeys[i];

            if (entities.Count == 1)
            {
                soloObject.SetActive(true);
                duoObject.SetActive(false);
                if (hasSquad) squadObject.SetActive(false);
                AddEntitiesToObject(soloObject.transform, entityPrefab, entities, i, isEnemy);
            }
            else if (entities.Count == 2)
            {
                soloObject.SetActive(false);
                duoObject.SetActive(true);
                if (hasSquad) squadObject.SetActive(false);
                AddEntitiesToObject(duoObject.transform, entityPrefab, entities, i, isEnemy);
            }
            else if (hasSquad && entities.Count == 3)
            {
                soloObject.SetActive(false);
                duoObject.SetActive(false);
                squadObject.SetActive(true);
                AddEntitiesToObject(squadObject.transform, entityPrefab, entities, i, isEnemy);
            }
            else
            {
                soloObject.SetActive(false);
                duoObject.SetActive(false);
                if (hasSquad) squadObject.SetActive(false);
            }
        }
    }

    protected void AddEntitiesToObject(Transform parent, GameObject prefab, List<int> entities, int positionIndex, bool isEnemy = false)
    {
        int childCount = parent.childCount;

        for (int i = 0; i < entities.Count; i++)
        {
            if (i < childCount)
            {
                Transform child = parent.GetChild(i);
                GameObject newEntityPrefab = Instantiate(prefab, child);
                newEntityPrefab.transform.localPosition = Vector3.zero;

                CharacterData characterData = GetCharacterData(entities[i]);
                newEntityPrefab.GetComponent<BattleObject>().SetCharacterData(characterData);
                newEntityPrefab.GetComponent<BattleObject>().IsEnemy = isEnemy;
                SkeletonAnimation skeletonAnimation = newEntityPrefab.GetComponentInChildren<SkeletonAnimation>();
                if (skeletonAnimation != null && characterData.SkeletonDataAsset != null)
                {
                    skeletonAnimation.skeletonDataAsset = characterData.SkeletonDataAsset;
                    skeletonAnimation.Initialize(true);
                }

                _battleEntities[positionIndex].Add(newEntityPrefab);
            }
            else
            {
                Debug.LogWarning("Not enough child objects to assign prefabs to");
                break;
            }
        }
    }

    protected abstract CharacterData GetCharacterData(int key);
}
