using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftUI : MonoBehaviour
{
    [SerializeField]
    private Transform _itemCellContent;

    private GameObject _itemPrefab;

    private void Awake()
    {
        _itemPrefab = Resources.Load<GameObject>("Prefabs/Kingdom/CraftItemCell");
    }

    public void CreateCraftItem(BuildingData data) //건물 데이터에 맞춰 생산 칸 삭제 & 생성
    {
        for(int i = 0; i < _itemCellContent.childCount; i++)
        {
            Destroy(_itemCellContent.GetChild(i));
        }

        for (int i = 0; i < data.CraftInfos.Count; i++)
        {
            GameObject itemObj = Instantiate(_itemPrefab, _itemCellContent);
            itemObj.GetComponent<CraftItemUI>().SetData(data.CraftInfos[i]);
        }
    }


    public void OnClickExitBtn() //Craft-나가기
    {
        print("ExitBtn Click");
        gameObject.SetActive(false);
    }
}
