using UnityEngine;
using UnityEngine.UI;

public class HPBarController : MonoBehaviour
{
    [SerializeField]
    private GameObject _target;
    [SerializeField]
    private Scrollbar _hpBar;
    private Camera _mainCamera;
    [SerializeField]
    private Vector3 _hpBarOffset = new Vector3(0, 2, 0);

    void Start()
    {
        _mainCamera = Camera.main;
        _hpBar = GetComponent<Scrollbar>();
    }


    public void SetTarget(GameObject target) 
    {
        _target = target;
    }

    public void SetHP(float hp, float maxHp)
    {
        // HP 값을 Scrollbar에 반영
        _hpBar.size = hp / maxHp;
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
