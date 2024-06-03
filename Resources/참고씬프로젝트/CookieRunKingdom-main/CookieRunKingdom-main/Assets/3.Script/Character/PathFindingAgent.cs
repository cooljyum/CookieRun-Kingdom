using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node
{
    public Node(bool _isWall, int _x, int _y) { isWall = _isWall; x = _x; y = _y; }

    public bool isWall;
    public Node ParentNode;

    // G : 시작으로부터 이동했던 거리, H : |가로|+|세로| 장애물 무시하여 목표까지의 거리, F : G + H
    public int x, y, G, H;
    public int F { get { return G + H; } }
}


public class PathFindingAgent : MonoBehaviour
{
    private CookieController _controller;

    [Header("Test")]
    [SerializeField] private List<Node> FinalNodeList;

    [Header("이동 관련")]
    [SerializeField] private float _moveSpeed = 1f;

    private Vector2Int _start, _end;
    private Node[,] _nodeArray;
    private Node _startNode, _targetNode, _curNode;
    private List<Node> _closedList;
    private PriorityQueue<Node> _openPq;
    private Coroutine _coMove = null;

    // 목적지를 정했는가?
    public bool SetDestination { get; private set; }

    // 도착했는가?
    public bool IsDestination { get; private set; }


    private Grid _grid => GridManager.Instance.Grid;                      
    private Tilemap[,] _map => GridManager.Instance.buildingGridData.gridData;

    public void Init(CookieController controller)
    {
        _controller = controller;

        // 맵 읽기
        Vector3Int start1 = _grid.WorldToCell(Utils.MapStartPoint);
        Vector3Int end1 = _grid.WorldToCell(Utils.MapEndPoint);

        int minX = start1.x > end1.x ? end1.x : start1.x;
        int maxX = start1.x > end1.x ? start1.x : end1.x;
        int minY = start1.y > end1.y ? end1.y : start1.y;
        int maxY = start1.y > end1.y ? start1.y : end1.y;

        _start = new Vector2Int(minX, minY);
        _end = new Vector2Int(maxX, maxY);
    }

    public void MoveTo(Vector3 target)
    {
        IsDestination = false;

        // 경로 찾고
        PathFinding(target);

        // _finalNodeList 대로 이동한다.
        if (_coMove != null)
            StopCoroutine(_coMove);
        _coMove = StartCoroutine(CoMove());
    }

    public void StopPathFinding()
    {
        if (_coMove != null)
            StopCoroutine(_coMove);
    }

    private void PathFinding(Vector3 target)
    {
        // NodeArray의 크기 정해주고, isWall, x, y 대입
        int sizeX = _end.x - _start.x + 1;
        int sizeY = _end.y - _start.y + 1;
        _nodeArray = new Node[sizeY, sizeX];

        for (int y = _end.y; y >= _start.y; y--)
        {
            for (int x = _end.x; x >= _start.x; x--)
            {
                bool isWall = _map[y - _start.y, x - _start.x] == null || !_map[y - _start.y, x - _start.x].isEmpty;
                _nodeArray[y - _start.y, x - _start.x] = new Node(isWall, x, y);
            }
        }

        Vector3Int startPoint = _grid.WorldToCell(transform.position);
        Vector3Int endPoint = _grid.WorldToCell(target);

        // 시작과 끝 노드, 열린리스트와 닫힌리스트, 마지막리스트 초기화
        _startNode = _nodeArray[startPoint.y - _start.y, startPoint.x - _start.x];
        _targetNode = _nodeArray[endPoint.y - _start.y, endPoint.x - _start.x];

        _closedList = new List<Node>();
        FinalNodeList = new List<Node>();

        _openPq = new PriorityQueue<Node>();
        _openPq.Enqueue(new Pair<Node, int>(_startNode, -_startNode.F));

        while (!_openPq.IsEmpty)
        {
            InfiniteLoopDetector.Run();
            // 열린리스트 중 가장 F가 작고 F가 같다면 H가 작은 걸 현재노드로 하고 열린리스트에서 닫힌리스트로 옮기기
            _curNode = _openPq.Dequeue().First;
            _closedList.Add(_curNode);

            // 마지막
            if (_curNode == _targetNode)
            {
                Node TargetCurNode = _targetNode;
                while (TargetCurNode != _startNode)
                {
                    FinalNodeList.Add(TargetCurNode);
                    TargetCurNode = TargetCurNode.ParentNode;
                }
                FinalNodeList.Add(_startNode);
                FinalNodeList.Reverse();

                return;
            }


            OpenListAdd(_curNode.x + 1, _curNode.y + 1);
            OpenListAdd(_curNode.x - 1, _curNode.y + 1);
            OpenListAdd(_curNode.x - 1, _curNode.y - 1);
            OpenListAdd(_curNode.x + 1, _curNode.y - 1);

            OpenListAdd(_curNode.x, _curNode.y + 1);
            OpenListAdd(_curNode.x + 1, _curNode.y);
            OpenListAdd(_curNode.x, _curNode.y - 1);
            OpenListAdd(_curNode.x - 1, _curNode.y);
        }
    }

    private void OpenListAdd(int checkX, int checkY)
    {
        // 상하좌우 범위를 벗어나지 않고, 벽이 아니면서, 닫힌리스트에 없다면
        if (checkX >= _start.x && checkX < _end.x + 1 && checkY >= _start.y && checkY < _end.y + 1 && !_nodeArray[checkY - _start.y, checkX - _start.x].isWall 
            && !_closedList.Contains(_nodeArray[checkY - _start.y, checkX - _start.x]))
        {
            if (_nodeArray[checkY - _start.y, checkX - _start.x].isWall && _nodeArray[checkY - _start.y, checkX - _start.x].isWall) 
                return;

            // 이웃노드에 넣고, 일직선 거리
            Node NeighborNode = _nodeArray[checkY - _start.y, checkX - _start.x];
            int MoveCost = _curNode.G + (int)Mathf.Sqrt(Mathf.Pow(_targetNode.x - _curNode.x, 2) + Mathf.Pow(_targetNode.y - _curNode.y, 2));

            // 이동비용이 이웃노드G보다 작거나 또는 열린리스트에 이웃노드가 없다면 G, H, ParentNode를 설정 후 열린리스트에 추가
            if (MoveCost < NeighborNode.G || !_openPq.Contains(NeighborNode))
            {
                NeighborNode.G = MoveCost;
                NeighborNode.H = (Mathf.Abs(NeighborNode.x - _targetNode.x) + Mathf.Abs(NeighborNode.y - _targetNode.y)) * 10;
                NeighborNode.ParentNode = _curNode;

                _openPq.Enqueue(new Pair<Node, int>(NeighborNode, -NeighborNode.F));
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if (FinalNodeList.Count != 0)
        {
            for (int i = 0; i < FinalNodeList.Count - 1; i++)
            {
                Vector3 s = _grid.CellToWorld(new Vector3Int(FinalNodeList[i].x, FinalNodeList[i].y, 0));
                Vector3 d = _grid.CellToWorld(new Vector3Int(FinalNodeList[i + 1].x, FinalNodeList[i + 1].y, 0));
                Gizmos.DrawLine(s, d);
            }
        }
    }

    private IEnumerator CoMove()
    {
        if(FinalNodeList.Count == 0)
        {
            Debug.LogWarning("그곳으로 갈 경로가 없습니다.");
            yield break;
        }

        bool isFlip = false;
        bool isBack = false;

        for(int i = 0; i < FinalNodeList.Count; i++)
        {
            Vector3 s = transform.position;
            Vector3 d = _grid.CellToWorld(new Vector3Int(FinalNodeList[i].x, FinalNodeList[i].y, 0));
            float currentTime = 0;

            isBack = s.y > d.y;
            if(s.x != d.x)
                isFlip = s.x > d.x;

            _controller.CharacterAnimator.FlipX(isFlip);

            string animationName = isBack ? "walk" : "walk_back";

            _controller.CharacterAnimator.PlayAnimation(isBack ? "walk" : "walk_back");

            while(Vector3.Distance(transform.position, d) > 0.01f)
            {
                currentTime += Time.deltaTime;
                transform.position = Vector3.Lerp(s, d, currentTime * _moveSpeed);
                yield return null;
            }

            _controller.CharacterAnimator.SettingOrder();
            transform.position = d;
        }

        _controller.CharacterAnimator.PlayAnimation(isBack ? "idle" : "idle_back");

        IsDestination = true;
    }
}
