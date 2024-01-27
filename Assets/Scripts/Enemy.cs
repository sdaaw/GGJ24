using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Entity
{
    [SerializeField]
    protected Transform target;

    [SerializeField]
    private Vector3 _targetPosition;

    [SerializeField]
    private bool _isEnabled = true;

    public bool chaseTarget = true;

    private NavMeshAgent _agent;
    private float timer;

    [SerializeField]
    private float _timerMax;

    protected virtual void Awake()
    {
        _agent = this.GetComponent<NavMeshAgent>();
    }

    protected virtual void Update()
    {
        if(_isEnabled)
        {
            if (chaseTarget)
            {
                _agent.SetDestination(target.position);
            }
            else
            {
                MoveAround();
                _agent.SetDestination(_targetPosition);
            }
        }
    }

    protected virtual void MoveAround()
    {
        timer += Time.deltaTime;

        if (timer >= _timerMax)
        {
            GetRandomClosePosition();
            timer = 0;
        }
    }

    public virtual void SetTarget(Transform target)
    {
        this.target = target;
    }

    public virtual void SetTarget(Vector3 target)
    {
        _targetPosition = target;
    }

    protected virtual void GetRandomClosePosition()
    {
        Vector3 target = RandomNavSphere(transform.position, 2, -1);
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
