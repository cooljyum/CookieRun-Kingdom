using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

[CreateAssetMenu(fileName = "MagicSkill", menuName = "Scriptable Object/Skills/MagicSkill")]
public class MagicSkill : SkillBase
{
    public float MagicDamage;
    public float EffectDuration;

    public override void UseSkill(GameObject user, GameObject target)
    {
        Debug.Log($"MagicSkill({SkillName}):{user.name}->{target.name}, [Damage:{MagicDamage}, Duration:{EffectDuration}]");
        
        IsPlay=true;
        MagicEffect(target);
        // CoroutineRunner.Instance.StartCoroutine(MagicEffect(target));
    }

    private void MagicEffect(GameObject target)
    {
        Debug.Log("MagicEffect!") ;
        IsPlay = false;
        target.GetComponent<BattleObject>().Damage(MagicDamage);


        float elapsed = 0f;
/*        while (elapsed < _effectDuration)
        {
            target.GetComponent<BattleObject>().Damage(_magicDamage);
            elapsed += Time.deltaTime;
        }*/
    }
}
