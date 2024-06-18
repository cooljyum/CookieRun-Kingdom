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
    private float _speedFactorY = 2.0f; // y축 이동 속도를 나누기

    [Header("Map Size")]
    [SerializeField]
    private float _mapWidth = 100.0f; // _map1의 가로
    [SerializeField]
    private float _mapHeight = 10.0f; // _map1의 세로

    [Header("Map StartPos")]
    [SerializeField]
    private Vector2 startPos1;
    [SerializeField]
    private Vector2 startPos2;

    void Start()
    {
        startPos1 = _map1.transform.position;
        startPos2 = _map2.transform.position;

        // 타일 크기 계산
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

        // 타일을 대각선으로 이동
        Vector3 movement = new Vector3(-_speed, -_speed / _speedFactorY, 0) * Time.deltaTime * BattleUIManager.Instance.StageSpeed;
        _map1.transform.position += movement;
        _map2.transform.position += movement;

        // _map1 화면 밖으로 벗어나는 경우 위치 조정
        if (_map1.transform.position.x < startPos1.x - _mapWidth 
            && _map1.transform.position.y < startPos1.y - _mapHeight)
        {
            _map1.transform.position = startPos2;
        }

        // _map2 화면 밖으로 벗어나는 경우 위치 조정
        if (_map2.transform.position.x < startPos1.x - _mapWidth
            && _map2.transform.position.y < startPos1.y - _mapHeight)
        {
            _map2.transform.position = startPos2;
        }
    }
}
