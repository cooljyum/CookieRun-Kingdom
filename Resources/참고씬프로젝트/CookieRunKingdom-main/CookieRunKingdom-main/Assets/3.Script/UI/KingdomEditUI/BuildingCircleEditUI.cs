using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCircleEditUI : MonoBehaviour
{
    public System.Action onInstallBuilding;
    public System.Action onUnInstallBuilding;

    protected BuildingController _currentBuilding;

    [SerializeField] protected GameObject _buttonsParent;

    [Header("MyButtons")]
    [SerializeField] protected MyButton _exitButton;
    [SerializeField] protected MyButton _storeButton;
    [SerializeField] protected MyButton _checkButton;
    [SerializeField] protected MyButton _sellButton;
    [SerializeField] protected MyButton _rotateButton;
    [SerializeField] protected MyButton _infoButton;

    [Header("UI")]
    [SerializeField] protected BuildingInfoUI _buildingInfoUI;
    [SerializeField] protected KingdomEditUI _kingdomEditUI;
    
    // 다른건 비활성화되어 있는데 얘만 활성화되어 있기 때문에 얘는 그냥 코드로 찾아줌
    protected KingdomManager _kingdomManager;  

    protected Vector3 _originPos;
    protected bool _originFlip;
    protected bool _currentFlip;

    public GameObject ButtonParent => _buttonsParent;

    private void Awake()
    {
        _kingdomManager = FindObjectOfType<KingdomManager>();

        _exitButton.AddListener(ExitUI);
        _storeButton.AddListener(StoreBuilding);
        _checkButton.AddListener(CheckBuilding);
        _sellButton.AddListener(SellBuilding);
        _rotateButton.AddListener(RotateBuilding);
        _infoButton.AddListener(ShowInfo);
    }

    public virtual void SetPrevBuilding(BuildingController currentBuilding)
    {
        // 이전 건물 원상태로 돌리기
        if (_currentBuilding != null && _currentBuilding != currentBuilding)
        {
            _currentBuilding.transform.position = _originPos;
            _currentBuilding.BuildingEditor.PutBuilding();
            _currentBuilding.BuildingAnimator.FlipX(_originFlip);
        }
    }

    public virtual void SetBuilding(BuildingController currentBuilding, Transform parent, float cameraOrthographicSize = 10f)
    {
        _buttonsParent.SetActive(true);

        if (currentBuilding != _currentBuilding)
        {
            _currentBuilding = currentBuilding;
            transform.SetParent(parent);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;

            // 여기서 빌딩 UI가 나오니까 
            // 여기서 초기값 넣어주면 됨
            _originPos = _currentBuilding.transform.position;
            _originFlip = _currentBuilding.BuildingEditor.IsFlip;
            _currentFlip = _originFlip;

            // 키면 내 자리 해제해줘야 해

            if(_currentBuilding.BuildingEditor.IsInstallable() || _currentBuilding.BuildingEditor.IsInstance)
                _currentBuilding.BuildingEditor.UnInstallBuilding();
        }

        transform.localScale = Vector3.one * (cameraOrthographicSize) / 10;
    }

    public virtual void HideUI()
    {
        _buttonsParent.SetActive(false);

        if (_currentBuilding != null)
            ExitUI();
    }


    /// <summary>
    /// exitButton => 변경사항을 취소하는거 => 건물은 원래대로 돌아감 => UI도 없어짐
    /// storeButton => 누르자마자 바로 창고에 들어감
    /// checkButton => 설치할 수 있으면 바로 설치하고 UI없어짐
    /// sellButton => 전용 UI가 또 나와
    /// rotateButton => 그냥 돌기만 해 => 이건 InitialFlipX 하면 될듯
    /// infoButton => 전용 UI가 또 나와
    /// </summary>

    #region 버튼 OnClick 메소드
    public virtual void ShowInfo()
    {
        _buildingInfoUI.SetBuilding(_currentBuilding);
        GameManager.UI.ShowPopUpUI(_buildingInfoUI);
    }

    public virtual void ExitUI()
    {
        // 원래대로 위치시킴
        _currentBuilding.transform.position = _originPos;
        _currentBuilding.BuildingAnimator.FlipX(_originFlip);
        _currentBuilding.BuildingEditor.PutBuilding();

        BuildingPreviewTileObjectPool.instance.ResetPreviewTile();
        _buttonsParent.SetActive(false);

        _currentBuilding = null;
    }

    public virtual void StoreBuilding()
    {
        if(_currentBuilding.BuildingEditor.IsInstance)
        {
            GameManager.Game.EnvironmentScore -= _currentBuilding.Data.BuildingScore;
            onUnInstallBuilding?.Invoke();
        }

        BuildingPreviewTileObjectPool.instance.ResetPreviewTile();
        _buttonsParent.SetActive(false);

        Debug.Log("주머니에 넣기");
        _kingdomEditUI.AddBuilding(_currentBuilding);

        // kingdomManager에 빼고
        // 안에 있는 일하는 쿠키 빼고
        // 안에 제작되고 있던 물건도 다 빼고
        // 안에 있는 UI 빼고
        // 파괴시킨다.
        _currentBuilding.BuildingEditor.IsInstance = false;
        _kingdomManager.buildingsInKingdom.Remove(_currentBuilding);

        if(_currentBuilding.BuildingWorker.IsCraftable)
        {
            if (_currentBuilding.BuildingWorker.Worker != null)
                _currentBuilding.BuildingWorker.Worker.CookieCitizeon.LeaveWork();
            _currentBuilding.BuildingWorker.InitCraftSlot();
        }

        _currentBuilding.BuildingWorker.SaveBuilding();

        transform.SetParent(null);
        _currentBuilding.gameObject.SetActive(false);
    }

    public virtual void RotateBuilding()
    {
        _currentFlip = !_currentFlip;
        _currentBuilding.BuildingAnimator.FlipX(_currentFlip);
    }

    public virtual void SellBuilding()
    {
        Debug.Log("건물을 팔꺼야");
    }

    public virtual void CheckBuilding()
    {
        if (_currentBuilding.BuildingEditor.IsInstallable())
        {
            _originPos = _currentBuilding.transform.position;
            _currentBuilding.BuildingEditor.PutBuilding();

            BuildingPreviewTileObjectPool.instance.ResetPreviewTile();
            _buttonsParent.SetActive(false);

            _currentBuilding.BuildingAnimator.FlipX(_currentFlip);
            _currentBuilding.BuildingEditor.IsFlip = _currentFlip;

            if(_currentBuilding.BuildingWorker.Worker != null)
            {
                _currentBuilding.BuildingWorker.Worker.CharacterAnimator.SettingOrder();
            }

            _currentBuilding.BuildingEditor.OnInstallEffect();
            _currentBuilding = null;
        }
        else
        {
            GuideDisplayer.Instance.ShowGuide("이곳엔 설치할 수 없습니다.");
        }
    }

    #endregion
}
