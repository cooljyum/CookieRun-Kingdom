using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Scriptable Object/StageData", order = 1)]
public class StageData : ScriptableObject
{
    [Header("Basic Info")]
    public int Key; // 고유 식별자
    public string Name; // 이름

    [Header("Stage Info")]
    public List<int> MonsterGroupList; // 몬스터그룹 리스트
    public List<StageItem> StageItemList; // 아이템 리스트
}

[System.Serializable]
public class StageItem
{
    public int Key; // 아이템 키
    public float Value; // 아이템 얻는 수치
}
