using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectRange : MonoBehaviour
{ 
    private LayerMask _enemyLayer;
    public List<CharacterBattleController> enemies = new List<CharacterBattleController>();

    public void Init(LayerMask targetLayer)
    {
        _enemyLayer = targetLayer;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == _enemyLayer)
        {
            CharacterBattleController enemy = collision.GetComponent<CharacterBattleController>();
            if (enemy != null)
                enemies.Add(enemy);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.layer == _enemyLayer)
        {
            CharacterBattleController enemy = collision.GetComponent<CharacterBattleController>();
            if (enemy != null)
                enemies.Remove(enemy);
        }
    }
}
