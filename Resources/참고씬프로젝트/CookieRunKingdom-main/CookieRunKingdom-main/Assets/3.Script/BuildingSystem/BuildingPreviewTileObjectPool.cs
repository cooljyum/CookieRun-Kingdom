using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPreviewTileObjectPool : MonoBehaviour
{
    #region 싱클톤
    public static BuildingPreviewTileObjectPool instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion

    [SerializeField] private SpriteRenderer previewTilemap;
    [SerializeField] private int maxCount = 16;

    private SpriteRenderer[] previewTilemaps;

    public void Init()
    {
        previewTilemaps = new SpriteRenderer[maxCount];

        for(int i = 0; i < maxCount; i++)
        {
            previewTilemaps[i] = Instantiate(previewTilemap, transform);
            previewTilemaps[i].gameObject.SetActive(false);
        }
    }

    // 빌딩의 크기만큼 previewTile 가져오기
    public List<SpriteRenderer> GetPreviewTile(Vector2Int size, Transform parent)
    {
        List<SpriteRenderer> resultTiles = new List<SpriteRenderer>();

        ResetPreviewTile();

        int count = size.x * size.y;

        for(int i = 0; i < count; i++)
        {
            resultTiles.Add(previewTilemaps[i]);
            previewTilemaps[i].transform.SetParent(parent);
            previewTilemaps[i].gameObject.SetActive(true);
        }

        return resultTiles;
    }

    // previewTile 초기화
    public void ResetPreviewTile()
    {
        for(int i = 0; i < maxCount; i++)
        {
            previewTilemaps[i].transform.SetParent(transform);
            previewTilemaps[i].transform.localPosition = Vector3.zero;
            previewTilemaps[i].gameObject.SetActive(false);
        }
    }
}
