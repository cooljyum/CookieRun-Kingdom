using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum ECraftingState
{
    empty, waiting, making, complete
}

[System.Serializable]
public class CraftingItemData
{
    public ECraftingState state = ECraftingState.empty;
    public CraftData craftData = null;
    public int takingTime = 0;

    public CraftingItemData(ECraftingState state, CraftData craftData)
    {
        this.state = state;
        this.craftData = craftData;
        takingTime = 0;
    }
}

public class CraftingItemUI : MonoBehaviour
{
    [SerializeField] private GameObject craftingItemParent;
    [SerializeField] private Image craftingItemImage;
    [SerializeField] private GameObject checkImage;

    private ECraftingState state = ECraftingState.empty;

    public void InitCraft()
    {
        state = ECraftingState.empty;
        craftingItemParent.SetActive(false);
        checkImage.SetActive(false);
    }

    // 왕국에 있을 때는 1초마다 하는것같음...
    public void UpdateCraft(CraftingItemData craftingItemData)
    {
        // 이전 상태와 다르면
        if (state != craftingItemData.state)
        {
            ExitState(state, craftingItemData);
            state = craftingItemData.state;
            EnterState(state, craftingItemData);
        }
        else
        {
            UpdateState(state, craftingItemData);
            return;
        }
    }

    private void EnterState(ECraftingState state, CraftingItemData craftingItemData)
    {
        Debug.Log(state + "  " + craftingItemData);
        Debug.Log(craftingItemData.craftData);
        Debug.Log(craftingItemData.craftData.CraftImage);

        switch (state)
        {
            case ECraftingState.empty:
                craftingItemParent.SetActive(false);
                break;
            case ECraftingState.waiting:
                craftingItemImage.sprite = craftingItemData.craftData.CraftImage;
                break;
            case ECraftingState.making:
                craftingItemImage.sprite = craftingItemData.craftData.CraftImage;
                // 시간표가 나옴
                break;
            case ECraftingState.complete:
                craftingItemImage.sprite = craftingItemData.craftData.CraftImage;
                checkImage.SetActive(true);
                break;
        }
    }

    private void ExitState(ECraftingState state, CraftingItemData craftingItemData)
    {
        switch (state)
        {
            case ECraftingState.empty:
                craftingItemParent.SetActive(true);
                break;
            case ECraftingState.waiting:
                break;
            case ECraftingState.making:
                // 시간표가 꺼짐
                break;
            case ECraftingState.complete:
                checkImage.SetActive(false);
                break;
        }
    }

    private void UpdateState(ECraftingState state, CraftingItemData craftingItemData)
    {
        switch (state)
        {
            case ECraftingState.waiting:
                // 시간표가 update됨
                break;
        }
    }
}
