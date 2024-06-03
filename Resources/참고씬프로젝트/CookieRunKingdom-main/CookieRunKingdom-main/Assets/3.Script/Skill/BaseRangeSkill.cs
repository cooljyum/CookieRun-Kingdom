using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseRangeSkill : BaseSkill
{
    [SerializeField] private BaseProjectile _baseProjectilePrefab;

    private Queue<BaseProjectile> _pool;
    private int _projectileCount = 4;

    public override void Init(BaseController controller)
    {
        base.Init(controller);

        _pool = new Queue<BaseProjectile>();
        for (int i = 0; i < _projectileCount; i++)
        {
            BaseProjectile projectile = Instantiate(_baseProjectilePrefab, transform);
            projectile.gameObject.SetActive(false);
            projectile.OnDisableEvent += ((projectileObject) => ReturnProjectile(projectileObject));
            _pool.Enqueue(projectile);
        }
    }

    public BaseProjectile GetProjectile()
    {
        if (_pool.Count == 0)
            return null;

        return _pool.Dequeue();
    }

    public void ReturnProjectile(BaseProjectile projectile)
    {
        _pool.Enqueue(projectile);
    }

    public override void SetLayer(LayerMask layer)
    {
        base.SetLayer(layer);

        BaseProjectile[] projectiles = _pool.ToArray();
        for(int i = 0; i < projectiles.Length; i++)
        {
            projectiles[i].Init(AttackPower, layer, transform, Vector3.up * 0.8f, _controller.CharacterStat);
        }
    }

    public override void NormalAttackEvent()
    {
        CharacterBattleController target = DetectTarget();
        if (target != null)
        {
            BaseProjectile baseProjectile = GetProjectile();
            if (baseProjectile != null)
            {
                baseProjectile.ShootProjectile(((target.transform.position) - transform.position).normalized);
                baseProjectile.gameObject.SetActive(true);
            }
        }
    }

    public override void OnSkillEvent(int index)
    {
    }

    public override bool UseSkill()
    {
        return true;
    }
}
