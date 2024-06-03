using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingObject : MonoBehaviour
{
    public System.Action onDisable = null;

    private void OnDisable()
    {
        onDisable?.Invoke();
    }
}
