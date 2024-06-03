using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Spine.Unity;

public class BuildingInfoUI : BaseUI
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI _buildingNameText;
    [SerializeField] private TextMeshProUGUI _buildingSizeText;
    [SerializeField] private TextMeshProUGUI _buildingScoreText;
    [SerializeField] private TextMeshProUGUI _buildingExplain;
    [SerializeField] private Button _okayButton;

    [SerializeField] private SkeletonGraphic _skeletonGraphic;

    private BuildingController _building;

    public void SetBuilding(BuildingController building)
    {
        _building = building;
    }

    public override void Hide()
    {
        base.Hide();
    }

    public override void Init()
    {
        base.Init();

        _okayButton.onClick.AddListener(() => GameManager.UI.ExitPopUpUI());
    }

    public override void Show()
    {
        base.Show();

        _buildingNameText.text = _building.Data.BuildingName;
        _buildingSizeText.text = _building.Data.BuildingSize.x + "X" + _building.Data.BuildingSize.y;
        _buildingScoreText.text = _building.Data.BuildingScore.ToString();
        _buildingExplain.text = _building.Data.BuildingExplain;

        _skeletonGraphic.skeletonDataAsset = _building.Data.SkeletonData;
        _skeletonGraphic.Initialize(true);
        _skeletonGraphic.AnimationState.SetAnimation(0, "loop_back", true);
    }
}
