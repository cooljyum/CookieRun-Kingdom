using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class KingdomManager : MonoBehaviour
{
    private KingdomStateFactory _factory;

    [Header("UI")]
    [SerializeField] private KingdomManageUI _kingdomManagerUI;
    [SerializeField] private KingdomEditUI _kingdomEditUI;
    [SerializeField] private GameObject _backGroundUI;
    [SerializeField] private KingdomCraftUI _kingdomCraftUI;
    [SerializeField] private KingdomPlayUI _kingdomPlayUI;
    [SerializeField] private KingdomAdventureUI _kingdomAdventureUI;

    [Header("트랜스폼")]
    [SerializeField] private Transform _kingdomGrid;

    [Header("컴포넌트")]
    [SerializeField] private BuildingCircleEditUI _buildingCircleEditUI;
    [SerializeField] private BuildingCircleEditUIInPreview _buildingCircleEditUIInPreview;
    [SerializeField] private Grid _grid;

    [Header("Data")]
    [SerializeField] private CameraControllData _cameraControllInKingdomData;
    [SerializeField] private CameraControllData _cameraControllInStageData;

    // 프로퍼티
    public KingdomManageUI KingdomManagerUI => _kingdomManagerUI;
    public KingdomEditUI KingdomEditUI => _kingdomEditUI;
    public GameObject KingdomBackGroundUI => _backGroundUI;
    public KingdomCraftUI KingdomCraftUI => _kingdomCraftUI;
    public KingdomPlayUI KingdomPlayUI => _kingdomPlayUI;

    public KingdomAdventureUI KingdomAdventureUI => _kingdomAdventureUI;

    public Transform KingdomGrid => _kingdomGrid;

    public BuildingCircleEditUI BuildingCircleEditUI => _buildingCircleEditUI;
    public BuildingCircleEditUIInPreview BuildingCircleEditUIInPreview => _buildingCircleEditUIInPreview;
    public Grid Grid => _grid;
    public CameraControllData CameraControllInKingdomData => _cameraControllInKingdomData;
    public CameraControllData CameraContrllInStageData => _cameraControllInStageData;

    public CameraControllData CurrentCameraControllerData { get; set; }

    public bool IsMoveCamera { get; set; } = true;

    // 내가 소유한 실질적인 건물
    public List<BuildingController> buildingsInInventory = new List<BuildingController>();  
    
    // 내 왕국에 있는 실질적인 건물(설치되어있는 건물이란 말이죠)
    public List<BuildingController> buildingsInKingdom = new List<BuildingController>();
    
    public List<CookieController> allCookies = new List<CookieController>();
    public List<CookieController> workingCookies = new List<CookieController>();

    public void Init()
    {
        _factory = new KingdomStateFactory(this);
        Debug.Log(GameManager.Game.StartKingdomState);
        ChangeState(GameManager.Game.StartKingdomState);
        StartCoroutine(CoUpdate());
    }


    #region InputAction에 넣을 메소드들!!
    public void OnWheel(InputAction.CallbackContext value)
    {
        if (!IsMoveCamera)
            return;

        if (_factory == null)
            return;

        _factory.CurrentKingdomState.OnWheel(value);
    }

    public void OnTouch(InputAction.CallbackContext value)
    {
        if(value.started)
        {
            Debug.Log("누르기 시작");
        }
        else if(value.canceled)
        {
            Debug.Log("누르기 끝");
        }

        if (!IsMoveCamera)
            return;
        if (_factory == null)
            return;

        // Utils.touchTIme을 잘 사용하도록 해..

        if (value.started)
        {
            OnClickStart();
            if (_coTouch != null)
                StopCoroutine(_coTouch);
            _coTouch = StartCoroutine(CoTouch());
        }
        if (value.canceled)
        {
            StopCoroutine(_coTouch);

            if (_isTouch)
                OnClick();
            else
                OnDragEnd();
        }
    }

    private Coroutine _coTouch;
    private bool _isTouch;

    private IEnumerator CoTouch()
    {
        // Utils.TouchTime이 지나면
        float currentTime = 0;
        _isTouch = true;

        while (true)
        {
            currentTime += Time.deltaTime;

            if(currentTime >= Utils.TouchTime)
            {
                _isTouch = false;
                OnDrag();
            }
            yield return null;
        }
    }
    
    public void OnClickStart()
    {
        if (!IsMoveCamera) return;
        if (_factory == null) return;

        _factory.CurrentKingdomState.OnClickStart();
    }

    public void OnClick()
    {
        if (!IsMoveCamera) return;
        if (_factory == null) return;

        _factory.CurrentKingdomState.OnClick();
    }

    public void OnDrag()
    {
        if (!IsMoveCamera) return;
        if (_factory == null) return;

        _factory.CurrentKingdomState.OnDrag();
    }

    public void OnDragEnd()
    {
        if (!IsMoveCamera) return;
        if (_factory == null) return;

        _factory.CurrentKingdomState.OnDragEnd();
    }
    #endregion


    #region 버튼에 넣을 메소드들!!
    public void ChangeState(int index)
    {
        _factory.ChangeState((EKingdomState)index);
    }

    public void ChangeState(EKingdomState state)
    {
        _factory.ChangeState(state);
    }
    #endregion

    private IEnumerator CoUpdate()
    {
        while (true)
        {
            yield return null;
            _factory.CurrentKingdomState.Update();
        }
    }
}
