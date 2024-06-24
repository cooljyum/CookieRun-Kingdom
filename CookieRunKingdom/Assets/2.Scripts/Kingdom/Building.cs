using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.U2D.Animation;
using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField]
    private GameObject _installEffect;
    [SerializeField]
    private Transform _craftBubble;
    [SerializeField]
    private GameObject _buildingOverlap;

    private GameObject _bubbleItemPrefab;
    private BuildingData _buildingData;
    public BuildingData BuildingData => _buildingData;
    private CraftItemInfo _craftItemInfo;
    private SkeletonAnimation _skeletonAnimation;
    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _collider;
    private Animator _effectAnimator;
    private Animator _bubbleAnimator;
    private Sprite _bubbleItemImage;
    private PolygonCollider2D _overlapCollider;
    public PolygonCollider2D OverlapCollider => _overlapCollider;
    private SpriteRenderer _overlapRenderer;

    private List<CraftItemInfo> _craftingItems = new List<CraftItemInfo>();
    public List<CraftItemInfo> CraftingItems => _craftingItems;

    private void Awake()
    {
        _bubbleItemPrefab = Resources.Load<GameObject>("Prefabs/Kingdom/Craft/BubbleItem");
        _bubbleItemImage = _bubbleItemPrefab.GetComponent<Sprite>();
        _skeletonAnimation = GetComponentInChildren<SkeletonAnimation>();
        _collider = GetComponent<BoxCollider2D>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _effectAnimator = _installEffect.GetComponent<Animator>();
        _bubbleAnimator = _craftBubble.GetComponent<Animator>();
    }

    private void Start()
    {
        _craftBubble.gameObject.SetActive(false);
        _bubbleAnimator.SetBool("isCrafted", false);
        _overlapCollider = _buildingOverlap.GetComponentInChildren<PolygonCollider2D>();
        _overlapRenderer = _overlapCollider.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if(Input.GetMouseButtonUp(0)) //설치된 건물 클릭
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if(_collider.bounds.Contains(mousePos))
            {
                KingdomManager.Instance.OnClickBuilding(this);
                KingdomManager.Instance.MapGrid.SetActive(false);
            }
        }

        //생산 여부에 따라 애니메이션 변경
        if (_craftingItems.Count > 0)
        {
            if (_skeletonAnimation.AnimationName != "working")
            {
                SetSkeletonAnimation("working");
            }
        }
        else
        {
            if (_skeletonAnimation.AnimationName != "loop_back")
            {
                SetSkeletonAnimation("loop_back");
            }
        }
    }

    public void AddCraftItem(CraftItemInfo item) //생산 시작한 아이템 생산 리스트에 넣기
    {
        _craftingItems.Add(item);
    }

    public void Build(BuildingData buildingData, Vector2 pos) //건물 데이터 세팅 & 설치
    {
        _buildingData = buildingData;

        if(_buildingData.Type != "Decorative")
        {
            _skeletonAnimation.skeletonDataAsset = buildingData.SkeletonDataAsset;
            _skeletonAnimation.AnimationName = "loop_back";
            _skeletonAnimation.Initialize(true);
            _spriteRenderer.gameObject.SetActive(false);
        }
        else
        {
            _spriteRenderer.sprite = buildingData.Sprite;
            _spriteRenderer.gameObject.SetActive(true);
            _skeletonAnimation.gameObject.SetActive(false);
        }

        transform.position = pos;
        GameManager.Instance.SaveBuilding(this); //건물 정보 저장

        gameObject.SetActive(true);
        _installEffect.gameObject.SetActive(true);
        _installEffect.GetComponentInChildren<ParticleSystem>().Play();

        KingdomManager.Instance.SetSelectedBuilding(null); //선택 해제
        KingdomManager.Instance.IsBuildingFixed = false;
    }

    public void SetSkeletonAnimation(string name)
    {
        _skeletonAnimation.AnimationName = name;
        _skeletonAnimation.Initialize(true);
    }

    public void SetOverlapColor(Color color)
    {
        if (_overlapRenderer != null)
        {
            _overlapRenderer.color = color;
        }
    }

    public Bounds GetBounds()
    {
        return _overlapCollider.bounds;
    }

    //public void SetCraftedItemBubble(int key) //*호출해야됨
    //{
    //    //생산 완료된 아이템 리스트 for문
    //    GameObject instantiatedPrefab = Instantiate(_bubbleItemPrefab);
    //    instantiatedPrefab.transform.SetParent(_craftBubble, false);
    //    //_bubbleItemImage = GameManager.Instance.GetItemData(key);
    //}
}
