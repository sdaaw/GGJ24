using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Entity
{
    [SerializeField]
    Transform _target;

    [SerializeField]
    bool _isEnabled = true;

    private NavMeshAgent _agent;

    private void Awake()
    {
        _agent = this.GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if(_isEnabled)
        {
            // if(_agent.destination != _target.position)
            _agent.SetDestination(_target.position);
        }
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }
}
