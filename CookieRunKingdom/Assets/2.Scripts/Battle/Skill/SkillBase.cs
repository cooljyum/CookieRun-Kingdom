using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillBase : ScriptableObject
{
    public bool IsPlay;

    public string SkillName;
    public float Cooldown;

    [SerializeField]
    public GameObject EffectPrefab;
    public GameObject EffectObj;

    public abstract void UseSkill(GameObject user, GameObject target);
}
