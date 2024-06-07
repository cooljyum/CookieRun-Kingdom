using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;

public abstract class BattleEntitiesBase : MonoBehaviour
{
    protected abstract GameObject LoadPrefab();
    protected abstract SkeletonAnimation GetSkeletonAnimation(GameObject obj);
    protected abstract CharacterData GetCharacterData(int key);

    protected void AddEntitiesToObject(Transform parent, GameObject prefab, List<int> keys)
    {
        for (int i = 0; i < keys.Count; i++)
        {
            Transform child = parent.GetChild(i);
            GameObject newEntityPrefab = Instantiate(prefab, child);
            newEntityPrefab.transform.localPosition = Vector3.zero;

            // key에 따라 캐릭터 설정
            CharacterData characterData = GetCharacterData(keys[i]);
            SkeletonAnimation skeletonAnimation = GetSkeletonAnimation(newEntityPrefab);
            if (skeletonAnimation != null && characterData.SkeletonDataAsset != null)
            {
                skeletonAnimation.skeletonDataAsset = characterData.SkeletonDataAsset;
                skeletonAnimation.Initialize(true); // 새로운 SkeletonDataAsset을 적용하기 위해 초기화
            }
        }
    }
}
