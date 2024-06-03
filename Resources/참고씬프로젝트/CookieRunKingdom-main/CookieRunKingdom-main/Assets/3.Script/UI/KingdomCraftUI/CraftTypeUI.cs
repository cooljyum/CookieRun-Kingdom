using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftTypeUI : MonoBehaviour
{
    [SerializeField] private Image craftItemImage;
    [SerializeField] private TextMeshProUGUI craftItemCount;
    [SerializeField] private TextMeshProUGUI craftItemName;
    [SerializeField] private TextMeshProUGUI craftTime;

    [SerializeField] protected Button craftButton;

    public virtual void Init(BuildingController building, CraftData craftData, System.Action<CraftData> action = null)
    {
        craftItemImage.sprite = craftData.CraftImage;
        craftItemCount.text = DataBaseManager.Instance.MyDataBase.itemDataBase[craftData.CraftResult.ingredientItem].ToString();

        craftItemName.text = craftData.CraftName;

        craftTime.text = Utils.GetTimeText(craftData.CraftTime);

        craftButton.onClick.RemoveAllListeners();
        craftButton.onClick.AddListener(() =>
        {
            if(building.BuildingWorker.Worker == null)
            {
                GuideDisplayer.Instance.ShowGuide("작업할 쿠키가 없습니다!");
            }
            else
            {
                action?.Invoke(craftData);
            }
        });
    }

    public virtual void UpdateUI()
    {

    }
}
