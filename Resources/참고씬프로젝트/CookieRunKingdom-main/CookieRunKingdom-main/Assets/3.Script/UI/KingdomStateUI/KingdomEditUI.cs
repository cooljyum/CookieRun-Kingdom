using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Spine.Unity;

public class KingdomEditUI : BaseUI
{
    [Header("RightTop")]
    [SerializeField] private TextMeshProUGUI _diaText;
    [SerializeField] private TextMeshProUGUI _moneyText;

    [Header("LeftTop")]
    [SerializeField] private TextMeshProUGUI _environmentScoreText;

    [Header("Bottom")]
    [SerializeField] private Transform editInventory;
    [SerializeField] private GameObject editSelected;
    [SerializeField] private Image editItemImage;
    [SerializeField] private TextMeshProUGUI editItemNameText;
    [SerializeField] private Button editExitButton;
    [SerializeField] private Button editCheckButton;

    [Header("Prefabs")]
    [SerializeField] private Transform _buildingParent;
    [SerializeField] private RectTransform editItemParent;
    [SerializeField] private Button editItemPrefab;
    [SerializeField] private Button editItemSkeletonPrefab;

    private Camera _camera;
    private List<Button> _ownedBuilding;  // 내가 소유하고 있는 건물
    private KingdomManager _kindomManager;

    public override void Show()
    {
        base.Show();
    }

    public override void Hide()
    {
        base.Hide();

        OnClickEditExitButton();
    }

    public override void Init()
    {
        base.Init();

        _camera = Camera.main;
        _kindomManager = FindObjectOfType<KingdomManager>();

        for (int i = 0; i < _kindomManager.buildingsInKingdom.Count; i++)
            GameManager.Game.EnvironmentScore += _kindomManager.buildingsInKingdom[i].Data.BuildingScore;
        _environmentScoreText.text = GameManager.Game.EnvironmentScore.ToString();

        GameManager.Game.OnChangeDia += (() => _diaText.text = GameManager.Game.Dia.ToString("#,##0"));
        GameManager.Game.OnChangeMoney += (() => _moneyText.text = GameManager.Game.Money.ToString("#,##0"));
        GameManager.Game.UpdateGoods();

        _kindomManager.BuildingCircleEditUI.onInstallBuilding += () => _environmentScoreText.text = GameManager.Game.EnvironmentScore.ToString();
        _kindomManager.BuildingCircleEditUI.onUnInstallBuilding += () => _environmentScoreText.text = GameManager.Game.EnvironmentScore.ToString();
        _kindomManager.BuildingCircleEditUIInPreview.onInstallBuilding += () => _environmentScoreText.text = GameManager.Game.EnvironmentScore.ToString();
        _kindomManager.BuildingCircleEditUIInPreview.onUnInstallBuilding += () => _environmentScoreText.text = GameManager.Game.EnvironmentScore.ToString();

        List<BuildingController> ownedBuilding = _kindomManager.buildingsInInventory;  // 내가 소유한 전체 건물
        List<BuildingController> buildingsInKingdom = _kindomManager.buildingsInKingdom;  // 왕국에 설치된 건물

        _ownedBuilding = new List<Button>();

        for (int i = 0; i < ownedBuilding.Count; i++)
        {
            AddBuilding(ownedBuilding[i]);
        }
        editExitButton.onClick.AddListener(() => OnClickEditExitButton());
    }

    // 인벤토리에 추가
    public void AddBuilding(BuildingController building)
    {
        Button editItemButton = Instantiate(editItemSkeletonPrefab, editItemParent);
        SkeletonGraphic skeletonGraphic = editItemButton.GetComponentInChildren<SkeletonGraphic>();

        _ownedBuilding.Add(editItemButton);

        skeletonGraphic.skeletonDataAsset = building.Data.SkeletonData;
        skeletonGraphic.Initialize(true);
        editItemButton.onClick.AddListener(() => OnClickEditBuildingButton(building, editItemButton));

        editItemParent.sizeDelta = new Vector2(20 + 220 * (_ownedBuilding.Count), editItemParent.sizeDelta.y);
    }


    // 인벤토리에 제거
    public void RemoveBuilding(Button removeButton)
    {
        _ownedBuilding.Remove(removeButton);
        Destroy(removeButton.gameObject);
    }


    private void OnClickEditExitButton()
    {
        editInventory.gameObject.SetActive(true);
        editSelected.gameObject.SetActive(false);
    }

    private void OnClickEditBuildingButton(BuildingController currentBuildingPrefab, Button button)
    {
        // 카메라가 보고 있는 중간에 해당 건물을 생성하고 editUI도 넣어준다.
        // 건물은 투명하게 생김
        currentBuildingPrefab.gameObject.SetActive(true);

        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        Vector2 screenCenter = new Vector2(screenWidth / 2f, screenHeight / 2f);
        Vector3 centerWorldPosition = _camera.ScreenToWorldPoint(new Vector3(screenCenter.x, screenCenter.y, _camera.nearClipPlane));
        centerWorldPosition = new Vector3(centerWorldPosition.x, centerWorldPosition.y, 0f);

        currentBuildingPrefab.transform.position = centerWorldPosition;
        currentBuildingPrefab.transform.SetGridTransform();

        _kindomManager.BuildingCircleEditUIInPreview.SetPrevBuilding(currentBuildingPrefab);
        currentBuildingPrefab.BuildingEditor.OnClickEditMode();
        _kindomManager.BuildingCircleEditUIInPreview.SetBuilding(currentBuildingPrefab, currentBuildingPrefab.transform, _camera.orthographicSize);

        // 타일은 없어지지 않음
        RemoveBuilding(button);
    }
}
