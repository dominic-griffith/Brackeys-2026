using NUnit.Framework;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;

public class Failure : MonoBehaviour
{
    [Header("Accepted Colliders")]
    [SerializeField] private List<Collider2D> _colliders = new();

    [SerializeField] private UnityEvent _onFailureEntered;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_colliders.Contains(collision))
        {
            _onFailureEntered?.Invoke();
        }
    }
}
