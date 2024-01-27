using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    protected override void Update()
    {
        base.Update();

        _shootTimer += Time.deltaTime;

        if (_shootTimer >= _shootTimerMax)
        {
            Shoot();
            _shootTimer = 0;
        }

        transform.LookAt(target);
    }

    public void Shoot()
    {
        // shoot logic
        GameObject bullet = Instantiate(_bullet, transform.position + transform.forward, Quaternion.identity);
        bullet.GetComponent<Bullet>().Activate(_bulletVelocity, transform.forward, transform, _damage);
    }
}
