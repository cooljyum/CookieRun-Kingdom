using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMapGenerator : MonoBehaviour
{
    private StageData _stageData;

    public void GenerateBattleMap(StageData stageData)
    {
        _stageData = stageData;

        GenerateBattleTile();
    }

    private void GenerateGround(int count)
    {
        Transform pieceParent = new GameObject("Piece" + count).transform;
        pieceParent.gameObject.AddComponent<MapPiece>();
        pieceParent.SetParent(transform);

        BoxCollider2D col = pieceParent.gameObject.AddComponent<BoxCollider2D>();
        col.isTrigger = true;
        col.offset = new Vector2(3, 3);
        col.size = new Vector2(10, 1);

        for(int i = 0; i < _stageData.StagePos.childCount; i++)
        {
            GameObject tile = new GameObject();
            tile.transform.SetParent(pieceParent);
            tile.transform.position = _stageData.StagePos.GetChild(i).position;
            SpriteRenderer tileSr = tile.AddComponent<SpriteRenderer>();
            tileSr.sortingOrder = -i - 1000;
            tileSr.sprite = _stageData.MapSprites[i];

            // 여기서 장애물을 해줌
            GenerateObject(tile.transform, i);
        }
    }

    private void GenerateObject(Transform parent, int index)
    {
        Sprite sprite = null;
        int randomIndex = 0;
        bool isBoid = false;
        Vector3 size = Vector3.one;
        Vector3 pos = Vector3.zero;


        // 11로 나눈 나머지가
        // 1 ~ 5  왼쪽
        // 6 ~ 10 오른쪽
        if (index % 11 == 1)  // 왼쪽 가까운
        {
            if(_stageData.CloseLeftObjectSprites.Length != 0)
            {
                randomIndex = Random.Range(0, _stageData.CloseLeftObjectSprites.Length);
                sprite = _stageData.CloseLeftObjectSprites[randomIndex];
                isBoid = true;
                size = Vector3.one * 0.5f;
                pos = Vector3.up;
            }
        }
        else if(index % 11 == 2)  // 왼쪽 중간
        {
            if (_stageData.MiddleLeftObjectSprites.Length != 0)
            {
                if (Random.Range(0, 3) == 0)
                {
                    randomIndex = Random.Range(0, _stageData.MiddleLeftObjectSprites.Length);
                    sprite = _stageData.MiddleLeftObjectSprites[randomIndex];
                    size = Vector3.one * 0.7f;
                    pos = -Vector3.up * 0.5f + (Vector3)(Random.insideUnitCircle * 0.5f);
                }
            }
        }
        else if (index % 11 == 3) // 왼쪽 먼
        {
            if (_stageData.FarLeftObjectSprites.Length != 0)
            {
                randomIndex = Random.Range(0, _stageData.FarLeftObjectSprites.Length);
                sprite = _stageData.FarLeftObjectSprites[randomIndex];
                pos = Vector3.up * Random.Range(1f, 2f);
            }
        }
        else if (index % 11 == 6) // 오른쪽 가까운
        {
            if (_stageData.CloseRightObjectSprites.Length != 0)
            {
                randomIndex = Random.Range(0, _stageData.CloseRightObjectSprites.Length);
                sprite = _stageData.CloseRightObjectSprites[randomIndex];
                isBoid = true;
                size = Vector3.one * 0.5f;
                pos = -Vector3.up;
            }
        }
        else if (index % 11 == 7) // 오른쪽 중간
        {
            if (_stageData.MiddleRightObjectSprites.Length != 0)
            {
                if(Random.Range(0, 3) == 0)
                {
                    randomIndex = Random.Range(0, _stageData.MiddleRightObjectSprites.Length);
                    sprite = _stageData.MiddleRightObjectSprites[randomIndex];
                    size = Vector3.one * 0.7f;
                    pos = Vector3.up * 0.5f + (Vector3)(Random.insideUnitCircle * 0.5f);
                }
            }
        }
        else if (index % 11 == 8) // 오른쪽 먼
        {
            if (_stageData.FarRightObjectSprites.Length != 0)
            {
                randomIndex = Random.Range(0, _stageData.FarRightObjectSprites.Length);
                sprite = _stageData.FarRightObjectSprites[randomIndex];
                pos = -Vector3.up * Random.Range(1f, 2f);
            }
        }

        if(sprite != null)
        {
            CreateObject(sprite, parent, pos, size, isBoid, index);
        }
    }

    private void CreateObject(Sprite sprite, Transform parent, Vector3 pos, Vector3 size, bool isBoid, int index)
    {
        if (index % 11 >= 6)
            index = -index;

        GameObject obj = null;

        if(isBoid)
        {
            for(int i = 0; i < 3; i++)
            {
                obj = new GameObject();
                obj.transform.SetParent(parent);
                obj.transform.localPosition = pos + (Vector3)(Random.insideUnitCircle * 1.5f);
                obj.transform.localScale = size;
                SpriteRenderer sr = obj.AddComponent<SpriteRenderer>();
                sr.sortingOrder = -100;
                sr.sprite = sprite;
            }
        }
        else
        {
            obj = new GameObject();
            obj.transform.SetParent(parent);
            obj.transform.localPosition = pos;
            obj.transform.localScale = size;
            SpriteRenderer sr = obj.AddComponent<SpriteRenderer>();
            sr.sortingOrder = -100;
            sr.sprite = sprite;
        }
    }

    // 타일만
    private void GenerateBattleTile()
    {
        for(int i = 0; i <= 6; i++)
        {
            GenerateGround(i);
        }
    }
}
