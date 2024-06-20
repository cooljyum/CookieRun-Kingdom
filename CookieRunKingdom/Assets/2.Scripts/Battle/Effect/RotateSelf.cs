using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSelf : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed;

    private void OnEnable()
    {
        transform.localRotation = Quaternion.identity;
    }

    private void Update()
    {
        transform.Rotate(0, 0, -_rotateSpeed * Time.deltaTime);
    }
}