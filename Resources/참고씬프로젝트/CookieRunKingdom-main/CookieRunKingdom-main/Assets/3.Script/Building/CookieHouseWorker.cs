using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookieHouseWorker : BuildingWorker
{
    [SerializeField] private ItemData _expCandyData;

    public override void Init(BuildingController controller)
    {
        _controller = controller;
    }

    public override bool TryHarvest()
    {
        Harvest();
        return true;
    }

    protected override void Harvest()
    {
        DataBaseManager.Instance.AddItem(_expCandyData, 10);
        GuideDisplayer.Instance.ShowGuide("∫∞ªÁ≈¡ " + 10 + "∞≥ »πµÊ");
    }

    public override void LoadBuilding()
    {
        BuildingInfo buildingInfo = GameManager.Game.OwnedCraftableBuildings[_controller.Data.BuildingIndex];

        IsCraftable = buildingInfo.isCraftable;

        if (buildingInfo.isInstall)
        {
            _controller.BuildingEditor.IsFlip = buildingInfo.isFlip;
            _controller.BuildingAnimator.FlipX(_controller.BuildingEditor.IsFlip);


            transform.position = buildingInfo.installationPosition;
            transform.SetGridTransform();

            _controller.BuildingEditor.IsInstance = true;
            _controller.BuildingEditor.OnClickEditMode();
            _controller.BuildingEditor.PutBuilding();
            BuildingPreviewTileObjectPool.instance.ResetPreviewTile();
        }
    }

    public override void SaveBuilding()
    {
        BuildingInfo buildingInfo = GameManager.Game.OwnedCraftableBuildings[_controller.Data.BuildingIndex];

        buildingInfo.isCraftable = IsCraftable;
        buildingInfo.installationPosition = transform.position;
        buildingInfo.isInstall = _controller.BuildingEditor.IsInstance;
        buildingInfo.isFlip = _controller.BuildingEditor.IsFlip;
    }
}
