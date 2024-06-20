using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class HPBarController : MonoBehaviour
{
    [SerializeField]
    private GameObject _target;
    [SerializeField]
    private Slider _hpBar;
    private Camera _mainCamera;
    [SerializeField]
    private Vector3 _hpBarOffset = new Vector3(0, 2, 0);

    private Image _fillImage;

    void Awake()
    {
        _mainCamera = Camera.main;
        _hpBar = GetComponent<Slider>();

        _fillImage = _hpBar.fillRect.GetComponent<Image>();
    }

    private void Start()
    {
        gameObject.SetActive(true);
    }

    public void SetTarget(GameObject target,bool isEnemy) 
    {
        _target = target;

        if (isEnemy)
            _fillImage.sprite = Resources.Load<Sprite>("Textures/UI/Battle/HPBarEnemyFront");
    }

    public void SetHP(float hp, float maxHp)
    {
        //if (_hpBar.value < 1) gameObject.SetActive(true); 

        // HP 값을 Slider에 반영
        _hpBar.value = hp / maxHp;
    }

    private void Update()
    {
        if (_target == null) return;

        if (_target.activeSelf != false)
        {
            UpdatePosition(_target.transform.position, _hpBarOffset);
        }
        else 
        {
            gameObject.SetActive(false);
        }
    }

    public void UpdatePosition(Vector3 worldPos, Vector3 offset)
    {
        // HP 바 위치 업데이트
        Vector3 screenPos = _mainCamera.WorldToScreenPoint(worldPos + offset);
        transform.position = screenPos;
    }

}
