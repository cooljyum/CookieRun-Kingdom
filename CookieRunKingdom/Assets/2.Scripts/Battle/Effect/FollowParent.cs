using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowParent : MonoBehaviour
{
    private Transform parentTransform;

    private void Awake()
    {
        parentTransform = transform.parent;
    }

    private void Update()
    {
        if (parentTransform != null)
        {
            // Update the child's position to match the parent's position
            transform.position = parentTransform.position;
        }
    }
}
