using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingWorker : MonoBehaviour
{
    protected BuildingController _controller;

    [SerializeField] private List<CraftingItemData> _craftingItemData = new List<CraftingItemData>();
    [SerializeField] private GameObject _craftBubble;

    [Header("일할 쿠키 정보")]
    [SerializeField] private Transform _workPlace;
    [SerializeField] private string _workAnimationName;

    // 해당 건물이 작업중인가, 아닌가?
    public bool isWorking = false;

    // 해당 건물이 대표 건물인가? 대표건물이면 책임지고 GameManagerEX의 시간을 현재시간으로 업데이트해준다.
    public bool isRepresentative = false;


    private int _slotCount = 0;

    private List<SpriteRenderer> _craftBubbleUnit = new List<SpriteRenderer>();
    private BuildingData _data;
    private KingdomManager _kingdomManager;

    public bool IsCraftable { get; protected set; }
    public CookieController Worker { get; private set; }
    public List<CraftingItemData> CraftingItemData => _craftingItemData;

    public virtual void Init(BuildingController controller)
    {
        _controller = controller;
        _data = _controller.Data;

        for(int i = 0; i < _craftBubble.transform.childCount; i++)
        {
            _craftBubbleUnit.Add(_craftBubble.transform.GetChild(i).GetComponent<SpriteRenderer>());
        }

        _kingdomManager = FindObjectOfType<KingdomManager>();
        _controller.BuildingAnimator.PlayAnimation("off");
    }

    // 건물의 제작슬롯을 초기화해줌
    public void InitCraftSlot()
    {
        _craftingItemData = new List<CraftingItemData>();
        for (int i = 0; i < _slotCount; i++)
            _craftingItemData.Add(new CraftingItemData(ECraftingState.empty, null));
    }


    public virtual void LoadBuilding()
    {
        BuildingInfo buildingInfo = GameManager.Game.OwnedCraftableBuildings[_controller.Data.BuildingIndex];

        _slotCount = buildingInfo.slotCount;
        InitCraftSlot();
        IsCraftable = buildingInfo.isCraftable;
        _craftingItemData = buildingInfo.craftingItemData;

        if (buildingInfo.isInstall)
        {
            _controller.BuildingEditor.IsFlip = buildingInfo.isFlip;
            _controller.BuildingAnimator.FlipX(_controller.BuildingEditor.IsFlip);

            transform.position = buildingInfo.installationPosition;
            transform.SetGridTransform();

            WorkBuilding();
            _controller.BuildingEditor.IsInstance = true;
            _controller.BuildingEditor.OnClickEditMode();
            _controller.BuildingEditor.PutBuilding();
            BuildingPreviewTileObjectPool.instance.ResetPreviewTile();
        }
    }

    public virtual void SaveBuilding()
    {
        BuildingInfo buildingInfo = GameManager.Game.OwnedCraftableBuildings[_controller.Data.BuildingIndex];

        buildingInfo.isCraftable = IsCraftable;
        buildingInfo.installationPosition = transform.position;
        buildingInfo.isInstall = _controller.BuildingEditor.IsInstance;
        buildingInfo.isFlip = _controller.BuildingEditor.IsFlip;

        buildingInfo.craftingItemData = _craftingItemData;

        if (Worker != null)
            buildingInfo.cookieWorkerIndex = ((CookieData)Worker.Data).CookieIndex;
        else
            buildingInfo.cookieWorkerIndex = -1;
    }

    // 건물의 정보를 최신화해줌
    public void LoadWorker(CookieController worker)
    {
        Worker = worker;

        if(Worker != null)
        {
            Worker.CookieCitizeon.GoToWork(_workPlace);
            Worker.CharacterAnimator.SettingOrderLayer(false);
        }

        // 일하는 중이 아님
        if (_craftingItemData[0].state == ECraftingState.empty)
        {
            _controller.BuildingAnimator.PlayAnimation("off");
        }
        else
        {
            if (!_controller.BuildingAnimator.IsAnimationExist("working"))
                _controller.BuildingAnimator.PlayAnimation("loop_back");
            else
                _controller.BuildingAnimator.PlayAnimation("working");

            if(Worker != null)
                Worker.CharacterAnimator.PlayAnimation(_workAnimationName);
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public void WorkBuilding()
    {
        StartCoroutine(CoSecond());
    }

    public void SelectedWorker()
    {
        if (Worker != null)
        {
            Worker.CharacterAnimator.PlayAnimation(_workAnimationName);
        }
    }

    public void Highlight(bool flag)
    {
        // 아무나 쿠키 위치 시키고
        // 쿠키 등장 애니메이션 넣어주기
        if (Worker == null)
        {
            List<CookieController> cookies = _kingdomManager.allCookies;
            foreach(CookieController cookie in cookies)
            {
                if (!cookie.CookieStat.IsHave)
                    continue;
                if (!_kingdomManager.workingCookies.Contains(cookie))
                    Worker = cookie;
            }

            if (Worker != null)
                Worker.CookieCitizeon.GoToWork(_workPlace);
        }

        _controller.BuildingAnimator.SettingOrderHigher(flag);

        if (Worker != null)
            Worker.CharacterAnimator.SettingOrderLayer(flag);
    }

    public void ChangeWorker(CookieController newWorker)
    {
        if(Worker != null)
            Worker.CookieCitizeon.LeaveWork();
        Worker = newWorker;
        Worker.CookieCitizeon.GoToWork(_workPlace);

        if (isWorking)
            Worker.CharacterAnimator.PlayAnimation(_workAnimationName);
    }

    /// <summary>
    /// 제작물 수확을 시도하는 메소드
    /// 수확할 수 있으면 수확하고 true반환
    /// 수확할 수 없으면 그냥 false 반환
    /// </summary>
    /// <returns>수확할 수 있나?</returns>
    public virtual bool TryHarvest()
    {
        for (int i = 0; i < _craftingItemData.Count; i++)
        {
            if (_craftingItemData[i].state == ECraftingState.complete)
            {
                Harvest();
                return true;
            }
        }
        return false;
    }

    // 실질적으로 수확하는 메소드
    protected virtual void Harvest()
    {
        // 버블에 떠다니는 수확물 다 없애기
        _craftBubble.SetActive(false);
        foreach (SpriteRenderer sr in _craftBubbleUnit)
            sr.sprite = null;

        int count = 0;

        for (int i = 0; i < _craftingItemData.Count; i++)
        {
            if (_craftingItemData[i].state == ECraftingState.complete)
            {
                DataBaseManager.Instance.AddItem(_craftingItemData[i].craftData.CraftResult.ingredientItem, _craftingItemData[i].craftData.CraftResult.count);
                _craftingItemData[i].state = ECraftingState.empty;
            }
        }

        for (int i = 0; i < _craftingItemData.Count; i++)
        {
            if (_craftingItemData[i].state != ECraftingState.empty)
            {
                _craftingItemData[count++] = new CraftingItemData(_craftingItemData[i].state, _craftingItemData[i].craftData);
                _craftingItemData[i].state = ECraftingState.empty;
            }
        }

        // 전부 다 수확하면 애니메이션 꺼주기
        if(_craftingItemData[0].state == ECraftingState.empty)
        {
            isWorking = false;
            Worker.CookieCitizeon.EndWork();
        }
    }

    private IEnumerator CoSecond()
    {
        WaitForSecondsRealtime second = new WaitForSecondsRealtime(1f);

        while (true)
        {
            yield return second;

            // 대표건물이 PrevcraftTime시간을 측정한다. 
            if (isRepresentative)
            {
                GameManager.Game.PrevCraftTime = System.DateTime.Now;
            }

            ManageCraftingItemData();
        }
    }

    private void ManageCraftingItemData()
    {
        for (int i = 0; i < _craftingItemData.Count; i++)
        {
            if (_craftingItemData[i].craftData == null)
                return;

            // 만들고 있다면
            if (_craftingItemData[i].state == ECraftingState.making)
            {
                // 1초씩 더해준다.
                _craftingItemData[i].takingTime += 1;

                // 만약 다 만들었다면
                if (_craftingItemData[i].takingTime >= _craftingItemData[i].craftData.CraftTime)
                {
                    // 버블
                    _craftBubble.SetActive(true);
                    foreach (SpriteRenderer sr in _craftBubbleUnit)
                    {
                        if (sr.sprite == null)
                        {
                            sr.sprite = _craftingItemData[i].craftData.CraftImage;
                            break;
                        }
                    }

                    _craftingItemData[i].state = ECraftingState.complete;
                    if (i + 1 < _craftingItemData.Count && _craftingItemData[i + 1].craftData != null &&
                        _craftingItemData[i + 1].state == ECraftingState.waiting)
                    {
                        _craftingItemData[i + 1].state = ECraftingState.making;
                        return;
                    }
                }
            }
        }
    }


    /// <summary>
    /// 다른 상태나 씬에서 KingdomManageState로 돌아올 때 GameManagerEx의 PrevCraftTime과 현재 시간을 계산해서
    /// 제작품의 정보를 최신화한다.
    /// </summary>
    public void UpdateCraftingItem()
    {
        int diffTime = (int)((System.DateTime.Now - GameManager.Game.PrevCraftTime).TotalSeconds);

        for (int i = 0; i < _craftingItemData.Count; i++)
        {
            if (_craftingItemData[i].craftData == null)
                return;

            // 만들고 있다면
            if (_craftingItemData[i].state == ECraftingState.making)
            {
                int remainTime = _craftingItemData[i].craftData.CraftTime - _craftingItemData[i].takingTime;

                // 시간의 차이가 제작품의 남은 시간보다 많으면 만든걸로 한다.
                if(diffTime >= remainTime)
                {
                    diffTime -= remainTime;
                    _craftingItemData[i].state = ECraftingState.complete;

                    // 버블
                    _craftBubble.SetActive(true);
                    foreach (SpriteRenderer sr in _craftBubbleUnit)
                    {
                        if (sr.sprite == null)
                        {
                            sr.sprite = _craftingItemData[i].craftData.CraftImage;
                            break;
                        }
                    }

                    if (i + 1 < _craftingItemData.Count && _craftingItemData[i + 1].craftData != null &&
                        _craftingItemData[i + 1].state == ECraftingState.waiting)
                    {
                        _craftingItemData[i + 1].state = ECraftingState.making;
                    }
                }
                else
                {
                    _craftingItemData[i].takingTime += diffTime;
                }
            }
        }
    }
}
