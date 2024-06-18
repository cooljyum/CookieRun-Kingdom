using UnityEngine;

public class BattleMapScrolling : MonoBehaviour
{
    [Header("Map Obj")]
    [SerializeField]
    private GameObject _map1;
    [SerializeField]
    private GameObject _map2;

    [Header("Map Speed")]
    [SerializeField]
    private float _speed = 4.5f;
    [SerializeField]
    private float _speedFactorY = 2.0f; // y�� �̵� �ӵ��� ������

    [Header("Map Size")]
    [SerializeField]
    private float _mapWidth = 100.0f; // _map1�� ����
    [SerializeField]
    private float _mapHeight = 10.0f; // _map1�� ����

    [Header("Map StartPos")]
    [SerializeField]
    private Vector2 startPos1;
    [SerializeField]
    private Vector2 startPos2;

    void Start()
    {
        startPos1 = _map1.transform.position;
        startPos2 = _map2.transform.position;

        // Ÿ�� ũ�� ���
        BoxCollider2D boxCollider = _map1.GetComponent<BoxCollider2D>();
        if (boxCollider != null)
        {
            _mapWidth = boxCollider.size.x;
            _mapHeight = boxCollider.size.y;
        }
    }

    void Update()
    {
        if (BattleManager.Instance.IsOnBattle) return;
        if (BattleManager.Instance.IsStop) return;

        // Ÿ���� �밢������ �̵�
        Vector3 movement = new Vector3(-_speed, -_speed / _speedFactorY, 0) * Time.deltaTime * BattleUIManager.Instance.StageSpeed;
        _map1.transform.position += movement;
        _map2.transform.position += movement;

        // _map1 ȭ�� ������ ����� ��� ��ġ ����
        if (_map1.transform.position.x < startPos1.x - _mapWidth 
            && _map1.transform.position.y < startPos1.y - _mapHeight)
        {
            _map1.transform.position = startPos2;
        }

        // _map2 ȭ�� ������ ����� ��� ��ġ ����
        if (_map2.transform.position.x < startPos1.x - _mapWidth
            && _map2.transform.position.y < startPos1.y - _mapHeight)
        {
            _map2.transform.position = startPos2;
        }
    }
}
