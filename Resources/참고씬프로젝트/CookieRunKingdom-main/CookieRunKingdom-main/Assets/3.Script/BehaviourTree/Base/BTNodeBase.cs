using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class BTNodeBase : BTElementBase
{
    protected List<BTNodeBase> _children = new List<BTNodeBase>();
    protected List<BTDecoratorBase> _decorators = new List<BTDecoratorBase>();
    protected List<BTServiceBase> _services = new List<BTServiceBase>();

    //진입
    protected System.Func<BehaviorTree.ENodeStatus> onEnterFn;

    //업데이트
    protected System.Func<BehaviorTree.ENodeStatus> onTickFn;


    public BehaviorTree.ENodeStatus LastStatus { get; protected set; } = BehaviorTree.ENodeStatus.Unknown;
    public bool DecoratorsPermitRunning { get; protected set; } = true;

    //생성자
    public BTNodeBase(string _Name = "", System.Func<BehaviorTree.ENodeStatus> _OnEnterFn = null,
    System.Func<BehaviorTree.ENodeStatus> _OnTickFn = null)
    {
        Name = _Name;
        onEnterFn = _OnEnterFn;
        onTickFn = _OnTickFn;
    }



    //Node 추가하기
    public BTNodeBase Add<T>(string _Name,
        System.Func<BehaviorTree.ENodeStatus> _OnEnterFn = null,
        System.Func<BehaviorTree.ENodeStatus> _OnTickFn = null) where T : BTNodeBase, new()
    {
        T newNode = new T();
        newNode.Name = _Name;
        newNode.onEnterFn = _OnEnterFn;
        newNode.onTickFn = _OnTickFn;

        return Add(newNode);
    }

    public BTNodeBase Add<T>(T newNode) where T : BTNodeBase
    {
        _children.Add(newNode);
        return newNode;
    }


    public BTNodeBase AddService<T>(string _Name, System.Action<float> _OnTickFn) where T : BTServiceBase, new()
    {
        T newService = new T();
        newService.Init(_Name, _OnTickFn);

        _services.Add(newService);

        return this;
    }

    public BTNodeBase AddService<T>(T newService) where T : BTServiceBase
    {
        _services.Add(newService);

        return this;
    }

    public BTNodeBase AddDecorator<T>(string _Name, System.Func<bool> _OnEvaluateFn) where T : BTDecoratorBase, new()
    {
        T newDecorator = new T();
        newDecorator.Init(_Name, _OnEvaluateFn);

        _decorators.Add(newDecorator);

        return this;
    }

    public BTNodeBase AddDecorator<T>(T newDecorator) where T : BTDecoratorBase
    {
        _decorators.Add(newDecorator);

        return this;
    }

    private void TickServices(float deltaTime)
    {
        foreach (var service in _services)
            service.OnTick(deltaTime);
    }
    private bool EvaluateDecorators()
    {
        bool canRun = true;

        foreach (var decorator in _decorators)
        {
            canRun = decorator.Evaluate();
            if (!canRun)
                break;
        }

        if (canRun != DecoratorsPermitRunning)
        {
            DecoratorsPermitRunning = canRun;
            if (canRun)
                Reset();
        }

        return canRun;
    }
    protected virtual void OnAbort()
    {
        Reset();
    }

    public virtual void Reset()
    {
        LastStatus = BehaviorTree.ENodeStatus.Unknown;

        foreach (var child in _children)
            child.Reset();
    }

    public void Tick(float deltaTime)
    {
        bool tickedAnyNodes = OnTick(deltaTime);
        if (!tickedAnyNodes)
            Reset();
    }

    protected virtual void OnEnter()
    {
        if (onEnterFn != null)
            LastStatus = onEnterFn.Invoke();
        else
            LastStatus = _children.Count > 0 ? BehaviorTree.ENodeStatus.InProgress : BehaviorTree.ENodeStatus.Succeeded;
    }

    protected virtual bool OnTick(float deltaTime)
    {
        bool tickedAnyNodes = false;

        //데코레이터 조건을 만족하는지 검사
        if (!DecoratorsPermitRunning)
        {
            LastStatus = BehaviorTree.ENodeStatus.Failed;
            tickedAnyNodes = true;
            return tickedAnyNodes;
        }

        //서비스 업데이트하기
        TickServices(deltaTime);


        //처음 진입시
        if (LastStatus == BehaviorTree.ENodeStatus.Unknown) //기본값
        {
            OnEnter();
            tickedAnyNodes = true;

            if (LastStatus == BehaviorTree.ENodeStatus.Failed)
                return tickedAnyNodes;
        }

        // 진행 단계
        if (onTickFn != null)
        {
            LastStatus = onTickFn.Invoke();
            tickedAnyNodes = true;
            if (LastStatus != BehaviorTree.ENodeStatus.InProgress)
                return tickedAnyNodes;
        }

        // 자녀 노드가 없을 때
        if (_children.Count == 0)
        {
            if (onTickFn == null)
                LastStatus = BehaviorTree.ENodeStatus.Succeeded; //성공 상태로 업데이트

            return tickedAnyNodes; // 종료 
        }

        //실행
        for (int childIndex = 0; childIndex < _children.Count; ++childIndex)
        {

            //자식 노드 가져오기
            var child = _children[childIndex];

            //이전 데코레이터에서 성공 여부
            bool childPreviouslyEnabledByDecorators = child.DecoratorsPermitRunning;


            //canRun 반환값
            bool childCurrentlyEnabledByDecorators = child.EvaluateDecorators();

            /*          ChildPreviouslyEnabledByDecorators
                        1. false일 경우
                            그러면 여기서 EvaluateDecorators를 할 때 canRun은 true고
                            DecoratorsPermitRunning은 false인 상황이 발생해서 여기서 자식들을 다 초기화한 후 true로 변경
                        2. true일 경우
                           자식들 초기화 안하고 true
            */

            if (child.LastStatus == BehaviorTree.ENodeStatus.InProgress)
            {
                tickedAnyNodes |= child.OnTick(deltaTime);
                return tickedAnyNodes;
            }

            if (child.LastStatus != BehaviorTree.ENodeStatus.Unknown)
                continue;

            tickedAnyNodes |= child.OnTick(deltaTime);


            LastStatus = child.LastStatus;

            if (!childPreviouslyEnabledByDecorators && childCurrentlyEnabledByDecorators)
            {
                for (int futureIndex = childIndex + 1; futureIndex < _children.Count; ++futureIndex)
                {
                    var futureChild = _children[futureIndex];
                    if (futureChild.LastStatus == BehaviorTree.ENodeStatus.InProgress)
                        futureChild.OnAbort();
                    else
                        futureChild.Reset();
                }
            }


            if (child.LastStatus == BehaviorTree.ENodeStatus.InProgress)
                return tickedAnyNodes;

            else if (child.LastStatus == BehaviorTree.ENodeStatus.Failed &&
                !ContinueEvaluatingIfChildFailed())
            {
                return tickedAnyNodes;
            }
            else if (child.LastStatus == BehaviorTree.ENodeStatus.Succeeded &&
                !ContinueEvaluatingIfChildSucceeded())
            {
                return tickedAnyNodes;
            }
        }

        //상태 업데이트
        OnTickedAllChildren();

        return tickedAnyNodes;
    }

    protected virtual bool ContinueEvaluatingIfChildFailed()
    {
        return true;
    }

    protected virtual bool ContinueEvaluatingIfChildSucceeded()
    {
        return true;
    }

    protected virtual void OnTickedAllChildren()
    {

    }

    public string GetDebugText()
    {
        StringBuilder debugTextBuilder = new StringBuilder();
        GetDebugTextInternal(debugTextBuilder);
        return debugTextBuilder.ToString();
    }

    protected override void GetDebugTextInternal(StringBuilder debugTextBuilder, int indentLevel = 0)
    {
        for (int index = 0; index < indentLevel; ++index)
            debugTextBuilder.Append(' ');

        debugTextBuilder.Append($"{Name} [{LastStatus.ToString()}]");

        foreach (var service in _services)
        {
            debugTextBuilder.AppendLine();
            debugTextBuilder.Append(service.GetDebugText(indentLevel + 1));
        }

        foreach (var decorator in _decorators)
        {
            debugTextBuilder.AppendLine();
            debugTextBuilder.Append(decorator.GetDebugText(indentLevel + 1));
        }

        foreach (var child in _children)
        {
            debugTextBuilder.AppendLine();
            child.GetDebugTextInternal(debugTextBuilder, indentLevel + 2);
        }
    }

}
