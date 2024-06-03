using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapScrolling : MonoBehaviour
{
    [Header("테스트입니다.")]
    [SerializeField] private MapPiece[] mapPieces;

    private Vector3 diffForPieces;
    private LinkedList<Transform> mapList;

    public void Init()
    {
        mapPieces = GetComponentsInChildren<MapPiece>();
        diffForPieces = Utils.Dir;

        mapList = new LinkedList<Transform>();

        transform.position -= diffForPieces * 3;

        for (int i = 0; i < mapPieces.Length; i++)
        {
            mapPieces[i].Init(this);

            mapList.AddLast(mapPieces[i].transform);
            mapPieces[i].transform.localPosition += diffForPieces * i;
        }
        ReArrangeZOrder();
    }

    /// <summary>
    /// 충돌할 때 방향이 같으면 true해서 제일 먼저 넣은걸 빼고 위치 조정하고 다시 끝에 넣어줌
    /// 충돌할 때 방향이 반대면 false해서 제일 나중에 넣은걸 빼서 위치 조정하고 다시 처음에 넣어줌
    /// </summary>
    /// <param name="isDirection"></param>
    public void ReArrange(bool isDirection = true)
    {
        // 앞으로 가면서 부딪혔을 때
        if(isDirection)
        {
            Transform mapPiece = mapList.First.Value;
            mapList.RemoveFirst();

            // mapPiece 위치 조정
            mapPiece.localPosition = mapList.Last.Value.localPosition + diffForPieces;

            mapList.AddLast(mapPiece);
        }
        else
        {
            Transform mapPiece = mapList.Last.Value;
            mapList.RemoveLast();

            // mapPiece 위치 조정
            mapPiece.localPosition = mapList.First.Value.localPosition - diffForPieces;

            mapList.AddFirst(mapPiece);
        }
    }

    private void ReArrangeZOrder()
    {
        for (int i = 0; i < mapPieces.Length; i++)
            foreach (Transform tile in mapPieces[i].transform)
                if (tile.childCount != 0)
                    foreach (Transform child in tile)
                        if (child != tile)
                        {
                            float height = child.GetComponent<SpriteRenderer>().sprite.rect.y / 2;
                            child.position = new Vector3(child.position.x, child.position.y, (child.position.y + height) / 1000);
                        }
    }
}
