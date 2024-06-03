using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    private static bool isApplicationQuit = false;

    public static T Instance
    {
        get
        {
            if (isApplicationQuit == true)
                return null;

            if (instance == null)
            {
                T[] _finds = FindObjectsOfType<T>();
                if (_finds.Length > 0)
                {
                    instance = _finds[0];
                    DontDestroyOnLoad(instance.gameObject);
                }

                if (_finds.Length > 1)
                {
                    for (int i = 1; i < _finds.Length; i++)
                        Destroy(_finds[i].gameObject);
                }

                if (instance == null)
                {
                    GameObject _createGameObject = new GameObject(typeof(T).Name);
                    DontDestroyOnLoad(_createGameObject);
                    instance = _createGameObject.AddComponent<T>();
                }
            }
            return instance;
        }
    }

    protected virtual void OnApplicationQuit()
    {
        isApplicationQuit = true;
    }
}

