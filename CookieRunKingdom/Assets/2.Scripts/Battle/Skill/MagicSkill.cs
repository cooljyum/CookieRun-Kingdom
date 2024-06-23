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
        
        IsPlay = true;
        MagicEffect(target, true);
        // CoroutineRunner.Instance.StartCoroutine(MagicEffect(target));
    }

    private void MagicEffect(GameObject target, bool isActive = true)
    {
        Debug.Log("MagicEffect!") ;
        IsPlay = false;
        target.GetComponent<BattleObject>().Damage(MagicDamage);

        if (EffectObj == null) return;

        EffectObj.transform.position = target.transform.position;
        EffectObj.SetActive(isActive);

        float elapsed = 0f;
        /*        while (elapsed < _effectDuration)
                {
                    target.GetComponent<BattleObject>().Damage(_magicDamage);
                    elapsed += Time.deltaTime;
                }*/
        CoroutineRunner.Instance.StartCoroutine(DeactivateEffectAfterTime(1f));
    }

    private IEnumerator DeactivateEffectAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        EffectObj.SetActive(false);
    }
}
