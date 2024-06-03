using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PathFindingAgent))]
public class CookieBT : BehaviorTree
{
    [SerializeField] private string _greetingAnimation = "call_user";

    private CookieController _controller;
    private PathFindingAgent _agent;
    private KingdomManager _kingdomManager;

    private Coroutine _coBT = null;
    private Coroutine _coHello = null;
    private Transform _originParent = null;
    private bool _isKingdomScene = false;
    private bool _isReadyToBT; // 행동할 준비가 됐다는 변수 이게 true면 활성화될 시 BT시작

    public bool IsWorking { get; private set; }

    private void Init(CookieController controller)
    {
        _isKingdomScene = GameManager.Scene.CurrentScene == ESceneName.Kingdom;

        if (!_isKingdomScene)
            return;

        _controller = controller;
        _agent = GetComponent<PathFindingAgent>();
        _kingdomManager = FindObjectOfType<KingdomManager>();
        _agent.Init(_controller);

        // BT 설정
        SetBT();
    }

    private void OnEnable()
    {
        if (_isReadyToBT)
        {
            _isReadyToBT = false;
            StartKingdomAI();
        }
    }

    private void OnDisable()
    {
        StopKingdomAI();
    }

    // AI시작
    public void StartKingdomAI()
    {
        // 킹덤씬에서만 AI행동
        if (!_isKingdomScene)
            return;

        // 일하고 있는 쿠키라면 AI안함
        if (IsWorking)
            return;

        StopKingdomAI();
        _coBT = StartCoroutine(CoBT());
    }

    // AI종료
    public void StopKingdomAI()
    {
        _agent.StopPathFinding();

        if (_coBT != null)
            StopCoroutine(_coBT);
    }

    // 하던 BT를 멈추고 인사함
    public void Hello()
    {
        if (_coHello != null)
            StopCoroutine(_coHello);
        // 하던 AI행동 그만!
        StopKingdomAI();
        _coHello = StartCoroutine(COHello());
    }

    // 출근하기
    public void GoToWork(Transform parent)
    {
        IsWorking = true;

        // 하고 있던 AI동장 중지
        _agent.StopPathFinding();
        if (_coBT != null)
            StopCoroutine(_coBT);

        // 출석부 등록하기
        _kingdomManager.workingCookies.Add(_controller);

        // 위치 조정하기
        _originParent = transform.parent;
        transform.SetParent(parent);
        transform.localPosition = Vector3.zero;
        _controller.CharacterAnimator.FlipX(false);
        _controller.CharacterAnimator.SettingOrder();

        // 인사하기
        _controller.CharacterAnimator.PlayAnimation(_greetingAnimation);
        _controller.CharacterAnimator.SettingOrderLayer(true);
    }

    // 일을 다했다면 인사함
    public void EndWork()
    {
        _controller.CharacterAnimator.PlayAnimation(_greetingAnimation);
    }

    // 퇴근하기
    public void LeaveWork()
    {
        IsWorking = false;

        // 출석부에서 빼기
        _kingdomManager.workingCookies.Remove(_controller);

        // 위치 조정하기
        transform.SetParent(_originParent);
        transform.position = GridManager.Instance.ReturnEmptyTilePosition();
        _controller.CharacterAnimator.SettingOrderLayer(false);

        if(!gameObject.activeSelf)
        {
            _isReadyToBT = true;
            return;
        }

        StartKingdomAI();
    }

    // 땅에 내려놓았을 때 유효하지 않은 위치라면 가장 가까운 유효한 위치로 순간이동시킴
    public void TeleportValildPosition(Vector3 originPos)
    {
        // 지금 플레이어의 그리드 위치
        Vector3Int playerGridPos = GridManager.Instance.Grid.WorldToCell(transform.position);

        if (!GridManager.Instance.ValidTileCheck(playerGridPos.x, playerGridPos.y))
        {
            GuideDisplayer.Instance.ShowGuide("쿠키를 내려놓을 수 없는 곳입니다.");

            // 그리드의 범위 밖이라면 이전 위치로 이동
            if (playerGridPos.y >= 100 || playerGridPos.y <= -100 || playerGridPos.x >= 100 || playerGridPos.x <= -100)
            {
                transform.position = originPos;
            }
            // 아니면 계산한 가장 가까운 유효한 위치로 이동
            else
            {
                Collider2D[] tilesCol = Physics2D.OverlapCircleAll(transform.position, 10f);
                List<Transform> tiles = new List<Transform>();

                for (int i = 0; i < tilesCol.Length; i++)
                {
                    if (tilesCol[i].gameObject.layer != LayerMask.NameToLayer("Ground"))
                        continue;
                    Vector3Int tilesGridPos = GridManager.Instance.Grid.WorldToCell(tilesCol[i].transform.position);
                    if (!GridManager.Instance.ValidTileCheck(tilesGridPos.x, tilesGridPos.y))
                        continue;

                    tiles.Add(tilesCol[i].transform);
                }

                if (tiles.Count == 0)
                {
                    transform.position = originPos;
                    return;
                }

                float distance = int.MaxValue;
                Vector3 minTile = Vector3.zero;

                for (int i = 0; i < tiles.Count; i++)
                {
                    // float distanceTile = Mathf.Sqrt(Mathf.Pow(transform.position.x - tiles[i].position.x, 2) + Mathf.Pow(transform.position.y - tiles[i].position.y, 2));
                    float distanceTile = Mathf.Pow(transform.position.x - tiles[i].position.x, 2) + Mathf.Pow(transform.position.y - tiles[i].position.y, 2);
                    if (distance > distanceTile)
                    {
                        distance = distanceTile;
                        minTile = tiles[i].transform.position;
                    }
                }
                transform.position = minTile;
            }
        }
    }

    private void SetBT()
    {
        ResetRootNode();

        BTNodeBase walkRoot = RootNode.Add<BTNodeAction>("돌아다니기",
            () =>
            {
                // OnEnter
                // 경로를 구하고 이동하기 시작한다.
                Debug.Log("시작하자");
                return ENodeStatus.InProgress;
            },
            () =>
            {
                // OnTick
                // 목적지까지 다왔다면 이게 실행
                Debug.Log("다왔는지 검사!");
                return ENodeStatus.Succeeded;
            });
    }

    private void MoveRandomPosition()
    {
        // 유효한 위치를 찾을 때까지 반복
        while(true)
        {
            Vector3 targetPosition = Random.insideUnitCircle * 5;
            Vector3Int cellTargetPosition = GridManager.Instance.Grid.WorldToCell(transform.position + targetPosition);
            if(GridManager.Instance.ValidTileCheck(cellTargetPosition.x, cellTargetPosition.y))
            {
                _agent.MoveTo(GridManager.Instance.Grid.CellToWorld(cellTargetPosition));
                break;
            }
        }
    }

    private IEnumerator CoBT()
    {
        while(true)
        {
            yield return null;
            Tick();
        }
    }

    private IEnumerator COHello()
    {
        _controller.CharacterAnimator.PlayAnimation("idle_back", false);
        _controller.CharacterAnimator.PlayAnimation(_greetingAnimation, false);

        yield return new WaitUntil(() => !_controller.CharacterAnimator.IsPlayingAnimation());
        StartKingdomAI();
    }
}
