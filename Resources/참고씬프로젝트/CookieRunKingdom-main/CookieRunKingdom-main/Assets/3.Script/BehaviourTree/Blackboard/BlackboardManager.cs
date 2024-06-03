using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackboard<BlackboardKeyType>
{
    //저장공간
    private Dictionary<BlackboardKeyType, object> _genericValues = new Dictionary<BlackboardKeyType, object>();

    //키 설정하기
    public void SetGeneric<T>(BlackboardKeyType key, T value)
    {
        _genericValues[key] = value;
    }
    // 키 가져오기
    public T GetGeneric<T>(BlackboardKeyType key)
    {
        object value;
        if (_genericValues.TryGetValue(key, out value))
            return (T)value;

        Debug.Log("해당 키에 해당하는 값이 없습니다");
        return default(T);
    }

}

public abstract class BlackboardKeyBase
{ 

}

public class BlackboardManager : MonoBehaviour
{
    public static BlackboardManager Instance { get; private set; } = null;

    //개인 정보
    private Dictionary<MonoBehaviour, object> _individualBlackboards = new Dictionary<MonoBehaviour, object>();
    //공유 정보
    private Dictionary<int, object> _sharedBlackboards = new Dictionary<int, object>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public Blackboard<T> GetIndividualBlackboard<T>(MonoBehaviour requestor) where T : BlackboardKeyBase, new()
    {
        if (!_individualBlackboards.ContainsKey(requestor))
            _individualBlackboards[requestor] = new Blackboard<T>();

        return _individualBlackboards[requestor] as Blackboard<T>;
    }

    public Blackboard<T> GetSharedBlackboard<T>(int uniqueID) where T : BlackboardKeyBase, new()
    {
        if (!_sharedBlackboards.ContainsKey(uniqueID))
            _sharedBlackboards[uniqueID] = new Blackboard<T>();

        return _sharedBlackboards[uniqueID] as Blackboard<T>;
    }


}