using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPiece : MonoBehaviour
{
    private MapScrolling _mapScrolling;
    public void Init(MapScrolling mapScrolling)
    {
        _mapScrolling = mapScrolling;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("CookieBundle"))
        {
            StartCoroutine(CoTrigger(collision));
        }
    }

    // 프레임간의 x변화량을 알기 위해 코루틴을 썼네요.
    private IEnumerator CoTrigger(Collider2D collision)
    {
        float x = collision.transform.position.x;
        yield return null;
        
        if(x <= collision.transform.position.x)
        {
            _mapScrolling.ReArrange(true);
            BattleManager.instance.TrySpawnNextWave();
        }
        else
        {
            _mapScrolling.ReArrange(false);
        }
    }
}
