using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ECookieCitizenState
{
    idle, working, greeting
}

[RequireComponent(typeof(PathFindingAgent))]
public class CookieCitizen : BehaviorTree
{
    public class BlackBoardKey : BlackboardKeyBase
    {
        public static readonly BlackBoardKey GreetingCookies = new BlackBoardKey() { Name = "GreetingCookies" };
        public static readonly BlackBoardKey GreetingTargetCookie = new BlackBoardKey() { Name = "GreetingTargetCookie" };
        public string Name;
    }

    private Blackboard<BlackBoardKey> _localMemory;


    [SerializeField] private string _callUserAnimation = "call_user";
    [SerializeField] private string _touchAnimation = "touch";
    [SerializeField] private string _personalAnimation = "personal1";

    [SerializeField] private string _greetingAnimation = "greeting";
    [SerializeField] private string _greetingBackAnimation = "greeting_back";

    [SerializeField] private string _talkAnimation = "talk";
    [SerializeField] private string _talkBackAnimation = "talk_back";

    private CookieController _controller;
    private PathFindingAgent _agent;
    private KingdomManager _kingdomManager;

    private Coroutine _coBT = null;
    private Coroutine _coHello = null;
    private Transform _originParent = null;
    private bool _isKingdomScene = false;
    private bool _isReadyToBT; // 행동할 준비가 됐다는 변수 이게 true면 활성화될 시 BT시작


    public ECookieCitizenState CookieState { get; private set; }


    public void Init(CookieController controller)
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
        if (!_isKingdomScene)
            return;

        if (_isReadyToBT)
        {
            _isReadyToBT = false;
            StartKingdomAI();
        }
    }

    private void OnDisable()
    {
        if (!_isKingdomScene)
            return;

        StopKingdomAI();
    }

    // AI시작
    public void StartKingdomAI()
    {
        // 킹덤씬에서만 AI행동
        if (!_isKingdomScene)
            return;

        // 일하고 있는 쿠키라면 AI안함
        if (CookieState == ECookieCitizenState.working)
            return;

        CookieState = ECookieCitizenState.idle;

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
        CookieState = ECookieCitizenState.working;

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
        _controller.CharacterAnimator.PlayAnimation(_callUserAnimation);
        _controller.CharacterAnimator.SettingOrderLayer(true);
    }

    // 일을 다했다면 인사함
    public void EndWork()
    {
        _controller.CharacterAnimator.PlayAnimation(_callUserAnimation);
    }

    // 퇴근하기
    public void LeaveWork()
    {
        CookieState = ECookieCitizenState.idle;

        // 출석부에서 빼기
        _kingdomManager.workingCookies.Remove(_controller);

        // 위치 조정하기
        transform.SetParent(_originParent);
        transform.position = GridManager.Instance.ReturnEmptyTilePosition();
        _controller.CharacterAnimator.SettingOrderLayer(false);

        if (!gameObject.activeSelf)
        {
            _isReadyToBT = true;
            return;
        }

        StartKingdomAI();
    }

    // 땅에 내려놓았을 때 유효하지 않은 위치라면 가장 가까운 유효한 위치로 순간이동시킴
    public void TeleportValidPosition(Vector3 originPos)
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
        _localMemory = BlackboardManager.Instance.GetIndividualBlackboard<BlackBoardKey>(this);
        _localMemory.SetGeneric<string>(BlackBoardKey.GreetingCookies, "");
        _localMemory.SetGeneric<CookieController>(BlackBoardKey.GreetingTargetCookie, null);

        BTNodeBase BTRoot = RootNode.Add<BTNodeSelector>("BT START");
        BTRoot.AddService<BTServiceBase>("근처에 쿠키가 있나요?", (float deltaTime) =>
        {
            _localMemory.SetGeneric<CookieController>(BlackBoardKey.GreetingTargetCookie, null);
            Collider2D[] cookieCollider = Physics2D.OverlapCircleAll(transform.position, 1.5f, 1 << LayerMask.NameToLayer("Cookie"));

            if (cookieCollider.Length == 0)
                return;

            List<CookieController> cookieList = new List<CookieController>();

            for(int i = 0; i < cookieCollider.Length; i++)
            {
                if(cookieCollider[i].transform != transform)
                {
                    CookieController cookie = cookieCollider[i].GetComponent<CookieController>();
                    if (cookie != null)
                        cookieList.Add(cookie);
                }
            }

            if (cookieList.Count == 0)
                return;

            _localMemory.SetGeneric<CookieController>(BlackBoardKey.GreetingTargetCookie, cookieList[0]);
        });


        BTNodeBase walkRoot = BTRoot.Add<BTNodeSequence>("움직임 시도");

        walkRoot.AddDecorator<BTDecoratorBase>("움직임 시도 체크", () =>
        {
            CookieController otherCookie = _localMemory.GetGeneric<CookieController>(BlackBoardKey.GreetingTargetCookie);
            if (otherCookie == null)
                return true;
            if (CheckGreeting(otherCookie))
                return true;
            return false;
        });

        BTNodeBase cookieWalk = walkRoot.Add<BTNodeAction>("걷자!",
            () =>
            {
                if (Random.Range(0, 2) == 0)
                    return ENodeStatus.Failed;

                if (MoveRandomPosition())
                    return ENodeStatus.Succeeded;
                else
                    return ENodeStatus.Failed;
            },
            () =>
            {
                if (_agent.IsDestination)
                    return ENodeStatus.Succeeded;
                else
                {
                    if (_localMemory.GetGeneric<CookieController>(BlackBoardKey.GreetingTargetCookie) != null)
                        return ENodeStatus.Failed;
                    return ENodeStatus.InProgress;
                }
            });

        BTNodeBase cookiePersonal = walkRoot.Add<BTNodeAction>("개인행동 노드", () =>
        {
                _controller.CharacterAnimator.PlayAnimation(_personalAnimation, false);
                return ENodeStatus.Succeeded;
        },
        () =>
        {
            if (!_controller.CharacterAnimator.IsPlayingAnimation())
                return ENodeStatus.Succeeded;
            else
            {
                if (_localMemory.GetGeneric<CookieController>(BlackBoardKey.GreetingTargetCookie) != null)
                    return ENodeStatus.Failed;
                return ENodeStatus.InProgress;
            }
        });


        BTNodeBase greetingRoot = BTRoot.Add<BTNodeSequence>("인사 시도");

        greetingRoot.AddDecorator<BTDecoratorBase>("인사 시도 체크", () =>
        {
            CookieController otherCookie = _localMemory.GetGeneric<CookieController>(BlackBoardKey.GreetingTargetCookie);
            if (otherCookie != null && otherCookie.CookieCitizeon.CookieState == ECookieCitizenState.idle && !CheckGreeting(otherCookie))
            {
                return true;
            }
            return false;
        });

        BTNodeBase cookieGreet = greetingRoot.Add<BTNodeAction>("인사한다.",
            () =>
            {
                // BT를 멈추고 서로 인사하는 시간을 가진다.
                CookieController otherCookie = _localMemory.GetGeneric<CookieController>(BlackBoardKey.GreetingTargetCookie);
                StartCoroutine(CoGreeting(otherCookie));
                return ENodeStatus.Succeeded;
            },
            () =>
            {
                return ENodeStatus.Succeeded;
            });
        // BTNodeBase cookieCommunication = greetingRoot.Add<BTNodeAction>("얘기한다.",)


        BTNodeBase cookieIdle = BTRoot.Add<BTNodeAction>("가만히 있는다.", () =>
            {
                _agent.StopPathFinding();
                _controller.CharacterAnimator.PlayAnimation("idle", false);
                return ENodeStatus.Succeeded;
            },
            () =>
            {
                CookieController otherCookie = _localMemory.GetGeneric<CookieController>(BlackBoardKey.GreetingTargetCookie);
                if (otherCookie != null && !CheckGreeting(otherCookie))
                    return ENodeStatus.Succeeded;

                if (!_controller.CharacterAnimator.IsPlayingAnimation())
                    return ENodeStatus.Succeeded;
                return ENodeStatus.InProgress;
            });
            
    }


    private bool MoveRandomPosition()
    {
        Vector3 targetPosition = Random.insideUnitCircle * 5;
        Vector3Int cellTargetPosition = GridManager.Instance.Grid.WorldToCell(transform.position + targetPosition);
        if (GridManager.Instance.ValidTileCheck(cellTargetPosition.x, cellTargetPosition.y))
        {
            _agent.MoveTo(GridManager.Instance.Grid.CellToWorld(cellTargetPosition));
            return true;
        }
        else
        {
            return false;
        }
    }

    public IEnumerator CoGreeting(CookieController otherCookie)
    {
        otherCookie.CookieCitizeon.CookieState = ECookieCitizenState.greeting;
        CookieState = ECookieCitizenState.greeting;

        Greeting(otherCookie);
        otherCookie.CookieCitizeon.Greeting(_controller);

        yield return new WaitUntil(() => !_controller.CharacterAnimator.IsPlayingAnimation());

        StartKingdomAI();
        otherCookie.CookieCitizeon.StartKingdomAI();
    }

    // 인사 상호작용
    public void Greeting(CookieController otherCookie)
    {
        StopKingdomAI();
        _controller.CharacterAnimator.PlayAnimation("idle_back", false);

        // 인사를 하면 인사를 했던 쿠키 목록에 넣어준다.
        string greetedCookies = _localMemory.GetGeneric<string>(BlackBoardKey.GreetingCookies);
        int cookieIndex = ((CookieData)otherCookie.Data).CookieIndex;
        greetedCookies += cookieIndex.ToString();
        _localMemory.SetGeneric<string>(BlackBoardKey.GreetingCookies, greetedCookies);

        // 내가 위에 있으면 
        if (otherCookie.transform.position.y < transform.position.y)
            _controller.CharacterAnimator.PlayAnimation(_greetingAnimation, false);
        else
            _controller.CharacterAnimator.PlayAnimation(_greetingBackAnimation, false);

        // 내가 오른쪽에 있으면
        if (otherCookie.transform.position.x < transform.position.x)
            _controller.CharacterAnimator.FlipX(true);
        else
            _controller.CharacterAnimator.FlipX(false);
    }

    // otherCookie와 인사를 했는지 체크
    private bool CheckGreeting(CookieController otherCookie)
    {
        string greetedCookies = _localMemory.GetGeneric<string>(BlackBoardKey.GreetingCookies);
        int cookieIndex = ((CookieData)otherCookie.Data).CookieIndex;

        if (greetedCookies.Contains(cookieIndex.ToString()))
            return true;
        return false;
    }

    private IEnumerator CoBT()
    {
        ResetRootNode();
        Tick();
        while (true)
        {
            Tick(0.2f);
            yield return new WaitForSeconds(0.2f);
        }
    }

    private IEnumerator COHello()
    {
        _controller.CharacterAnimator.PlayAnimation("idle_back", false);
        _controller.CharacterAnimator.PlayAnimation(_touchAnimation, false);

        yield return new WaitUntil(() => !_controller.CharacterAnimator.IsPlayingAnimation());
        StartKingdomAI();
    }
}
