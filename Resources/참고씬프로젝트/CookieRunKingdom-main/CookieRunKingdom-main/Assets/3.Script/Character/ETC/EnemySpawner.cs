using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform[] positions;

    private StageData _stageData;
    private int _index = 0;

    public void Init(StageData stageData)
    {
        _index = 0;
        _stageData = stageData;

        transform.position = Utils.Dir * 3;
    }

    public void SpawnEnemy()
    {
        BaseController[] enemies = _stageData.WaveInfo[_index++].enemies;
        for(int i = 0; i < enemies.Length; i++)
        {
            if(enemies[i] != null)
            {
                BaseController enemy = Instantiate(enemies[i], positions[i]);
                enemy.transform.localPosition = Vector3.zero;
                enemy.transform.SetParent(null);

                enemy.gameObject.layer = LayerMask.NameToLayer("Enemy");
                enemy.CharacterBattleController.StartBattle(false);
                enemy.CharacterBattleController.ChangeState(EBattleState.BattleRunState);
                enemy.CharacterBattleController.SetEnemy(LayerMask.NameToLayer("Cookie"));

                SkeletonAnimation originalPrefab = enemies[i].GetComponentInChildren<SkeletonAnimation>();
                SkeletonAnimation newObject = enemy.GetComponentInChildren<SkeletonAnimation>();

                if(originalPrefab != null && newObject != null)
                {
                    Renderer originalRenderer = originalPrefab.GetComponent<Renderer>();
                    Renderer newRenderer = newObject.GetComponent<Renderer>();

                    if(originalRenderer != null && newRenderer != null)
                    {
                        newRenderer.sharedMaterials = originalRenderer.sharedMaterials;
                    }
                }
            }
        }
    }
}
