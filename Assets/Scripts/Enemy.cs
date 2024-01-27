using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Entity
{
    [SerializeField]
    private Transform _target;

    private Vector3 _targetPosition;

    [SerializeField]
    private bool _isEnabled = true;

    public bool chaseTarget = true;

    private NavMeshAgent _agent;
    private float timer;

    [SerializeField]
    private float _timerMax;

    private void Awake()
    {
        _agent = this.GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if(_isEnabled)
        {
            if (chaseTarget)
            {
                _agent.SetDestination(_target.position);
            } else
            {
                MoveAround();
                _agent.SetDestination(_targetPosition);
            }
        }
    }

    public void MoveAround()
    {
        timer += Time.deltaTime;

        if (timer >= _timerMax)
        {
            GetRandomClosePosition();
            timer = 0;
        }
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    public void SetTarget(Vector3 target)
    {
        _targetPosition = target;
    }

    private void GetRandomClosePosition()
    {
        var target = RandomNavSphere(transform.position, 2, -1);
        SetTarget(target);
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask)
    {
        Vector3 randomDirection = Random.insideUnitSphere * distance;

        randomDirection += origin;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);

        return navHit.position;
    }
}
