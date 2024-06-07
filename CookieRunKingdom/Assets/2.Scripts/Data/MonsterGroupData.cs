using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Scriptable Object/MonsterGroupData", order = 1)]
public class MonsterGroupData : ScriptableObject
{
    [Header("Basic Info")]
    public int Key; // 고유 식별자
    public string Name; // 이름

    [Header("MonsterGroup Info")]
    public List<int> FrontMonsterKeyList; // 전방 몬스터 Key 리스트
    public List<int> MiddleMonsterKeyList; // 중방 몬스터 Key 리스트
    public List<int> BackMonsterKeyList; // 후방 몬스터 Key 리스트
}

