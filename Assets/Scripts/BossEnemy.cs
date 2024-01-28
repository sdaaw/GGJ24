using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : ShootingEnemy
{
    [SerializeField]
    private float _teleportTimerMax;

    private float _teleportTimer;

    [SerializeField]
    private List<Transform> _teleportPositions = new List<Transform>();

    IEnumerator WaitForPlayer()
    {
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

            base.Update();
        }
    }

}
