using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : ShootingEnemy
{
    [SerializeField]
    private float _teleportTimerMax;

    private float _teleportTimer;

    [SerializeField]
    private float _attack2TimerMax;

    private float _attack2Timer;

    [SerializeField]
    private List<Transform> _teleportPositions = new List<Transform>();

    [SerializeField]
    private GameObject _detonateObjectPrefab;

    IEnumerator WaitForPlayer()
    {
        GameObject[] arr = GameObject.FindGameObjectsWithTag("spawnspot");
        foreach(GameObject g in arr)
        {
            _teleportPositions.Add(g.transform);
        }
        
        yield return new WaitForSeconds(1);
        SetTarget(FindFirstObjectByType<FPSController>().transform);
        this._isEnabled = true;
    }

    protected override void Start()
    {
        base.Start();
        StartCoroutine(WaitForPlayer());
    }

    public void Teleport()
    {
        var randomSpot = _teleportPositions[Random.Range(0, _teleportPositions.Count - 1)];
        transform.position = randomSpot.position;
    }

    protected override void Update()
    {
        if (_isEnabled && target != null)
        {
            _teleportTimer += Time.deltaTime;

            if (_teleportTimer >= _teleportTimerMax)
            {
                Teleport();
                _teleportTimer = 0;
            }

            ShootLogic();

            UpdateAttack2Timer();

            base.Update();
        }
    }

    private void UpdateAttack2Timer()
    {

        _attack2Timer += Time.deltaTime;

        if (_attack2Timer >= _attack2TimerMax)
        {
            Attack2();
            _attack2Timer = 0;
        }
    }

    protected void Attack2()
    {
        var g = Instantiate(_detonateObjectPrefab, target.transform.position - new Vector3(0,1,0), Quaternion.identity);
        // activate detonate timer

    }

}
