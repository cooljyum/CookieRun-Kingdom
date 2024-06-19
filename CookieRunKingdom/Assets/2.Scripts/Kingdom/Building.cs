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

    private List<CraftItemInfo> _craftingItems = new();
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
    }

    private void Update()
    {
        if(Input.GetMouseButtonUp(0)) //��ġ�� �ǹ� Ŭ��
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if(_collider.bounds.Contains(mousePos))
            {
                KingdomManager.Instance.OnClickBuilding(this);
                KingdomManager.Instance.MapGrid.SetActive(false);
            }
        }

        //���� ���ο� ���� �ִϸ��̼� ����
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

    public void AddCraftItem(CraftItemInfo item) //���� ������ ������ ���� ����Ʈ�� �ֱ�
    {
        _craftingItems.Add(item);
    }

    public void Build(BuildingData buildingData, Vector2 pos) //�ǹ� ������ ���� & ��ġ
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
        gameObject.SetActive(true);
        _installEffect.gameObject.SetActive(true);
        _installEffect.GetComponentInChildren<ParticleSystem>().Play();
    }

    public void SetSkeletonAnimation(string name)
    {
        _skeletonAnimation.AnimationName = name;
        _skeletonAnimation.Initialize(true);
    }

    public void SetCraftedItemBubble(int key) //*ȣ���ؾߵ�
    {
        //���� �Ϸ�� ������ ����Ʈ for��
        GameObject instantiatedPrefab = Instantiate(_bubbleItemPrefab);
        instantiatedPrefab.transform.SetParent(_craftBubble, false);
        //_bubbleItemImage = GameManager.Instance.GetItemData(key);
    }
}
