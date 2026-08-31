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

    private bool _goalTriggered;

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (_colliders.Contains(collision))
    //    {
    //        _onGoalEntered?.Invoke();
    //    }
    //}

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (_goalTriggered || !_colliders.Contains(collision))
        {
            return;
        }

        Draggable draggable = collision.GetComponentInParent<Draggable>();

        // Wait until the player releases the object.
        if (draggable != null && draggable.IsBeingDragged)
        {
            return;
        }

        _goalTriggered = true;
        _onGoalEntered?.Invoke();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_colliders.Contains(collision))
        {
            _goalTriggered = false;
        }
    }
}
