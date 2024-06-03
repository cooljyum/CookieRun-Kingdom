using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    static private T _instance;
    static public T Instance
    {
        get
        {
            if (_instance == null)
            {                
                string name = typeof(T).Name;
                GameObject obj = new GameObject(name);
                _instance = obj.AddComponent<T>();
            }
    
            return _instance;
        }
    }
}
