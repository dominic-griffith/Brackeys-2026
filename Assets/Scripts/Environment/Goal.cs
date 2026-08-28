using NUnit.Framework;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;

public class Goal : MonoBehaviour
{
    [Header("Accepted Colliders")]
    [SerializeField] private List<Collider2D> _colliders = new();

    [SerializeField] private UnityEvent _onGoalEntered;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_colliders.Contains(collision))
        {
            _onGoalEntered?.Invoke();
        }
    }
}
