using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShootingEnemy : Enemy
{
    [SerializeField]
    private GameObject _bullet;

    [SerializeField]
    private float _bulletVelocity;

    [SerializeField]
    private float _damage;

    private float _shootTimer;

    [SerializeField]
    private float _shootTimerMax;

    // [SerializeField]
    // private float _FOV = 90;

    [SerializeField]
    protected NavMeshAgent Agent;

    protected override void Awake()
    {
        base.Awake();
        Agent = GetComponent<NavMeshAgent>();
    }

    protected override void Update()
    {
        base.Update();

        CheckLoSToPlayer();

        _shootTimer += Time.deltaTime;

        if (_shootTimer >= _shootTimerMax)
        {
            Shoot();
            _shootTimer = 0;
        }

        //TODO: need to change this, so enemies can turn around while moving to show samuels art
        transform.LookAt(target);
    }

    public void Shoot()
    {
        GameObject bullet = Instantiate(_bullet, transform.position + transform.forward, Quaternion.identity);
        bullet.GetComponent<Bullet>().Activate(_bulletVelocity, transform.forward, transform, _damage);
    }

    public void CheckLoSToPlayer()
    {
        RaycastHit hit;
        var rayDirection = target.position - transform.position;

        if (Physics.Raycast(transform.position, rayDirection, out hit))
        {
            if (hit.transform.GetComponent<FPSController>())
            {
                chaseTarget = false;
            }
            else
            {
                chaseTarget = true;
            }
        }

    }


    /*public int ColliderArraySortComparer(Collider A, Collider B)
    {
        if (A == null && B != null)
        {
            return 1;
        }
        else if (A != null && B == null)
        {
            return -1;
        }
        else if (A == null && B == null)
        {
            return 0;
        }
        else
        {
            return Vector3.Distance(Agent.transform.position, A.transform.position).CompareTo(Vector3.Distance(Agent.transform.position, B.transform.position));
        }
    }*/
}
