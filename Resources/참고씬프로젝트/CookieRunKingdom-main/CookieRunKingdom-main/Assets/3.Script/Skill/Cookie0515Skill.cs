using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cookie0515Skill : BaseRangeSkill
{
    [SerializeField] private DetectRange _detectSkillRange;
    [SerializeField] private BaseProjectile _skillProjectilePrafab;
    [SerializeField] private Transform _firePoint;

    private int _skillIndex = 0;

    private Queue<BaseProjectile> _pool;
    private int _projectileSkillCount = 12;

    public override void Init(BaseController controller)
    {
        base.Init(controller);

        _pool = new Queue<BaseProjectile>();
        for (int i = 0; i < _projectileSkillCount; i++)
        {
            BaseProjectile projectile = Instantiate(_skillProjectilePrafab, transform);
            projectile.gameObject.SetActive(false);
            projectile.OnDisableEvent += ((projectileObject) => ReturnSkillProjectile(projectileObject));
            _pool.Enqueue(projectile);
        }
    }

    public BaseProjectile GetSkillProjectile()
    {
        if (_pool.Count == 0)
            return null;

        return _pool.Dequeue();
    }

    public void ReturnSkillProjectile(BaseProjectile projectile)
    {
        _pool.Enqueue(projectile);
    }

    public override bool IsReadyToUseSkill()
    {
        return _detectSkillRange.enemies.Count != 0;
    }

    public override void NormalAttack()
    {
        base.NormalAttack();
    }

    public override void NormalAttackEvent()
    {
        base.NormalAttackEvent();
    }

    private bool isShoot = false;

    public override void OnSkillEvent(int index)
    {
        base.OnSkillEvent(index);

        isShoot = !isShoot;

        if (isShoot)
        {
            CharacterBattleController target = null;

            if (_detectSkillRange.enemies.Count != 0)
            {
                target = _detectSkillRange.enemies[0];
                BaseProjectile baseProjectile = GetSkillProjectile();
                if (baseProjectile != null)
                {
                    baseProjectile.ShootProjectile(((target.transform.position) - _firePoint.position).normalized);
                    baseProjectile.gameObject.SetActive(true);
                }
            }
        }
    }

    public override void SetLayer(LayerMask layer)
    {
        base.SetLayer(layer);

        BaseProjectile[] projectiles = _pool.ToArray();
        for (int i = 0; i < projectiles.Length; i++)
        {
            projectiles[i].Init(AttackPower / 3, layer, _firePoint, Vector3.zero, _controller.CharacterStat);
        }

        _detectSkillRange.Init(layer);
    }

    private int _animationCount = 0;

    public override bool UseSkill()
    {
        if(_skillIndex == 0)
        {
            PlayAnimation(animationName[_skillIndex++], false);
        }
        else if(_skillIndex == 1)
        {
            if(!_controller.CharacterAnimator.IsPlayingAnimation())
            {
                PlayAnimation(animationName[1], false);
                PlayAnimation(animationName[0], false);
                PlayAnimation(animationName[1], false);
                _animationCount++;
                if (_animationCount >= 4)
                    _skillIndex++;
            }
        }
        else if(_skillIndex == 2)
        {
            if (!_controller.CharacterAnimator.IsPlayingAnimation())
            {
                _animationCount = 0;
                _skillIndex = 0;
                return false;
            }
        }

        return true;
    }
}
