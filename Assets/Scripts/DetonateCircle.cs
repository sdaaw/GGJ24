using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetonateCircle : MonoBehaviour
{
    public float damage;

    private bool _canDealDamage;

    private List<Entity> _alreadyDealtDamageTo = new List<Entity>();

    private float _detonateTimer = 0;
    public float detonateTimerMax = 1.5f;

    [SerializeField]
    private GameObject _explosionParticle;

    private void Update()
    {
        _detonateTimer += Time.deltaTime;

        if (_detonateTimer >= detonateTimerMax)
        {
            Explode();
            _detonateTimer = 0;
        }
    }

    protected void Explode()
    {
        // spawn explosion particle
        // deal damage
        _canDealDamage = true;
        SpawnExplosion();
        StartCoroutine(RemoveExplosion());
    }

    private void OnTriggerStay(Collider other)
    {
        Entity hit = other.GetComponent<Entity>();
        if (hit != null && _canDealDamage)
        {
            if (!_alreadyDealtDamageTo.Contains(hit))
            {
                // TODO: deal damage
                // Debug.Log(hit);
                hit.CurrentHealth -= damage;
                _alreadyDealtDamageTo.Add(hit);
            }
        }
    }

    private void SpawnExplosion()
    {
        if (_explosionParticle != null)
        {
            Instantiate(_explosionParticle, transform.position, Quaternion.identity);
        }
    }

    IEnumerator RemoveExplosion()
    {
        yield return new WaitForSeconds(1);
        Destroy(this.gameObject);
    }
}
