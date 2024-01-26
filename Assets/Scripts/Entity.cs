using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{

    public float CurrentHealth
    {
        get
        {
            return _currentHealth;
        }
        set
        {
            _currentHealth = value;
            OnHealthChanged();
        }
    }

    [SerializeField]
    private float _currentHealth;

    private Renderer _renderer;

    private float _timer;

    void Start()
    {
        _renderer = GetComponent<Renderer>();
    }

    void Update()
    {
    }

    protected void OnHealthChanged()
    {
        StartCoroutine(DamageVisual());
        if (_currentHealth <= 0)
        {
            GameManager.instance.EntitiesInWorld.Remove(gameObject);
            Destroy(gameObject);
        }
    }

    private IEnumerator DamageVisual()
    {
        if (_renderer == null) yield return null;

        _renderer.material.color *= 2;
        yield return new WaitForSeconds(0.2f);
        _renderer.material.color /= 2;
    }
}
